using Assets.MCVlibrary.Model;
using Game.View;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleWindow : BaseWindow
{
    public bool isOver;
    public BattleWindow()
    {
        selfType = WindowType.BattleWindow;
        scenesType = ScenesType.Battle;
        resident = false;
        resName = "UI/Windows/BattleWindow";
    }
    CancellationTokenSource ct;
    CancellationTokenSource deadct;
    int int_level = 0;
    int int_coin = 0;
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (time <= 3)
        {
            reciprocal.text = time.ToString();
            reciprocal.gameObject.SetActive(true);
        }
        if (time == 1)
        {
            Time.timeScale = 0.2f;
        }
        else
        {
            Time.timeScale = 1;
        }
        if (time <= 0)
        {
            Time.timeScale = 0;
            if (isOver == false)
            {
                TimeOver();
                isOver = true;
            }
        }
        Skill(J, KeyCode.J);
        Skill(K, KeyCode.K);
        Skill(L, KeyCode.L);
        AScore.text = A_score.ToString();
        BScore.text = B_score.ToString();
    }
    async void TimeOver() //遊戲結束處理
    {
        Camera.main.transform.position = new Vector3(-9.6f, 4.45f, -10);
        Camera.main.orthographicSize = 2.5f;
        playerCtrl.transform.position = new Vector3(-10, 0.6f, 2);
        playerCtrl.transform.eulerAngles = new Vector3(0, 180, 0);
        if (A_score > B_score)
        {
            if (playerInfo.TeamID == 0) //贏
            {
                reciprocal.text = "Win!";
                int_level = 2;
                int_coin = 100;
                Win.SetActive(true);        
                playerCtrl.animatorManager.Play(PlayerAnimationClip.victory);
            }
            else //輸
            {
                reciprocal.text = "Lose...";
                int_level = 1;
                int_coin = 50;
                Lose.SetActive(true);
                playerCtrl.animatorManager.Play(PlayerAnimationClip.die);
            }
        }
        else if (A_score < B_score)
        {
            if (playerInfo.TeamID == 0) //輸
            {
                reciprocal.text = "Lose...";
                int_level = 1;
                int_coin = 50;
                Lose.SetActive(true);
                playerCtrl.animatorManager.Play(PlayerAnimationClip.die);
            }
            else //贏
            {
                reciprocal.text = "Win!";
                int_level = 2;
                int_coin = 100;
                Win.SetActive(true);
                playerCtrl.animatorManager.Play(PlayerAnimationClip.victory);
            }
        }
        else
        {
            reciprocal.text = "Tie...";
            int_level = 1;
            int_coin = 50;
            Tie.SetActive(true);
            playerCtrl.animatorManager.Play(PlayerAnimationClip.die);
        }
        LevelText.text = (RolesCtrl.Instance.GetRolesInfo().Level + int_level).ToString();
        CoinText.text = (RolesCtrl.Instance.GetRolesInfo().GoldCoin + int_coin).ToString();
        damageText.text = BattleCtrl.Instance.GetDamage(playerInfo.RolesInfo.RolesID).ToString();
        KillsText.text = BattleCtrl.Instance.GetKills(playerInfo.RolesInfo.RolesID).ToString();
        await Task.Delay(2500);
        reciprocal.gameObject.SetActive(false);
        WinSreen.SetActive(true);
    }
    Image J, backJ, K, backK, L, backL;
    Text JName, KName, LName, AScore, BScore, Min, tenSec, Sec, reciprocal, LevelText, CoinText, damageText, KillsText,deadText;
    GameObject WinSreen,Win,Lose,Tie,deadSecond;
    int time;
    int deadTime;
    public int A_score, B_score;
    PlayerInfo playerInfo;
    PlayerCtrl playerCtrl;//自己本地的
    InputCtrl inputCtrl;
    protected override void Awake()
    {
        base.Awake();
        isOver = false;
        A_score = 0;
        B_score = 0;
        time = 180;
        deadTime = 10;
        playerCtrl = RoomCtrl.Instance.GetSelfPlayerCtrl();
        playerInfo = playerCtrl.PlayerInfo;
        inputCtrl = GameObject.Find("BattleManager").GetComponent<InputCtrl>();

        J = transform.Find("J底圖/J").GetComponent<Image>();
        backJ = transform.Find("J底圖").GetComponent<Image>();
        J.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "J");
        backJ.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "J");
        K = transform.Find("K底圖/K").GetComponent<Image>();
        backK = transform.Find("K底圖").GetComponent<Image>();
        K.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "K");
        backK.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "K");
        L = transform.Find("L底圖/L").GetComponent<Image>();
        backL = transform.Find("L底圖").GetComponent<Image>();
        L.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "L");
        backL.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "L");
        JName = transform.Find("J底圖/JName").GetComponent<Text>();
        JName.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(playerInfo.HeroID).Att_ID).SkillName;
        KName = transform.Find("K底圖/KName").GetComponent<Text>();
        KName.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(playerInfo.HeroID).Skill1_ID).SkillName;
        LName = transform.Find("L底圖/LName").GetComponent<Text>();
        LName.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(playerInfo.HeroID).Skill2_ID).SkillName;
        AScore = transform.Find("記分板/A").GetComponent<Text>();
        BScore = transform.Find("記分板/B").GetComponent<Text>();
        Min = transform.Find("記分板/時間/1分").GetComponent<Text>();
        tenSec = transform.Find("記分板/時間/10秒").GetComponent<Text>();
        Sec = transform.Find("記分板/時間/1秒").GetComponent<Text>();
        reciprocal = transform.Find("倒數").GetComponent<Text>();
        LevelText = transform.Find("結算畫面/Level/LevelText").GetComponent<Text>();
        CoinText = transform.Find("結算畫面/Coin/CoinText").GetComponent<Text>();
        damageText = transform.Find("結算畫面/Damage/DamageText").GetComponent<Text>();
        KillsText = transform.Find("結算畫面/Kills/KillsText").GetComponent<Text>();
        WinSreen = transform.Find("結算畫面").gameObject;
        Win = transform.Find("結算畫面/Win").gameObject;
        Lose = transform.Find("結算畫面/Lose").gameObject;
        Tie = transform.Find("結算畫面/Tie").gameObject;
        deadSecond = transform.Find("死亡倒數BG").gameObject;
        deadText = transform.Find("死亡倒數BG/Text").GetComponent<Text>();
        ct = new CancellationTokenSource();
        deadct = new CancellationTokenSource();
        TimeDown();
    }

    async void TimeDown()
    {
        while (time > 0)
        {
            await Task.Delay(1000); //每隔一秒
            if (!ct.IsCancellationRequested)
            {
                time -= 1;
                int min = time / 60;
                int tensec = time % 60 / 10;
                int sec = (time % 60) - (tensec * 10);
                Min.text = min.ToString();
                tenSec.text = tensec.ToString();
                Sec.text = sec.ToString();
            }
        }
    }
    public void DeadHandle()
    {
        deadSecond.SetActive(true);
        DeadTimeDown(); 
    }

    async void DeadTimeDown()
    {
        while (deadTime > 0)
        {
            await Task.Delay(1000); //每隔一秒
            if (!deadct.IsCancellationRequested)
            {
                deadTime -= 1;
                deadText.text = deadTime.ToString();
            }
        }
        if (isOver == true)
        {
            deadSecond.SetActive(false);
            deadTime = 10;
            playerCtrl.isDead = false;
            return;
        }
        if (deadTime <= 0)
        {
            deadSecond.SetActive(false);
            deadTime = 10;
            playerCtrl.isDead = false;
        }
    }
    void Skill(Image image,KeyCode key) //計算技能CD
    {
        switch (key)
        {
            case KeyCode.J:
                if (inputCtrl.isJ == false)
                {
                    image.fillAmount = playerCtrl.skillManager.SurplusTime(key) / playerCtrl.skillManager.coolingConfig[key];
                    if (image.fillAmount == 1)
                    {
                        inputCtrl.isJ = true;
                    }
                }
                break;
            case KeyCode.K:
                if (inputCtrl.isK == false)
                {
                    image.fillAmount = playerCtrl.skillManager.SurplusTime(key) / playerCtrl.skillManager.coolingConfig[key];
                    if (image.fillAmount == 1)
                    {
                        inputCtrl.isK = true;
                    }
                }
                break;
            case KeyCode.L:
                if (inputCtrl.isL == false)
                {
                    image.fillAmount = playerCtrl.skillManager.SurplusTime(key) / playerCtrl.skillManager.coolingConfig[key];
                    if (image.fillAmount == 1)
                    {
                        inputCtrl.isL = true;
                    }
                }
                break;
            default:
                break;
        }        
    }
    protected override void OnAddListener()
    {
        base.OnAddListener();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ct.Cancel();
        deadct.Cancel();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnRemoveListener()
    {
        base.OnRemoveListener();
    }

    protected override void RegisterUIEvent()
    {
        base.RegisterUIEvent();
        for (int i = 0; i < buttonList.Length; i++)
        {
            switch (buttonList[i].name)
            {
                case "OKBtn":
                    buttonList[i].onClick.AddListener(EnterOKBtnOnClick);
                    break;
                default:
                    break;
            }
        }
    }

    private void EnterOKBtnOnClick()
    {
        AudioManager.Instance.OKBtn();
        Time.timeScale = 1;

        RolesInfo rolesInfo = RolesCtrl.Instance.GetRolesInfo();
        rolesInfo.Level = RolesCtrl.Instance.GetRolesInfo().Level + int_level;
        rolesInfo.GoldCoin = RolesCtrl.Instance.GetRolesInfo().GoldCoin + int_coin;
        RolesCtrl.Instance.SaveRolesInfo(rolesInfo);
        inputCtrl.SendBattleOverC2S(RolesCtrl.Instance.GetRolesInfo().Level, RolesCtrl.Instance.GetRolesInfo().GoldCoin);

        BattleListener.Instance.Release();
        BattleModel.Instance.Clear();
        WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
        SceneManager.LoadScene("Lobby");
        Close();
    }
}
