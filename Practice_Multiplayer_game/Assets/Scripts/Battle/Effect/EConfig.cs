using Assets.MCVlibrary.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//挂载在每个特效物体上,为什么呢?
public class EConfig : MonoBehaviour
{
    #region 由特效人员进行配置
   
    public float moveSpeed;//移动的速度
    public MoveType moveType;//移动的类型
    public float destroyTime;//销毁的时间
    public GameObject hitLoad;//碰撞时候触发的特效
    public GameObject spawnLoad;//出生时的特效,如枪口

    public Transform moveRoot;//运动的根节点
    public int effectID;//特效ID=英雄ID+技能按键名称

    public DestroyMode destroyMode;//销毁模式
    //作用类型 敌方或者友方 ..

    #endregion

    #region 由程序动态配置
    [HideInInspector]
    public int releaserID;//释放者ID
    [HideInInspector]
    public PlayerCtrl releaser;//释放者控制器
    [HideInInspector]
    public string lockTag;//锁定的目标的标签
    [HideInInspector]
    public int lockID;//锁定的目标ID

    [HideInInspector]
    public Vector3 skillDirection;//方向
    [HideInInspector]
    public Vector3 spawnPositon;//出生的旋转
    [HideInInspector]
    public Transform trackObject;

    public Action<EConfig,GameObject> hitAction;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="releaserID">釋放技能的用戶ID</param>
    /// <param name="lockTag">鎖定目標的標籤</param>
    /// <param name="lockID">鎖定目標的ID</param>
    /// <param name="skillDirection">技能方向</param>
    /// <param name="spawnPositon">出生位置</param>
    /// <param name="hitAction">碰撞到的回調</param>
    public  void Init(int releaserID,PlayerCtrl releaser, string lockTag, int lockID, Vector3 skillDirection, Vector3 spawnPositon,Action<EConfig,GameObject> hitAction)
    {
        if (hitAction != null)
        {
            this.hitAction = hitAction;
        }

        this.releaserID = releaserID;
        this.releaser = releaser;
        this.lockTag = lockTag;
        this.lockID = lockID;
        this.skillDirection = skillDirection;
        this.spawnPositon = spawnPositon;
        if (moveType==MoveType.TrackMove)
        {
            if (lockTag=="Player")
            {
                //獲取到跟蹤的物體 緩存起來 移動的時候可以使用
                trackObject = RoomModel.Instance.playerObjects[lockID].transform;
            }
        }

        if (destroyTime != 0)
        {
            //訂時間進行銷毀
            Destroy(this.gameObject, destroyTime);
        }
        //移動的根物體
        if (moveRoot != null)
        {
            //碰撞和移動是保持在同個節點下的

            //加上特效移動的腳本
            moveRoot.gameObject.AddComponent<EMove>().Init(this,this.transform);
            //加上碰撞控制的腳本 把碰撞的處理回調 傳遞進去
            moveRoot.gameObject.AddComponent<EHit>().Init(this);
        }

      
    }

    public void Awake()
    {
      
    }
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        if (effectID == 100501)
        {
            if (other.GetComponent<EConfig>())
            {
                EConfig otherEConfig = other.GetComponent<EConfig>();
                if (PlayerModel.Instance.rolesInfo.RolesID == otherEConfig.releaserID)
                {
                    otherEConfig.releaser.inputCtrl.SendHudInputC2S((int)(otherEConfig.releaser.skillManager.demageConfig[otherEConfig.gameObject.tag] * 1.5f / otherEConfig.releaser.currentAttribute.Armor), otherEConfig.releaserID);
                }               
                Destroy(other.gameObject);
            }
        }
    }
}


public enum MoveType
{
    DirectMove, //根據方向移動
    TrackMove, //跟蹤目標移動
}

/// <summary>
/// 销毁模式
/// </summary>
public enum DestroyMode
{
    OnHit_SameCampPlayer, //同陣營
    OnHit_DifferentCampPlayer, //不同陣營
    OnHit_AllPlayer, //所有玩家
    OnDestroyTimeEnd,//当销毁时间结束 执行销毁 也就是不让触碰的时候销毁 因为有些技能是持续性的 

    //可以配置更多 比如有的技能是可以攻击防御塔的 有的是不可以的
}