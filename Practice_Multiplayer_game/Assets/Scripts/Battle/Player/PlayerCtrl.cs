using Assets.MCVlibrary.Model;
using DG.Tweening;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    Vector3 spawnPosition;
    Vector3 spawnRotation;
    public PlayerInfo PlayerInfo;
    public bool isSelf = false;
    public HeroAttributeEntity currentAttribute;
    public HeroAttributeEntity totalAttribute;
    public GameObject hud;
    public bool isJump = false;
    public bool isInvincible = false;
    public void Relive()
    {
        this.gameObject.SetActive(true);
        hud.SetActive(true);
        this.transform.position = spawnPosition;
        this.transform.eulerAngles = spawnRotation;
        //this.transform.GetComponent<Rigidbody>().useGravity = true;
        this.transform.GetComponent<BoxCollider>().enabled = true;       
        currentAttribute.HP = totalAttribute.HP;
        HudUpdate();
    }
    Text NickName;
    public Text HurtText;
    Image hpFill;
    public SkillManager skillManager;
    public AnimatorManager animatorManager;
    public PlayerFSM playerFSM;
    public InputCtrl inputCtrl;

    internal void Init(PlayerInfo playerInfo)
    {
        this.PlayerInfo = playerInfo;
        //英雄是不是自己的角色
        isSelf = PlayerModel.Instance.CheckIsSelf(playerInfo.RolesInfo.RolesID);
        //復活時會用到
        spawnPosition = transform.position;
        spawnRotation = transform.eulerAngles;

        //獲取他的屬性 當前屬性 還有總的屬性
        currentAttribute = HeroAttributeConfig.GetInstance(playerInfo.HeroID);
        totalAttribute = HeroAttributeConfig.GetInstance(playerInfo.HeroID);

        RoomModel.Instance.SaveHeroAttribute(playerInfo.RolesInfo.RolesID, currentAttribute, totalAttribute);

        //人物的HUD 血條 藍條 暱稱 等級
        hud = ResManager.Instance.LoadHUD();
        hud.transform.position = transform.position;
        hud.transform.eulerAngles = Camera.main.transform.eulerAngles;
        hpFill = hud.transform.Find("Hero_Head/HPBG/HP").GetComponent<Image>();
        NickName = hud.transform.Find("Hero_Head/NickName").GetComponent<Text>();
        HurtText = hud.transform.Find("Hero_Head/HurtText").GetComponent<Text>();
        HudUpdate(true);
        //技能管理器
        skillManager = this.gameObject.AddComponent<SkillManager>();
        skillManager.Init(this);
        //動畫管理器
        animatorManager = this.gameObject.AddComponent<AnimatorManager>();
        animatorManager.Init(this);
        //角色的狀態機
        playerFSM = this.gameObject.AddComponent<PlayerFSM>();
        playerFSM.Init(this);
        //輸入控制器
        inputCtrl = GameObject.Find("BattleManager").GetComponent<InputCtrl>();
    }

    public void HudUpdate(bool init = false)
    {
        if (PlayerInfo.TeamID == 0)
        {
            NickName.color = Color.green;
        }
        else
        {
            NickName.color = Color.blue;
        }
        NickName.text = PlayerInfo.RolesInfo.NickName;
        if (init == true)
        {
            hpFill.fillAmount = 1;
        }
        else
        {
            hpFill.DOFillAmount(currentAttribute.HP / totalAttribute.HP, 0.2f).SetAutoKill(true);
        }
    }
    public IEnumerator ShowDamage(int damage)
    {
        HurtText.color = Color.red;
        Vector3 originalPos = hpFill.transform.position;
        
        HurtText.text = "-" + damage;
        for (int i = 0; i < 20; i++)
        {
            HurtText.transform.position += new Vector3(0, 0.05f, 0);
            HurtText.transform.localScale += new Vector3(0.008f, 0.008f, 0.008f);
            yield return new WaitForSeconds(0.015f);
        }
        HurtText.text = "";
        HurtText.transform.position = originalPos;
        HurtText.transform.localScale = new Vector3(0.06492111f, 0.06492111f, 0.06492111f);
    }
    public IEnumerator ShowHeal(int heal)
    {
        HurtText.color = Color.green;
        Vector3 originalPos = hpFill.transform.position;

        HurtText.text = "+" + heal;
        for (int i = 0; i < 20; i++)
        {
            HurtText.transform.position += new Vector3(0, 0.05f, 0);
            HurtText.transform.localScale += new Vector3(0.008f, 0.008f, 0.008f);
            yield return new WaitForSeconds(0.015f);
        }
        HurtText.text = "";
        HurtText.transform.position = originalPos;
        HurtText.transform.localScale = new Vector3(0.06492111f, 0.06492111f, 0.06492111f);
    }

    //Vector3 cameraOffset = new Vector3(3.74f, 1.6f, 0);
    public void LateUpdate()
    {
        if (hud != null)
        {
            hud.transform.position = transform.position + new Vector3(0, 0.3f, -2);
            hud.transform.rotation = Camera.main.transform.rotation;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PConfig>())
        {
            PConfig pConfig = collision.gameObject.GetComponent<PConfig>();
            GameObject effect = ResManager.Instance.LoadPropsEffect(pConfig.PropsID, transform.position);
            PConfig pConfig1 = effect.GetComponent<PConfig>();
            pConfig1.playerCtrl = this;
        }
        if (collision.gameObject.tag == "Ground")
        {
            if (this.transform.position.y > collision.transform.position.y)
            {
                isJump = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (this.transform.position.y > collision.transform.position.y)
            {
                isJump = false;
            }
        }
    }
    /// <summary>
    /// 碰撞到技能触发器
    /// </summary>
    /// <param name="eConfig"></param>
    /// <param name="trrigerObject"></param>
    public void OnSkillTrriger(EConfig eConfig, GameObject trrigerObject) //打到敵人
    {
        bool isDestroy = false;
        //角色
        if (trrigerObject.CompareTag("Player"))
        {
            PlayerCtrl hitPlayerCtrl = trrigerObject.transform.GetComponent<PlayerCtrl>();
            PlayerInfo hitPlayerInfo = hitPlayerCtrl.PlayerInfo;
            //判断是否同个阵营的
            if (hitPlayerInfo.TeamID != PlayerInfo.TeamID)
            {
                if (eConfig.effectID != 100501)
                {

                }
                if (eConfig.destroyMode == DestroyMode.OnHit_DifferentCampPlayer || eConfig.destroyMode == DestroyMode.OnHit_AllPlayer)
                {
                    isDestroy = true;
                    //克隆爆炸特效
                    if (isDestroy && eConfig.hitLoad != null)
                    {
                        //克隆爆炸物
                        GameObject hitObj = GameObject.Instantiate(eConfig.hitLoad);
                        //hitObj.transform.position = trrigerObject.transform.position;
                        if (eConfig.effectID == 100200)
                        {
                            hitObj.transform.position = trrigerObject.transform.position + new Vector3(0, 0.5f, -2);
                        }
                        else
                        {
                            hitObj.transform.position = eConfig.moveRoot.gameObject.transform.position;
                        }
                        //并且销毁
                        Destroy(eConfig.gameObject);
                    }
                    else
                    {
                        Destroy(eConfig.gameObject);
                    }
                }
            }
            else
            {
                //如果是同个阵营的
                return;
            }
        }
    }
    private void OnTriggerEnter(Collider other) //被敵人打到
    {
        if (other.GetComponent<PConfig>())
        {
            PConfig pConfig = other.GetComponent<PConfig>();
            GameObject effect = ResManager.Instance.LoadPropsEffect(pConfig.PropsID, transform.position);
            PConfig pConfig1 = effect.GetComponent<PConfig>();
            pConfig1.playerCtrl = this;
        }
        if (isSelf == true && isInvincible == false)
        {
            //角色
            if (other.GetComponent<EConfig>())
            {
                EConfig eConfig = other.GetComponent<EConfig>();
                PlayerCtrl hitPlayerCtrl = eConfig.releaser;
                PlayerInfo hitPlayerInfo = hitPlayerCtrl.PlayerInfo;

                //判断是否同个阵营的
                if (hitPlayerInfo.TeamID != PlayerInfo.TeamID)
                {
                    if (eConfig.effectID != 100501)
                    {
                        int damage = (int)((int)(hitPlayerCtrl.skillManager.demageConfig[eConfig.gameObject.tag] * hitPlayerCtrl.currentAttribute.Power) / currentAttribute.Armor);
                        inputCtrl.SendHudInputC2S(damage, hitPlayerInfo.RolesInfo.RolesID); //傳給伺服器我被打到多少傷害 交給伺服器廣播我的血量
                    }
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
    /// <summary>
    /// 受到技能伤害 要减血
    /// </summary>
    /// <param name="hurt"></param>
    public void OnSkillHit(int demage)
    {
        //Debug.LogError($"攻擊傷害:{hurt} 防禦值:{currentAttribute.Armor} 造成傷害:{demage} ");       
        StartCoroutine(ShowDamage(demage));
    }
    public void OnHeal(int heal)
    {
        //Debug.LogError($"攻擊傷害:{hurt} 防禦值:{currentAttribute.Armor} 造成傷害:{demage} ");       
        StartCoroutine(ShowHeal(heal));
    }
    public void PropsEffectOver(int propsID,int delay)
    {
        string props = "Props" + propsID.ToString();
        Invoke(props, delay);
    }
    public void Props102()
    {
        currentAttribute.Power = currentAttribute.Power / 1.5f;
    }
    public void Props103()
    {
        currentAttribute.MoveSpeed = currentAttribute.MoveSpeed / 1.5f;
    }
    public void Props104()
    {
        isInvincible = false;
    }
}
