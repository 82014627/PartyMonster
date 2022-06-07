using Game.Net;
using Game.View;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RolesWindow : BaseWindow
{
    public RolesWindow()
    {
        selfType = WindowType.RolesWindow;
        scenesType = ScenesType.Login;
        resident = false;
        resName = "UI/Windows/RolesWindow";
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }
    InputField nickName_inputField;
    protected override void Awake()
    {
        base.Awake();
        nickName_inputField = transform.Find("NickName_InputField").GetComponent<InputField>();
    }

    protected override void OnAddListener()
    {
        base.OnAddListener();
        NetEvent.Instance.AddEventListener(1201, HandleRolesCreateS2C);
    }

    private void HandleRolesCreateS2C(BufferEntity response)
    {
        RolesCreateS2C s2cMSG = ProtobufHelper.FromBytes<RolesCreateS2C>(response.proto);

        if (s2cMSG.Result == 0)
        {
            //緩存角色
            RolesCtrl.Instance.SaveRolesInfo(s2cMSG.RolesInfo);
            //關閉當前這個窗口
            Close();
            WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
        }
        else
        {
            Debug.Log("角色已經存在 創建失敗");
            WindowManager.Instance.ShowTips("存在相同的角色名 創建失敗");
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnRemoveListener()
    {
        base.OnRemoveListener();
        NetEvent.Instance.RemoveEventListener(1201, HandleRolesCreateS2C);
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
        RolesCreateC2S c2sMSG = new RolesCreateC2S();
        c2sMSG.NickName = nickName_inputField.text;

        BufferFactory.CreateAndSendPackage(1201, c2sMSG);
    }
}
