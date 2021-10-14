using Game.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterWindow : BaseWindow
{
    public CharacterWindow()
    {
        selfType = WindowType.CharacterWindow;
        scenesType = ScenesType.Login;
        resident = false;
        resName = "UI/Windows/CharacterWindow";
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
    }
    GameObject Choose, Role;
    int HeroID;
    Image Hero, J, K, L;
    Text HeroName, HpText, PowerText, 
        DefenseText, AttackSpeedText, MoveSpeedText,
        JText, JSkill, JCDText, KText, KSkill, KCDText, LText, LSkill, LCDText;
    protected override void Awake()
    {
        base.Awake();
        Choose = transform.Find("Choose").gameObject;
        Role = transform.Find("Role").gameObject;
        Hero = transform.Find("Role/Hero").GetComponent<Image>();
        J = transform.Find("Role/Hero/J").GetComponent<Image>();
        K = transform.Find("Role/Hero/K").GetComponent<Image>();
        L = transform.Find("Role/Hero/L").GetComponent<Image>();
        HeroName = transform.Find("Role/Hero/Name").GetComponent<Text>();
        HpText = transform.Find("Role/Hero/Hp/HpText").GetComponent<Text>();
        PowerText = transform.Find("Role/Hero/Power/PowerText").GetComponent<Text>();
        DefenseText = transform.Find("Role/Hero/Defense/DefenseText").GetComponent<Text>();
        AttackSpeedText = transform.Find("Role/Hero/AttackSpeed/AttackSpeedText").GetComponent<Text>();
        MoveSpeedText = transform.Find("Role/Hero/MoveSpeed/MoveSpeedText").GetComponent<Text>();
        JText = transform.Find("Role/Hero/J/JText").GetComponent<Text>();
        JSkill = transform.Find("Role/Hero/J/JSkill").GetComponent<Text>();
        JCDText = transform.Find("Role/Hero/J/JCD/JCDText").GetComponent<Text>();
        KText = transform.Find("Role/Hero/K/KText").GetComponent<Text>();
        KSkill = transform.Find("Role/Hero/K/KSkill").GetComponent<Text>();
        KCDText = transform.Find("Role/Hero/K/KCD/KCDText").GetComponent<Text>();
        LText = transform.Find("Role/Hero/L/LText").GetComponent<Text>();
        LSkill = transform.Find("Role/Hero/L/LSkill").GetComponent<Text>();
        LCDText = transform.Find("Role/Hero/L/LCD/LCDText").GetComponent<Text>();
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
        base.RegisterUIEvent();
        for (int i = 0; i < buttonList.Length; i++)
        {
            switch (buttonList[i].name)
            {
                case "1001":
                    buttonList[i].onClick.AddListener(Enter1001BtnOnClick);
                    buttonList[i].onClick.AddListener(EnterBtnOnClick);
                    break;
                case "1002":
                    buttonList[i].onClick.AddListener(Enter1002BtnOnClick);
                    buttonList[i].onClick.AddListener(EnterBtnOnClick);
                    break;
                case "1003":
                    buttonList[i].onClick.AddListener(Enter1003BtnOnClick);
                    buttonList[i].onClick.AddListener(EnterBtnOnClick);
                    break;
                case "1004":
                    buttonList[i].onClick.AddListener(Enter1004BtnOnClick);
                    buttonList[i].onClick.AddListener(EnterBtnOnClick);
                    break;
                case "1005":
                    buttonList[i].onClick.AddListener(Enter1005BtnOnClick);
                    buttonList[i].onClick.AddListener(EnterBtnOnClick);
                    break;
                case "BackToLobbyBtn":
                    buttonList[i].onClick.AddListener(EnterLobbyBtnOnClick);
                    break;
                case "BackToChooseBtn":
                    buttonList[i].onClick.AddListener(EnterChooseBtnOnClick);
                    break;
                default:
                    break;
            }
        }
    }

    private void Enter1001BtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        HeroID = 1001;
    }

    private void Enter1002BtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        HeroID = 1002;
    }

    private void Enter1003BtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        HeroID = 1003;
    }

    private void Enter1004BtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        HeroID = 1004;
    }

    private void Enter1005BtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        HeroID = 1005;
    }

    private void EnterChooseBtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        Choose.SetActive(true);
        Role.SetActive(false);
    }

    private void EnterLobbyBtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().BackBtn();
        Close();
        WindowManager.Instance.OpenWindow(WindowType.LobbyWindow);
    }

    private void EnterBtnOnClick()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().OKBtn();
        Hero.sprite = ResManager.Instance.LoadRoundHead(HeroID.ToString());
        J.sprite = ResManager.Instance.LoadSkillIcon(HeroID, "J");
        K.sprite = ResManager.Instance.LoadSkillIcon(HeroID, "K");
        L.sprite = ResManager.Instance.LoadSkillIcon(HeroID, "L");
        HeroName.text = HeroAttributeConfig.GetInstance(HeroID).HeroName;
        HpText.text = HeroAttributeConfig.GetInstance(HeroID).HP.ToString();
        PowerText.text = (HeroAttributeConfig.GetInstance(HeroID).Power*10).ToString();
        DefenseText.text = (HeroAttributeConfig.GetInstance(HeroID).Armor * 10).ToString();
        AttackSpeedText.text = HeroAttributeConfig.GetInstance(HeroID).AttackSpeed.ToString();
        MoveSpeedText.text = HeroAttributeConfig.GetInstance(HeroID).MoveSpeed.ToString();
        JText.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(HeroID).Att_ID).SkillName;
        JSkill.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(HeroID).Att_ID).SkillInfo;
        JCDText.text = HeroAttributeConfig.GetInstance(HeroID).AttackSpeed.ToString() + " Sec";
        KText.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(HeroID).Skill1_ID).SkillName;
        KSkill.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(HeroID).Skill1_ID).SkillInfo;
        KCDText.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(HeroID).Skill1_ID).CoolingTime.ToString() + " Sec";
        LText.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(HeroID).Skill2_ID).SkillName;
        LSkill.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(HeroID).Skill2_ID).SkillInfo;
        LCDText.text = AllSkillConfig.GetInstance(HeroSkillConfig.GetInstance(HeroID).Skill2_ID).CoolingTime.ToString() + " Sec";

        Choose.SetActive(false);
        Role.SetActive(true);
    }
}
