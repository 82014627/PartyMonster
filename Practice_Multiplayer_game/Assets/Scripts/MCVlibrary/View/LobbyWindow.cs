using Game.Net;
using Game.View;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyWindow : BaseWindow
{
    public LobbyWindow()
    {
        selfType = WindowType.LobbyWindow;
        scenesType = ScenesType.Login;
        resident = true;
        resName = "UI/Windows/LobbyWindow";
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }
    Transform BattleBtn, ShopBtn, FriendBtn, CancelBattleBtn, MatchTips;
    Text NickName, Level, GoldCoinCount, DiamondsCount;
    protected override void Awake()
    {
        base.Awake();

        BattleBtn = transform.Find("BattleBtn");
        ShopBtn = transform.Find("ShopBtn");
        FriendBtn = transform.Find("FriendBtn");
        CancelBattleBtn = transform.Find("CancelBattleBtn");

        NickName = transform.Find("NickName/NickName_text").GetComponent<Text>();
        Level = transform.Find("Level/Level_Count").GetComponent<Text>();
        GoldCoinCount = transform.Find("GoldCoin/GoldCoin_Count").GetComponent<Text>();
        DiamondsCount = transform.Find("Diamonds/Diamonds_Count").GetComponent<Text>();
        MatchTips = transform.Find("MatchTips");
    }

    protected override void OnAddListener()
    {
        base.OnAddListener();
        NetEvent.Instance.AddEventListener(1300, HandleLobbyToMatchS2C);
        NetEvent.Instance.AddEventListener(1301, HandleLobbyUpdateMatchStateS2C);
        NetEvent.Instance.AddEventListener(1302, HandleLobbyQuitMatchS2C);
    }

    private void HandleLobbyQuitMatchS2C(BufferEntity response)
    {
        LobbyQuitMatchS2C s2cMSG = ProtobufHelper.FromBytes<LobbyQuitMatchS2C>(response.proto);
        if (s2cMSG.Result == 0)
        {
            BattleBtn.gameObject.SetActive(true);
            MatchTips.gameObject.SetActive(false);
            CancelBattleBtn.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("退出匹配有問題");
        }
    }

    private void HandleLobbyUpdateMatchStateS2C(BufferEntity response)
    {
        LobbyUpdateMatchStateS2C s2cMSG = ProtobufHelper.FromBytes<LobbyUpdateMatchStateS2C>(response.proto);
        if (s2cMSG.Result == 0)
        {
            BattleBtn.gameObject.SetActive(true);
            CancelBattleBtn.gameObject.SetActive(false);
            MatchTips.gameObject.SetActive(false);

            //房間訊息
            RolesCtrl.Instance.SaveRoomInfo(s2cMSG.RoomInfo);

            Close();
            WindowManager.Instance.OpenWindow(WindowType.RoomWindow);
        }
        else
        {
            Debug.Log("無法更新匹配狀態");
        }
    }

    private void HandleLobbyToMatchS2C(BufferEntity response)
    {
        LobbyToMatchS2C s2cMSG = ProtobufHelper.FromBytes<LobbyToMatchS2C>(response.proto);
        if (s2cMSG.Result == 0)
        {
            BattleBtn.gameObject.SetActive(false);
            CancelBattleBtn.gameObject.SetActive(true);
            MatchTips.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("無法加入匹配");//無法進行匹配 可能是被懲罰 需要等待
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RolesInfo rolesInfo = RolesCtrl.Instance.GetRolesInfo();
        NickName.text = rolesInfo.NickName;
        Level.text = rolesInfo.Level.ToString();
        GoldCoinCount.text = rolesInfo.GoldCoin.ToString();
        DiamondsCount.text = rolesInfo.Diamonds.ToString();
    }

    protected override void OnRemoveListener()
    {
        base.OnRemoveListener();
        NetEvent.Instance.RemoveEventListener(1300, HandleLobbyToMatchS2C);
        NetEvent.Instance.RemoveEventListener(1301, HandleLobbyUpdateMatchStateS2C);
        NetEvent.Instance.RemoveEventListener(1302, HandleLobbyQuitMatchS2C);
    }

    protected override void RegisterUIEvent()
    {
        base.RegisterUIEvent();
        for (int i = 0; i < buttonList.Length; i++)
        {
            switch (buttonList[i].name)
            {
                case "BattleBtn":
                    buttonList[i].onClick.AddListener(BattleBtnOnClick);
                    break;
                case "CancelBattleBtn":
                    buttonList[i].onClick.AddListener(CancelBattleBtnOnClick);
                    break;
                case "ShopBtn":
                    buttonList[i].onClick.AddListener(ShopBtnOnClick);
                    break;
                case "FriendBtn":
                    buttonList[i].onClick.AddListener(FriendBtnOnClick);
                    break;
                case "SettingBtn":
                    buttonList[i].onClick.AddListener(SettingBtnOnClick);
                    break;
                case "CharacterBtn":
                    buttonList[i].onClick.AddListener(CharacterBtnOnClick);
                    break;
                case "TeachBtn":
                    buttonList[i].onClick.AddListener(TeachBtnOnClick);
                    break;
                default:
                    break;
            }
        }
    }

    private void TeachBtnOnClick()
    {
        AudioManager.Instance.OKBtn();
        WindowManager.Instance.OpenWindow(WindowType.TeachWindow);
    }

    private void CharacterBtnOnClick()
    {
        AudioManager.Instance.OKBtn();
        Close();
        WindowManager.Instance.OpenWindow(WindowType.CharacterWindow);
    }

    private void SettingBtnOnClick()
    {
        AudioManager.Instance.OKBtn();
        WindowManager.Instance.ShowTips("尚未開發此功能");
    }

    private void FriendBtnOnClick()
    {
        AudioManager.Instance.OKBtn();
        WindowManager.Instance.ShowTips("尚未開發此功能");
    }

    private void ShopBtnOnClick()
    {
        AudioManager.Instance.OKBtn();
        WindowManager.Instance.ShowTips("尚未開發此功能");
    }

    private void CancelBattleBtnOnClick()
    {
        AudioManager.Instance.BackBtn();
        BufferFactory.CreateAndSendPackage(1302, new LobbyQuitMatchC2S());
    }

    private void BattleBtnOnClick()
    {
        AudioManager.Instance.OKBtn();
        BufferFactory.CreateAndSendPackage(1300, new LobbyToMatchC2S());
    }
}
