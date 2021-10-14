using Game.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsWindow : BaseWindow
{
    public TipsWindow()
    {
        selfType = WindowType.TipsWindow;
        scenesType = ScenesType.Login;
        resident = true;
        resName = "UI/Windows/TipsWindow";
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }
    Transform Tips;
    Text TipsText;
    protected override void Awake()
    {
        base.Awake();
        Tips = transform.Find("TipsWindow");
        TipsText = transform.Find("TipText").GetComponent<Text>();
    }

    protected override void OnAddListener()
    {
        base.OnAddListener();
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
    Action EnterOKBtn;
    private void EnterOKBtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        if (EnterOKBtn != null)
        {
            EnterOKBtn();
            EnterOKBtn = null;
        }
        else
        {
            Close();
        }
    }
    public void ShowTips(string text, Action EnterOKBtn = null)
    {
        TipsText.text = text;
        this.EnterOKBtn = EnterOKBtn;
    }
}
