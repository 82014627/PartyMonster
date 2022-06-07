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
    public bool isCanJump = false;
    public bool isInvincible = false;
    public bool isDead = false;
    public void Relive()
    {
        isDead = false;
        this.gameObject.SetActive(true);
        hud.SetActive(true);
        this.transform.position = spawnPosition;
        this.transform.eulerAngles = spawnRotation;
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
    /// <summary>
    /// 血量UI更新
    /// </summary>
    /// <param name="init"></param>
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
    /// <summary>
    /// 顯示傷害
    /// </summary>
    /// <param name="damage">傷害</param>
    /// <returns></returns>
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
    /// <summary>
    /// 顯示回復血量
    /// </summary>
    /// <param name="heal">回復血量</param>
    /// <returns></returns>
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
    /// <summary>
    /// 判斷掉下去死亡 以及血量位置轉向更新
    /// </summary>
    public void LateUpdate()
    {
        if (transform.position.x < 2 || transform.position.y < -2 || transform.position.x > 23 || transform.position.y < -2)
        {
            if (isSelf == true && isDead == false)
            {
                BattleWindow battleWindow = (BattleWindow)WindowManager.Instance.GetWindow(WindowType.BattleWindow);
                if (battleWindow.isOver == false)
                {
                    inputCtrl.SendHudInputC2S(1000, 0);
                    isDead = true;
                }
            }
        }
        if (hud != null)
        {
            hud.transform.position = transform.position + new Vector3(0, 0.3f, -2);
            hud.transform.rotation = Camera.main.transform.rotation;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (this.transform.position.y > collision.transform.position.y)
            {
                isCanJump = true;
            }
        }
        if (collision.gameObject.GetComponent<PConfig>())
        {
            PConfig pConfig = collision.gameObject.GetComponent<PConfig>();
            GameObject effect = ResManager.Instance.LoadPropsEffect(pConfig.PropsID, transform.position);
            PConfig pConfig1 = effect.GetComponent<PConfig>();
            pConfig1.playerCtrl = this;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (this.transform.position.y > collision.transform.position.y)
            {
                isCanJump = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (this.transform.position.y > collision.transform.position.y)
            {
                isCanJump = false;
            }
        }
    }
    /// <summary>
    /// 碰撞到技能觸發器
    /// </summary>
    /// <param name="eConfig"></param>
    /// <param name="trrigerObject"></param>
    public void OnSkillTrigger(EConfig eConfig, GameObject trrigerObject) //打到敵人
    {
        bool isDestroy = false;
        //角色
        if (trrigerObject.CompareTag("Player"))
        {
            PlayerCtrl hitPlayerCtrl = trrigerObject.transform.GetComponent<PlayerCtrl>();
            PlayerInfo hitPlayerInfo = hitPlayerCtrl.PlayerInfo;
            //判斷是否同個陣營
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
                        if (eConfig.effectID == 100200)
                        {
                            hitObj.transform.position = trrigerObject.transform.position + new Vector3(0, 0.5f, -2);
                        }
                        else
                        {
                            hitObj.transform.position = eConfig.moveRoot.gameObject.transform.position;
                        }
                        //並且銷毀
                        Destroy(eConfig.gameObject);
                    }
                    else
                    {
                        Destroy(eConfig.gameObject);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other) //被敵人打到
    {
        if (other.GetComponent<PConfig>())
        {
            PConfig pConfig = other.GetComponent<PConfig>();
            if (pConfig.PropsID == 105)
            {
                return;
            }
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

                //判斷是否同陣營
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
    /// 受到傷害 顯示扣血
    /// </summary>
    /// <param name="hurt"></param>
    public void OnSkillHit(int demage)
    {
        Debug.LogError($"造成傷害:{demage}");       
        StartCoroutine(ShowDamage(demage));
    }
    /// <summary>
    /// 顯示治癒 
    /// </summary>
    /// <param name="heal"></param>
    public void OnHeal(int heal)
    {
        StartCoroutine(ShowHeal(heal));
    }
    /// <summary>
    /// 道具效果結束
    /// </summary>
    /// <param name="propsID"></param>
    /// <param name="delay"></param>
    public void PropsEffectOver(int propsID, int delay)
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
