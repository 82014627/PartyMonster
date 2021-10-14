using Game.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeachWindow : BaseWindow
{
    GameObject[] Contents= {null, null, null, null, null};
    int i;
    GameObject TeachWindows;
    Button BackBtn, NextBtn;

    public TeachWindow()
    {
        selfType = WindowType.TeachWindow;
        scenesType = ScenesType.Login;
        resident = false;
        resName = "UI/Windows/TeachWindow";
    }

    public void Next()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        if (i == 4)
        {
            Close();
        }
        int now = i + 1;
        if (now > 4)
        {
            return;
        }
        Contents[now].SetActive(true);
        Contents[i].SetActive(false);
        i = now;
    }
    public void Back()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().BackBtn();
        int now = i - 1;
        if (now < 0)
        {
            return;
        }
        Contents[now].SetActive(true);
        Contents[i].SetActive(false);
        i = now;
    }

    protected override void Awake()
    {
        base.Awake();
        i = 0;
        Contents[0] = transform.Find("GameTeachWindow/Gamecontent").gameObject;
        Contents[1] = transform.Find("GameTeachWindow/Skillcontent").gameObject;
        Contents[2] = transform.Find("GameTeachWindow/Propscontent").gameObject;
        Contents[3] = transform.Find("GameTeachWindow/Props_content").gameObject;
        Contents[4] = transform.Find("GameTeachWindow/Last_content").gameObject;
        BackBtn = transform.Find("GameTeachWindow/Back").GetComponent<Button>();
        NextBtn = transform.Find("GameTeachWindow/Next").GetComponent<Button>();
        TeachWindows = transform.Find("GameTeachWindow").gameObject;
    }

    protected override void RegisterUIEvent()
    {
        base.RegisterUIEvent();
        for (int i = 0; i < buttonList.Length; i++)
        {
            switch (buttonList[i].name)
            {
                case "Next":
                    buttonList[i].onClick.AddListener(Next);
                    break;
                case "Back":
                    buttonList[i].onClick.AddListener(Back);
                    break;
                default:
                    break;
            }
        }
    }

    protected override void OnAddListener()
    {
        base.OnAddListener();
    }

    protected override void OnRemoveListener()
    {
        base.OnRemoveListener();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        switch (i)
        {
            case 0:
                BackBtn.gameObject.SetActive(false);
                break;
            case 1:
                BackBtn.gameObject.SetActive(true);
                NextBtn.gameObject.SetActive(true);
                break;
            case 3:
                NextBtn.gameObject.transform.Find("Text").GetComponent<Text>().text = "Next";
                break;
            case 4:
                NextBtn.gameObject.transform.Find("Text").GetComponent<Text>().text = "Over";
                break;
            default:
                break;
        }
    }
}
