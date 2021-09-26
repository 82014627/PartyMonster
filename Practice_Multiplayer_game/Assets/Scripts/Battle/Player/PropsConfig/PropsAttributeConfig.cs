
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PropsAttributeConfig
{

    static PropsAttributeConfig()
    {
        
       PropsAttributeEntity PropsAttributeEntity0 = new PropsAttributeEntity();
       PropsAttributeEntity0.PropsID = 101;
       PropsAttributeEntity0.PropsName = @"體力瓶";
       PropsAttributeEntity0.Props_Illustrate = @"回復玩家150點的血量";
       PropsAttributeEntity0.Effect_Value = 150;
       PropsAttributeEntity0.DelayDestroy = 10;
       PropsAttributeEntity0.Probability = 0.2f;

        if (!entityDic.ContainsKey(PropsAttributeEntity0.PropsID))
        {
          entityDic.Add(PropsAttributeEntity0.PropsID, PropsAttributeEntity0);
        }

       PropsAttributeEntity PropsAttributeEntity1 = new PropsAttributeEntity();
       PropsAttributeEntity1.PropsID = 102;
       PropsAttributeEntity1.PropsName = @"力量瓶";
       PropsAttributeEntity1.Props_Illustrate = @"提升玩家1.5倍的攻擊力持續8秒";
       PropsAttributeEntity1.Effect_Value = 8;
       PropsAttributeEntity1.DelayDestroy = 10;
       PropsAttributeEntity1.Probability = 0.2f;

        if (!entityDic.ContainsKey(PropsAttributeEntity1.PropsID))
        {
          entityDic.Add(PropsAttributeEntity1.PropsID, PropsAttributeEntity1);
        }

       PropsAttributeEntity PropsAttributeEntity2 = new PropsAttributeEntity();
       PropsAttributeEntity2.PropsID = 103;
       PropsAttributeEntity2.PropsName = @"速度瓶";
       PropsAttributeEntity2.Props_Illustrate = @"提升玩家1.5倍的跑速持續8秒";
       PropsAttributeEntity2.Effect_Value = 8;
       PropsAttributeEntity2.DelayDestroy = 10;
       PropsAttributeEntity2.Probability = 0.2f;

        if (!entityDic.ContainsKey(PropsAttributeEntity2.PropsID))
        {
          entityDic.Add(PropsAttributeEntity2.PropsID, PropsAttributeEntity2);
        }

       PropsAttributeEntity PropsAttributeEntity3 = new PropsAttributeEntity();
       PropsAttributeEntity3.PropsID = 104;
       PropsAttributeEntity3.PropsName = @"無敵星星";
       PropsAttributeEntity3.Props_Illustrate = @"玩家進入無敵狀態持續5秒";
       PropsAttributeEntity3.Effect_Value = 5;
       PropsAttributeEntity3.DelayDestroy = 8;
       PropsAttributeEntity3.Probability = 0.05f;

        if (!entityDic.ContainsKey(PropsAttributeEntity3.PropsID))
        {
          entityDic.Add(PropsAttributeEntity3.PropsID, PropsAttributeEntity3);
        }

       PropsAttributeEntity PropsAttributeEntity4 = new PropsAttributeEntity();
       PropsAttributeEntity4.PropsID = 105;
       PropsAttributeEntity4.PropsName = @"地雷";
       PropsAttributeEntity4.Props_Illustrate = @"踩到的玩家會損失100點的血量";
       PropsAttributeEntity4.Effect_Value = 100;
       PropsAttributeEntity4.DelayDestroy = 12;
       PropsAttributeEntity4.Probability = 0.35f;

        if (!entityDic.ContainsKey(PropsAttributeEntity4.PropsID))
        {
          entityDic.Add(PropsAttributeEntity4.PropsID, PropsAttributeEntity4);
        }

    }

    
    static Dictionary<int, PropsAttributeEntity> entityDic = new Dictionary<int, PropsAttributeEntity>();
    public static PropsAttributeEntity Get(int key)
    {
        if (entityDic.ContainsKey(key))
        {
            return entityDic[key];
        }
        return null;
    }

    
   
    public static PropsAttributeEntity GetInstance(int key)
    {
        PropsAttributeEntity instance = new PropsAttributeEntity();
        if (entityDic.ContainsKey(key))
        {
            
            instance.PropsID = entityDic[key].PropsID;
            instance.PropsName = entityDic[key].PropsName;
            instance.Props_Illustrate = entityDic[key].Props_Illustrate;
            instance.Effect_Value = entityDic[key].Effect_Value;
            instance.DelayDestroy = entityDic[key].DelayDestroy;
            instance.Probability = entityDic[key].Probability;

        }
        return instance;
    }
}


public class PropsAttributeEntity
{
    //TemplateMember
    public int PropsID;//道具PropsID
    public string PropsName;//道具名稱
    public string Props_Illustrate;//道具說明
    public int Effect_Value;//道具效果數值
    public int DelayDestroy;//幾秒後銷毀
    public float Probability;//機率

}
