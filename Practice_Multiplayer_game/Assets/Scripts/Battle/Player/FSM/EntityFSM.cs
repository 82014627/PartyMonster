using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFSM 
{
    public Transform transform;
    public PlayerFSM fsm; //管理类

    //进入状态
    public virtual void Enter() {
        AddListener();
    }

    //状态更新中
    public virtual void Update() { 
    
    }
    //移動
    public virtual void FixedUpdate()
    {

    }
    //退出状态
    public virtual void Exit() {
        RemoveListener();
    }

    //监听一些游戏事件
    public virtual void AddListener() { 
    
    }

    //移除掉监听的事件
    public virtual void RemoveListener()
    {

    }

    //處理每一禎的網路事件
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
