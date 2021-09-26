using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleListener : Singleton<BattleListener>
{
    Queue<BattleUserInputS2C> awaitHandle;
    Queue<byte[]> awaitMoveHandle;
    Queue<byte[]> awaitHudHandle;
    Queue<byte[]> awaitPropsHandle;
    //初始化的方法 監聽戰鬥的網路消息
    public void Init()
    {
        awaitHandle = new Queue<BattleUserInputS2C>();
        awaitMoveHandle = new Queue<byte[]>();
        awaitHudHandle = new Queue<byte[]>();
        awaitPropsHandle = new Queue<byte[]>();
        NetEvent.Instance.AddEventListener(1500, HandleBattleUserInputS2C);
        NetEvent.Instance.AddEventListener(1, HandleMoveInputS2C);
        NetEvent.Instance.AddEventListener(2, HandleHudS2C);
        NetEvent.Instance.AddEventListener(3, HandlePropsS2C);
    }
    //釋放的方法 移除監聽網路消息
    public void Release()
    {
        NetEvent.Instance.RemoveEventListener(1500, HandleBattleUserInputS2C);
        NetEvent.Instance.RemoveEventListener(1, HandleMoveInputS2C);
        NetEvent.Instance.RemoveEventListener(2, HandleHudS2C);
        NetEvent.Instance.RemoveEventListener(3, HandlePropsS2C);
        awaitHandle.Clear();
        awaitMoveHandle.Clear();
        awaitHudHandle.Clear();
        awaitPropsHandle.Clear();
    }

    //處理存儲網路事件的方法
    public void HandleBattleUserInputS2C(BufferEntity response)
    {
        BattleUserInputS2C s2cMSG = ProtobufHelper.FromBytes<BattleUserInputS2C>(response.proto);
        awaitHandle.Enqueue(s2cMSG);
    }
    byte[] MoveData;
    byte[] HudData;
    byte[] propsData;
    public void HandleMoveInputS2C(BufferEntity response)
    {
        MoveData = new byte[24];
        MoveData = response.proto;
        awaitMoveHandle.Enqueue(MoveData);
    }
    public void HandleHudS2C(BufferEntity response)
    {
        HudData = new byte[16];
        HudData = response.proto;
        awaitHudHandle.Enqueue(HudData);
    }
    public void HandlePropsS2C(BufferEntity response)
    {
        propsData = new byte[8];
        propsData = response.proto;
        awaitPropsHandle.Enqueue(propsData);
    }
    //調用網路事件的方法
    public void PlayerFrame(Action<BattleUserInputS2C> action)
    {
        if (action != null && awaitHandle.Count > 0)
        {
            action(awaitHandle.Dequeue());
        }
    }
    public void PlayerMove(Action<byte[]> action)
    {
        if (action != null)
        {
            if (MoveData != null)
            {
                if (awaitMoveHandle.Count > 0)
                {
                    action(awaitMoveHandle.Dequeue());
                }
                else
                {
                    action(MoveData);
                }
            }
        }
    }
    public void PlayerHud(Action<byte[]> action)
    {
        if (action != null && awaitHudHandle.Count > 0)
        {
            if (HudData != null)
            {
                action(awaitHudHandle.Dequeue());
            }
        }
    }
    public void RandomProps(Action<byte[]> action)
    {
        if (action != null && awaitPropsHandle.Count > 0)
        {
            if (propsData != null)
            {
                action(awaitPropsHandle.Dequeue());
            }
        }
    }
}
