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

        skillID[KeyCode.J] = heroSkillEntity.Att_ID;
        skillID[KeyCode.K] = heroSkillEntity.Skill1_ID;
        skillID[KeyCode.L] = heroSkillEntity.Skill2_ID;

        demageConfig["J"] = AllSkillConfig.GetInstance(heroSkillEntity.Att_ID).Damage;
        demageConfig["K"] = AllSkillConfig.GetInstance(heroSkillEntity.Skill1_ID).Damage;
        demageConfig["L"] = AllSkillConfig.GetInstance(heroSkillEntity.Skill2_ID).Damage;

        coolingConfig[KeyCode.J] = HeroAttributeConfig.GetInstance(playerInfo.HeroID).AttackSpeed;
        coolingConfig[KeyCode.K] = AllSkillConfig.GetInstance(heroSkillEntity.Skill1_ID).CoolingTime;
        coolingConfig[KeyCode.L] = AllSkillConfig.GetInstance(heroSkillEntity.Skill2_ID).CoolingTime;

        keyDownTime[KeyCode.J] = 0;
        keyDownTime[KeyCode.K] = 0;
        keyDownTime[KeyCode.L] = 0;
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
