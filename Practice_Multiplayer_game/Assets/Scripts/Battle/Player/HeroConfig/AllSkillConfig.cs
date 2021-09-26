
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AllSkillConfig
{

    static AllSkillConfig()
    {
        
       AllSkillEntity AllSkillEntity0 = new AllSkillEntity();
       AllSkillEntity0.ID = 1;
       AllSkillEntity0.SkillName = @"近攻";
       AllSkillEntity0.SkillInfo = "近距離攻擊";
       AllSkillEntity0.CoolingTime = 0f;
       AllSkillEntity0.AttackDis = 0f;
       AllSkillEntity0.Damage = 60f;

        if (!entityDic.ContainsKey(AllSkillEntity0.ID))
        {
          entityDic.Add(AllSkillEntity0.ID, AllSkillEntity0);
        }

       AllSkillEntity AllSkillEntity1 = new AllSkillEntity();
       AllSkillEntity1.ID = 2;
       AllSkillEntity1.SkillName = @"遠攻";
       AllSkillEntity1.SkillInfo = "遠距離攻擊";
       AllSkillEntity1.CoolingTime = 0f;
       AllSkillEntity1.AttackDis = 0f;
       AllSkillEntity1.Damage = 40f;

        if (!entityDic.ContainsKey(AllSkillEntity1.ID))
        {
          entityDic.Add(AllSkillEntity1.ID, AllSkillEntity1);
        }

       AllSkillEntity AllSkillEntity2 = new AllSkillEntity();
       AllSkillEntity2.ID = 100101;
       AllSkillEntity2.SkillName = @"獄火焚身";
       AllSkillEntity2.SkillInfo = "周圍產生火焰，持續5秒，敵人靠近會造成每秒60點的傷害";
       AllSkillEntity2.CoolingTime = 15f;
       AllSkillEntity2.AttackDis = 0f;
       AllSkillEntity2.Damage = 60f;

        if (!entityDic.ContainsKey(AllSkillEntity2.ID))
        {
          entityDic.Add(AllSkillEntity2.ID, AllSkillEntity2);
        }

       AllSkillEntity AllSkillEntity3 = new AllSkillEntity();
       AllSkillEntity3.ID = 100102;
       AllSkillEntity3.SkillName = @"地爆天星";
       AllSkillEntity3.SkillInfo = "向前方扔一顆著火的隕石，敵人碰到會造成120點的傷害";
       AllSkillEntity3.CoolingTime = 28f;
       AllSkillEntity3.AttackDis = 0f;
       AllSkillEntity3.Damage = 120f;

        if (!entityDic.ContainsKey(AllSkillEntity3.ID))
        {
          entityDic.Add(AllSkillEntity3.ID, AllSkillEntity3);
        }

       AllSkillEntity AllSkillEntity4 = new AllSkillEntity();
       AllSkillEntity4.ID = 100201;
       AllSkillEntity4.SkillName = @"我超勇的好不好";
       AllSkillEntity4.SkillInfo = "自身防禦力提升1.5倍，持續5秒";
       AllSkillEntity4.CoolingTime = 18f;
       AllSkillEntity4.AttackDis = 0f;
       AllSkillEntity4.Damage = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity4.ID))
        {
          entityDic.Add(AllSkillEntity4.ID, AllSkillEntity4);
        }

       AllSkillEntity AllSkillEntity5 = new AllSkillEntity();
       AllSkillEntity5.ID = 100202;
       AllSkillEntity5.SkillName = @"龜波氣斬";
       AllSkillEntity5.SkillInfo = "向前方畫出一個弧形彎刀斬，對敵人造成110點的傷害";
       AllSkillEntity5.CoolingTime = 25f;
       AllSkillEntity5.AttackDis = 0f;
       AllSkillEntity5.Damage = 110f;

        if (!entityDic.ContainsKey(AllSkillEntity5.ID))
        {
          entityDic.Add(AllSkillEntity5.ID, AllSkillEntity5);
        }

       AllSkillEntity AllSkillEntity6 = new AllSkillEntity();
       AllSkillEntity6.ID = 100301;
       AllSkillEntity6.SkillName = @"死亡射線";
       AllSkillEntity6.SkillInfo = "向前方射出一個死亡射線，對敵人造成80點的傷害";
       AllSkillEntity6.CoolingTime = 12f;
       AllSkillEntity6.AttackDis = 0f;
       AllSkillEntity6.Damage = 80f;

        if (!entityDic.ContainsKey(AllSkillEntity6.ID))
        {
          entityDic.Add(AllSkillEntity6.ID, AllSkillEntity6);
        }

       AllSkillEntity AllSkillEntity7 = new AllSkillEntity();
       AllSkillEntity7.ID = 100302;
       AllSkillEntity7.SkillName = @"渾沌虛無";
       AllSkillEntity7.SkillInfo = "集氣，向前方放出一個渾沌，打到敵人造成150點的傷害";
       AllSkillEntity7.CoolingTime = 35f;
       AllSkillEntity7.AttackDis = 0f;
       AllSkillEntity7.Damage = 150f;

        if (!entityDic.ContainsKey(AllSkillEntity7.ID))
        {
          entityDic.Add(AllSkillEntity7.ID, AllSkillEntity7);
        }

       AllSkillEntity AllSkillEntity8 = new AllSkillEntity();
       AllSkillEntity8.ID = 100401;
       AllSkillEntity8.SkillName = @"踹踹看";
       AllSkillEntity8.SkillInfo = "跳起來在空中做飛踢，對敵人造成90點的傷害";
       AllSkillEntity8.CoolingTime = 10f;
       AllSkillEntity8.AttackDis = 0f;
       AllSkillEntity8.Damage = 90f;

        if (!entityDic.ContainsKey(AllSkillEntity8.ID))
        {
          entityDic.Add(AllSkillEntity8.ID, AllSkillEntity8);
        }

       AllSkillEntity AllSkillEntity9 = new AllSkillEntity();
       AllSkillEntity9.ID = 100402;
       AllSkillEntity9.SkillName = @"天崩地裂";
       AllSkillEntity9.SkillInfo = "奮力跳起，雙拳向地上砸，對敵人造成125點的傷害，並且會在敵人下方產生一個障礙物，障礙物在十秒後消失";
       AllSkillEntity9.CoolingTime = 30f;
       AllSkillEntity9.AttackDis = 0f;
       AllSkillEntity9.Damage = 125f;

        if (!entityDic.ContainsKey(AllSkillEntity9.ID))
        {
          entityDic.Add(AllSkillEntity9.ID, AllSkillEntity9);
        }

       AllSkillEntity AllSkillEntity10 = new AllSkillEntity();
       AllSkillEntity10.ID = 100501;
       AllSkillEntity10.SkillName = @"反擊的盾";
       AllSkillEntity10.SkillInfo = "擺出防禦的架式，前方產生一個盾，如果有敵人的技能打到這個盾，會把技能給抵消，並把原本傷害的1.5倍還給敵人";
       AllSkillEntity10.CoolingTime = 12f;
       AllSkillEntity10.AttackDis = 0f;
       AllSkillEntity10.Damage = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity10.ID))
        {
          entityDic.Add(AllSkillEntity10.ID, AllSkillEntity10);
        }

       AllSkillEntity AllSkillEntity11 = new AllSkillEntity();
       AllSkillEntity11.ID = 100502;
       AllSkillEntity11.SkillName = @"你死定了!";
       AllSkillEntity11.SkillInfo = "擺出你死定了的架式，自身攻擊力和跑速提升1.5倍，持續8秒";
       AllSkillEntity11.CoolingTime = 20f;
       AllSkillEntity11.AttackDis = 0f;
       AllSkillEntity11.Damage = 0f;

        if (!entityDic.ContainsKey(AllSkillEntity11.ID))
        {
          entityDic.Add(AllSkillEntity11.ID, AllSkillEntity11);
        }

    }

    
    static Dictionary<int, AllSkillEntity> entityDic = new Dictionary<int, AllSkillEntity>();
    public static AllSkillEntity Get(int key)
    {
        if (entityDic.ContainsKey(key))
        {
            return entityDic[key];
        }
        return null;
    }

    
   
    public static AllSkillEntity GetInstance(int key)
    {
        AllSkillEntity instance = new AllSkillEntity();
        if (entityDic.ContainsKey(key))
        {
            
            instance.ID = entityDic[key].ID;
            instance.SkillName = entityDic[key].SkillName;
            instance.SkillInfo = entityDic[key].SkillInfo;
            instance.CoolingTime = entityDic[key].CoolingTime;
            instance.AttackDis = entityDic[key].AttackDis;
            instance.Damage = entityDic[key].Damage;

        }
        return instance;
    }
}


public class AllSkillEntity
{
    //TemplateMember
    public int ID;//技能ID
    public string SkillName;//名稱
    public string  SkillInfo;//介紹
    public float CoolingTime;//冷卻時間
    public float AttackDis;//攻擊距離
    public float Damage;//傷害

}
