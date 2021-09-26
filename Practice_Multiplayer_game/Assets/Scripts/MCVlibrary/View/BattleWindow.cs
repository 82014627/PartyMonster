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
    public BattleWindow()
    {
        selfType = WindowType.BattleWindow;
        scenesType = ScenesType.Battle;
        resident = false;
        resName = "UI/Windows/BattleWindow";
    }
    CancellationTokenSource ct;
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
            reciprocal.gameObject.SetActive(false);
            Time.timeScale = 0;
            TimeOver();
        }
        Skill(Mouse0, KeyCode.Mouse0);
        Skill(Q, KeyCode.Q);
        Skill(E, KeyCode.E);
        AScore.text = A_score.ToString();
        BScore.text = B_score.ToString();
    }
    void TimeOver()
    {
        if (A_score > B_score)
        {
            if (playerInfo.TeamID == 0)
            {
                int_level = 2;
                int_coin = 100;
                Win.SetActive(true);
            }
            else
            {
                int_level = 1;
                int_coin = 50;
                Lose.SetActive(true);
            }
        }
        else if (A_score < B_score)
        {
            if (playerInfo.TeamID == 0)
            {
                int_level = 1;
                int_coin = 50;
                Lose.SetActive(true);
            }
            else
            {
                int_level = 2;
                int_coin = 100;
                Win.SetActive(true);
            }
        }
        else
        {
            int_level = 1;
            int_coin = 50;
            Tie.SetActive(true);
        }
        LevelText.text = (RolesCtrl.Instance.GetRolesInfo().Level + int_level).ToString();
        CoinText.text = (RolesCtrl.Instance.GetRolesInfo().GoldCoin + int_coin).ToString();
        damageText.text = BattleCtrl.Instance.GetDamage(playerInfo.RolesInfo.RolesID).ToString();
        KillsText.text = BattleCtrl.Instance.GetKills(playerInfo.RolesInfo.RolesID).ToString();
        WinSreen.SetActive(true);
    }
    Image Mouse0, backMouse0, Q, backQ, E, backE;
    Text MouseName, QName, EName, AScore, BScore, Min, tenSec, Sec, reciprocal, LevelText, CoinText, damageText, KillsText;
    GameObject WinSreen,Win,Lose,Tie;
    int time;
    public int A_score, B_score;
    PlayerInfo playerInfo;
    PlayerCtrl playerCtrl;//自己本地的
    InputCtrl inputCtrl;
    protected override void Awake()
    {
        base.Awake();
        time = 300;
        playerCtrl = RoomCtrl.Instance.GetSelfPlayerCtrl();
        playerInfo = playerCtrl.PlayerInfo;
        inputCtrl = GameObject.Find("BattleManager").GetComponent<InputCtrl>();

        Mouse0 = transform.Find("左鍵底圖/Mouse0").GetComponent<Image>();
        backMouse0 = transform.Find("左鍵底圖").GetComponent<Image>();
        Mouse0.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "Mouse0");
        backMouse0.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "Mouse0");
        Q = transform.Find("Q底圖/Q").GetComponent<Image>();
        backQ = transform.Find("Q底圖").GetComponent<Image>();
        Q.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "Q");
        backQ.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "Q");
        E = transform.Find("E底圖/E").GetComponent<Image>();
        backE = transform.Find("E底圖").GetComponent<Image>();
        E.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "E");
        backE.sprite = ResManager.Instance.LoadSkillIcon(playerCtrl.PlayerInfo.HeroID, "E");
        MouseName = transform.Find("左鍵底圖/MouseName").GetComponent<Text>();
        MouseName.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(playerInfo.HeroID).Att_ID).SkillName;
        QName = transform.Find("Q底圖/QName").GetComponent<Text>();
        QName.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(playerInfo.HeroID).Skill1_ID).SkillName;
        EName = transform.Find("E底圖/EName").GetComponent<Text>();
        EName.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(playerInfo.HeroID).Skill2_ID).SkillName;
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
        ct = new CancellationTokenSource();
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
    void Skill(Image image,KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Mouse0:
                if (inputCtrl.isMouse0 == false)
                {
                    image.fillAmount = playerCtrl.skillManager.SurplusTime(key) / playerCtrl.skillManager.coolingConfig[key];
                    if (image.fillAmount == 1)
                    {
                        inputCtrl.isMouse0 = true;
                    }
                }
                break;
            case KeyCode.Q:
                if (inputCtrl.isQ == false)
                {
                    image.fillAmount = playerCtrl.skillManager.SurplusTime(key) / playerCtrl.skillManager.coolingConfig[key];
                    if (image.fillAmount == 1)
                    {
                        inputCtrl.isQ = true;
                    }
                }
                break;
            case KeyCode.E:
                if (inputCtrl.isE == false)
                {
                    image.fillAmount = playerCtrl.skillManager.SurplusTime(key) / playerCtrl.skillManager.coolingConfig[key];
                    if (image.fillAmount == 1)
                    {
                        inputCtrl.isE = true;
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
        Time.timeScale = 1;

        RolesInfo rolesInfo = RolesCtrl.Instance.GetRolesInfo();
        rolesInfo.Level = RolesCtrl.Instance.GetRolesInfo().Level + int_level;
        rolesInfo.GoldCoin = RolesCtrl.Instance.GetRolesInfo().GoldCoin + int_coin;
        RolesCtrl.Instance.SaveRolesInfo(rolesInfo);
        inputCtrl.SendBattleOverC2S(RolesCtrl.Instance.GetRolesInfo().Level, RolesCtrl.Instance.GetRolesInfo().GoldCoin);

        BattleListener.Instance.Release();
        BattleModel.Instance.Clear();
        Close();
        WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
        SceneManager.LoadScene("Lobby");
    }
}
