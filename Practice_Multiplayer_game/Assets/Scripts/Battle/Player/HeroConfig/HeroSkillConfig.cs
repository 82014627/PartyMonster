
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroSkillConfig
{

    static HeroSkillConfig()
    {
        
       HeroSkillEntity HeroSkillEntity0 = new HeroSkillEntity();
       HeroSkillEntity0.ID = 1001;
       HeroSkillEntity0.Att_ID = 2;
       HeroSkillEntity0.Skill1_ID = 100101;
       HeroSkillEntity0.Skill2_ID = 100102;

        if (!entityDic.ContainsKey(HeroSkillEntity0.ID))
        {
          entityDic.Add(HeroSkillEntity0.ID, HeroSkillEntity0);
        }

       HeroSkillEntity HeroSkillEntity1 = new HeroSkillEntity();
       HeroSkillEntity1.ID = 1002;
       HeroSkillEntity1.Att_ID = 2;
       HeroSkillEntity1.Skill1_ID = 100201;
       HeroSkillEntity1.Skill2_ID = 100202;

        if (!entityDic.ContainsKey(HeroSkillEntity1.ID))
        {
          entityDic.Add(HeroSkillEntity1.ID, HeroSkillEntity1);
        }

       HeroSkillEntity HeroSkillEntity2 = new HeroSkillEntity();
       HeroSkillEntity2.ID = 1003;
       HeroSkillEntity2.Att_ID = 2;
       HeroSkillEntity2.Skill1_ID = 100301;
       HeroSkillEntity2.Skill2_ID = 100302;

        if (!entityDic.ContainsKey(HeroSkillEntity2.ID))
        {
          entityDic.Add(HeroSkillEntity2.ID, HeroSkillEntity2);
        }

       HeroSkillEntity HeroSkillEntity3 = new HeroSkillEntity();
       HeroSkillEntity3.ID = 1004;
       HeroSkillEntity3.Att_ID = 1;
       HeroSkillEntity3.Skill1_ID = 100401;
       HeroSkillEntity3.Skill2_ID = 100402;

        if (!entityDic.ContainsKey(HeroSkillEntity3.ID))
        {
          entityDic.Add(HeroSkillEntity3.ID, HeroSkillEntity3);
        }

       HeroSkillEntity HeroSkillEntity4 = new HeroSkillEntity();
       HeroSkillEntity4.ID = 1005;
       HeroSkillEntity4.Att_ID = 1;
       HeroSkillEntity4.Skill1_ID = 100501;
       HeroSkillEntity4.Skill2_ID = 100502;

        if (!entityDic.ContainsKey(HeroSkillEntity4.ID))
        {
          entityDic.Add(HeroSkillEntity4.ID, HeroSkillEntity4);
        }

    }

    
    static Dictionary<int, HeroSkillEntity> entityDic = new Dictionary<int, HeroSkillEntity>();
    public static HeroSkillEntity Get(int key)
    {
        if (entityDic.ContainsKey(key))
        {
            return entityDic[key];
        }
        return null;
    }

    
   
    public static HeroSkillEntity GetInstance(int key)
    {
        HeroSkillEntity instance = new HeroSkillEntity();
        if (entityDic.ContainsKey(key))
        {
            
            instance.ID = entityDic[key].ID;
            instance.Att_ID = entityDic[key].Att_ID;
            instance.Skill1_ID = entityDic[key].Skill1_ID;
            instance.Skill2_ID = entityDic[key].Skill2_ID;

        }
        return instance;
    }
}


public class HeroSkillEntity
{
    //TemplateMember
    public int ID;//英雄ID
    public int Att_ID;//普攻的ID
    public int Skill1_ID;//技能1的ID
    public int Skill2_ID;//技能2的ID

}
