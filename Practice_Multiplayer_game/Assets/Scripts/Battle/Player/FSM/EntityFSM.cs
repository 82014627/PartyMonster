using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFSM 
{
    public Transform transform;
    public PlayerFSM fsm; //管理類

    /// <summary>
    /// 進入狀態
    /// </summary>
    public virtual void Enter() 
    {
        AddListener();
    }

    /// <summary>
    /// 狀態更新
    /// </summary>
    public virtual void Update() 
    { 
    
    }
    /// <summary>
    /// 移動
    /// </summary>
    public virtual void FixedUpdate()
    {

    }
    /// <summary>
    /// 退出狀態
    /// </summary>
    public virtual void Exit() 
    {
        RemoveListener();
    }

    /// <summary>
    /// 增加監聽遊戲事件
    /// </summary>
    public virtual void AddListener() 
    { 
    
    }

    /// <summary>
    /// 移除監聽遊戲事件
    /// </summary>
    public virtual void RemoveListener()
    {

    }

    /// <summary>
    /// 處理網路事件
    /// </summary>
    /// <param name="s2cMSG"></param>
    public virtual void HandleCMD(BattleUserInputS2C s2cMSG)
    {

    }
    public virtual void HandleCMD(byte[] data)
    {
        HandleMoveEvent(data);
    }
    public virtual void HandleHudCMD(byte[] data)
    {
        
    }
    public virtual void HandleMoveEvent(byte[] data)
    {
        fsm.moveCMD = data;
    }
    public virtual void HandleSkillEvent(BattleUserInputS2C s2cMSG)
    {
        fsm.SkillCMD = s2cMSG;
    }
}
