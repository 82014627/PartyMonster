using Assets.MCVlibrary.Controller;
using Game.Net;
using Game.View;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class LoginWindow : BaseWindow
    {
        public LoginWindow()
        {
            resName = "UI/Windows/LoginWindow";
            resident = false;
            selfType = WindowType.LoginWindow;
            scenesType = ScenesType.Login;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
        InputField AccountInput;
        InputField PwdInput;
        protected override void Awake()
        {
            base.Awake();
            AccountInput = transform.Find("UserBack/AccountInput").GetComponent<InputField>();
            PwdInput = transform.Find("UserBack/pwdInput").GetComponent<InputField>();
        }

        protected override void OnAddListener()
        {
            base.OnAddListener();
            NetEvent.Instance.AddEventListener(1000, handleUserRegisterS2C);
            NetEvent.Instance.AddEventListener(1001, handleUserLoginS2C);
        }
        /// <summary>
        /// 處理登入結果
        /// </summary>
        /// <param name="response"></param>
        private void handleUserLoginS2C(BufferEntity response)
        {
            UserLoginS2C s2cMSG = ProtobufHelper.FromBytes<UserLoginS2C>(response.proto);
            switch (s2cMSG.Result)
            {
                case 0:
                    Debug.Log("登入成功!");
                    if (s2cMSG.RolesInfo != null)
                    {
                        LoginCtrl.Instance.SaveRolesInfo(s2cMSG.RolesInfo);//保存角色數據
                        SceneManagers.LoginToLobby();
                        WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);//打開大廳介面
                    }
                    else
                    {
                        WindowManager.Instance.OpenWindow(WindowType.RolesWindow);
                    }

                    Close();
                    break;
                case 1:
                    Debug.Log("存在敏感字詞");
                    WindowManager.Instance.ShowTips("存在敏感字詞!");//打開提示窗體 提示
                    break;
                case 2:
                    Debug.Log("帳號密碼不匹配");
                    WindowManager.Instance.ShowTips("帳號密碼不匹配!"); 
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 處理註冊結果
        /// </summary>
        /// <param name="response"></param>
        private void handleUserRegisterS2C(BufferEntity response)
        {
            UserRegisterS2C s2cMSG = ProtobufHelper.FromBytes<UserRegisterS2C>(response.proto);
            switch (s2cMSG.Result)
            {
                case 0:
                    Debug.Log("註冊成功!");
                    WindowManager.Instance.ShowTips("註冊成功!");
                    break;
                case 1:
                    Debug.Log("存在敏感字詞");
                    WindowManager.Instance.ShowTips("存在敏感字詞!");
                    break;
                case 2:
                    Debug.Log("長度不夠");
                    WindowManager.Instance.ShowTips("長度不夠!");
                    break;
                case 3:
                    Debug.Log("帳號已被註冊");
                    WindowManager.Instance.ShowTips("帳號已被註冊!");
                    break;
                default:
                    break;
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
        }
        protected override void RegisterUIEvent()
        {
            base.RegisterUIEvent();
            for (int i = 0; i < buttonList.Length; i++)
            {
                switch (buttonList[i].name)
                {
                    case "Register_button":
                        buttonList[i].onClick.AddListener(RegisterBtnOnClick);
                        break;
                    case "login_button":
                        buttonList[i].onClick.AddListener(LoginBtnOnClick);
                        break;
                    default:
                        break;
                }
            }

        }
        private void LoginBtnOnClick()
        {
            AudioManager.Instance.OKBtn();
            if (string.IsNullOrEmpty(AccountInput.text))
            {
                Debug.Log("帳號不可為空");
                WindowManager.Instance.ShowTips("帳號不可為空!");
                return;
            }
            if (string.IsNullOrEmpty(PwdInput.text))
            {
                Debug.Log("密碼不可為空");
                WindowManager.Instance.ShowTips("密碼不可為空!");
                return;
            }
            UserLoginC2S c2sMSG = new UserLoginC2S();
            c2sMSG.UserInfo = new UserInfo();
            c2sMSG.UserInfo.Account = AccountInput.text;
            c2sMSG.UserInfo.Password = PwdInput.text;

            BufferFactory.CreateAndSendPackage(1001, c2sMSG);
        }

        private void RegisterBtnOnClick()
        {
            AudioManager.Instance.OKBtn();
            if (string.IsNullOrEmpty(AccountInput.text))
            {
                Debug.Log("帳號不可為空");
                WindowManager.Instance.ShowTips("帳號不可為空!");
                return;
            }
            if (string.IsNullOrEmpty(PwdInput.text))
            {
                Debug.Log("密碼不可為空");
                WindowManager.Instance.ShowTips("密碼不可為空!");
                return;
            }
            UserRegisterC2S c2sMSG = new UserRegisterC2S();
            c2sMSG.UserInfo = new UserInfo();
            c2sMSG.UserInfo.Account = AccountInput.text;
            c2sMSG.UserInfo.Password = PwdInput.text;

            BufferFactory.CreateAndSendPackage(1000, c2sMSG);
        }
    }
}

