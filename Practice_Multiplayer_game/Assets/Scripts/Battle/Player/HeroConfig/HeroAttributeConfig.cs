
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HeroAttributeConfig
{

    static HeroAttributeConfig()
    {
        
       HeroAttributeEntity HeroAttributeEntity0 = new HeroAttributeEntity();
       HeroAttributeEntity0.ID = 1001;
       HeroAttributeEntity0.HeroName = @"火眼";
       HeroAttributeEntity0.Occupation = @"法師";
       HeroAttributeEntity0.HP = 750f;
       HeroAttributeEntity0.Power = 1.3f;
       HeroAttributeEntity0.Armor = 1.2f;
       HeroAttributeEntity0.AttackSpeed = 1.4f;
       HeroAttributeEntity0.MoveSpeed = 14f;//718.5

        if (!entityDic.ContainsKey(HeroAttributeEntity0.ID))
        {
          entityDic.Add(HeroAttributeEntity0.ID, HeroAttributeEntity0);
        }

       HeroAttributeEntity HeroAttributeEntity1 = new HeroAttributeEntity();
       HeroAttributeEntity1.ID = 1002;
       HeroAttributeEntity1.HeroName = @"刺刺龜";
       HeroAttributeEntity1.Occupation = @"坦克";
       HeroAttributeEntity1.HP = 800f;
       HeroAttributeEntity1.Power = 1.2f;
       HeroAttributeEntity1.Armor = 1.5f;
       HeroAttributeEntity1.AttackSpeed = 1f;
       HeroAttributeEntity1.MoveSpeed = 11f;//814.7

        if (!entityDic.ContainsKey(HeroAttributeEntity1.ID))
        {
          entityDic.Add(HeroAttributeEntity1.ID, HeroAttributeEntity1);
        }

       HeroAttributeEntity HeroAttributeEntity2 = new HeroAttributeEntity();
       HeroAttributeEntity2.ID = 1003;
       HeroAttributeEntity2.HeroName = @"紫袍";
       HeroAttributeEntity2.Occupation = @"法師";
       HeroAttributeEntity2.HP = 750f;
       HeroAttributeEntity2.Power = 1.5f;
       HeroAttributeEntity2.Armor = 1.1f;
       HeroAttributeEntity2.AttackSpeed = 1.5f;
       HeroAttributeEntity2.MoveSpeed = 15f;//719.1

        if (!entityDic.ContainsKey(HeroAttributeEntity2.ID))
        {
          entityDic.Add(HeroAttributeEntity2.ID, HeroAttributeEntity2);
        }

       HeroAttributeEntity HeroAttributeEntity3 = new HeroAttributeEntity();
       HeroAttributeEntity3.ID = 1004;
       HeroAttributeEntity3.HeroName = @"布丁狗";
       HeroAttributeEntity3.Occupation = @"鬥士";
       HeroAttributeEntity3.HP = 800f;
       HeroAttributeEntity3.Power = 1.3f;
       HeroAttributeEntity3.Armor = 1.3f;
       HeroAttributeEntity3.AttackSpeed = 0.75f;
       HeroAttributeEntity3.MoveSpeed = 13f;//815.35

        if (!entityDic.ContainsKey(HeroAttributeEntity3.ID))
        {
          entityDic.Add(HeroAttributeEntity3.ID, HeroAttributeEntity3);
        }

       HeroAttributeEntity HeroAttributeEntity4 = new HeroAttributeEntity();
       HeroAttributeEntity4.ID = 1005;
       HeroAttributeEntity4.HeroName = @"懶鳥";
       HeroAttributeEntity4.Occupation = @"坦克";
       HeroAttributeEntity4.HP = 850f;
       HeroAttributeEntity4.Power = 1.1f;
       HeroAttributeEntity4.Armor = 1.4f;
       HeroAttributeEntity4.AttackSpeed = 0.8f;
       HeroAttributeEntity4.MoveSpeed = 10f;//863.3

        if (!entityDic.ContainsKey(HeroAttributeEntity4.ID))
        {
          entityDic.Add(HeroAttributeEntity4.ID, HeroAttributeEntity4);
        }

    }

    
    static Dictionary<int, HeroAttributeEntity> entityDic = new Dictionary<int, HeroAttributeEntity>();
    public static HeroAttributeEntity Get(int key)
    {
        if (entityDic.ContainsKey(key))
        {
            return entityDic[key];
        }
        return null;
    }

    
   
    public static HeroAttributeEntity GetInstance(int key)
    {
        HeroAttributeEntity instance = new HeroAttributeEntity();
        if (entityDic.ContainsKey(key))
        {
            
            instance.ID = entityDic[key].ID;
            instance.HeroName = entityDic[key].HeroName;
            instance.Occupation = entityDic[key].Occupation;
            instance.HP = entityDic[key].HP;
            instance.Power = entityDic[key].Power;
            instance.Armor = entityDic[key].Armor;
            instance.AttackSpeed = entityDic[key].AttackSpeed;
            instance.MoveSpeed = entityDic[key].MoveSpeed;

        }
        return instance;
    }
}


public class HeroAttributeEntity
{
    //TemplateMember
    public int ID;//英雄ID
    public string HeroName;//英雄名稱
    public string Occupation;//職業名稱
    public float HP;//生命
    public float Power;//力量
    public float Armor;//防禦值
    public float AttackSpeed;//攻速
    public float MoveSpeed;//跑速

}
