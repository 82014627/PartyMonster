using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;
    PlayerInfo playerInfo;

    Dictionary<KeyCode, int> skillID = new Dictionary<KeyCode, int>();
    public Dictionary<string, float> demageConfig = new Dictionary<string, float>();
    public Dictionary<KeyCode, float> coolingConfig = new Dictionary<KeyCode, float>();

    public void Init(PlayerCtrl playerCtrl)
    {
        this.playerCtrl = playerCtrl;
        this.playerInfo = playerCtrl.PlayerInfo;

        HeroSkillEntity heroSkillEntity = HeroSkillConfig.GetInstance(playerInfo.HeroID); //技能的配置信息

        skillID[KeyCode.Mouse0] = heroSkillEntity.Att_ID;
        skillID[KeyCode.Q] = heroSkillEntity.Skill1_ID;
        skillID[KeyCode.E] = heroSkillEntity.Skill2_ID;

        demageConfig["Mouse0"] = AllSkillConfig.GetInstance(heroSkillEntity.Att_ID).Damage;
        demageConfig["Q"] = AllSkillConfig.GetInstance(heroSkillEntity.Skill1_ID).Damage;
        demageConfig["E"] = AllSkillConfig.GetInstance(heroSkillEntity.Skill2_ID).Damage;

        coolingConfig[KeyCode.Mouse0] = HeroAttributeConfig.GetInstance(playerInfo.HeroID).AttackSpeed;
        coolingConfig[KeyCode.Q] = AllSkillConfig.GetInstance(heroSkillEntity.Skill1_ID).CoolingTime;
        coolingConfig[KeyCode.E] = AllSkillConfig.GetInstance(heroSkillEntity.Skill2_ID).CoolingTime;

        keyDownTime[KeyCode.Mouse0] = 0;
        keyDownTime[KeyCode.Q] = 0;
        keyDownTime[KeyCode.E] = 0;
    }

    Dictionary<KeyCode, float> keyDownTime = new Dictionary<KeyCode, float>();
    public void DoCooling(KeyCode key)//, Action<float> action)
    {
        keyDownTime[key] = Time.time;
        //if (action != null)
        //{
        //    action(keyDownTime[key]);
        //}
    }
    public float SurplusTime(KeyCode key)
    {
        //總的配置時間-按下到現在已經過去的時間-剩餘的冷卻時間
        float time = coolingConfig[key] - (Time.time - keyDownTime[key]);
        if (time <= 0)
        {
            time = 0;
        }
        return coolingConfig[key] - time;
    }
    public bool IsCooling(KeyCode key)
    {
        return SurplusTime(key) > 0;
    }
    public int GetID(KeyCode key)
    {
        return skillID[key];
    }
}
