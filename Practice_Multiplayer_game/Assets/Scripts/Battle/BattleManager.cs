using Assets.MCVlibrary.Model;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    InputCtrl inputCtrl;
    public Dictionary<int, PlayerCtrl> playerCtrlDic = new Dictionary<int, PlayerCtrl>();
    bool isInit = false;
    private void Awake()
    {
        inputCtrl = this.gameObject.AddComponent<InputCtrl>();
        //取得緩存的數據 房間裡面的所有角色
        foreach (var playerInfo in RoomModel.Instance.playerInfos)
        {
            if (playerInfo.RolesInfo == PlayerModel.Instance.rolesInfo)
            {
                SendGameStart(playerInfo.PosID);
            }
            //創建角色(模型)
            GameObject hero = ResManager.Instance.LoadHero(playerInfo.HeroID);
            //設置它的位置
            switch (playerInfo.PosID)
            {
                case 0:
                    hero.transform.position = new Vector3(7, 13, 2);
                    hero.transform.eulerAngles = new Vector3(0, 90, 0);
                    break;
                case 1:
                    hero.transform.position = new Vector3(12, 13, 2);
                    hero.transform.eulerAngles = new Vector3(0, 90, 0);
                    break;
                case 5:
                    hero.transform.position = new Vector3(15, 13, 2);
                    hero.transform.eulerAngles = new Vector3(0, -90, 0);
                    break;
                case 6:
                    hero.transform.position = new Vector3(19, 13, 2);
                    hero.transform.eulerAngles = new Vector3(0, -90, 0);
                    break;
                default:
                    break;
            }
            //添加控制器
            PlayerCtrl playerCtrl = hero.GetComponent<PlayerCtrl>();
            //緩存 角色物體
            playerCtrlDic[playerInfo.RolesInfo.RolesID] = playerCtrl;
            RoomModel.Instance.playerObjects[playerInfo.RolesInfo.RolesID] = hero;
            //每個角色自己要做初始化 每個控制器
            playerCtrl.Init(playerInfo);
        }
        
        //加載戰鬥介面
        WindowManager.Instance.OpenWindow(WindowType.BattleWindow);
        //輸入管理器的初始化
        inputCtrl.Init(playerCtrlDic[PlayerModel.Instance.rolesInfo.RolesID]);

        isInit = true;
    }
    //發送網路事件的玩家的FSM 處理他的事件
    public void HandleCMD(BattleUserInputS2C s2cMSG)
    {
        //先確認 這條操作命令是哪個玩家發的
        //然後調用他的角色控制器 FSM狀態機 去處理這個事件
        playerCtrlDic[s2cMSG.CMD.RolesID].playerFSM.HandleCMD(s2cMSG);
    }
    public void HandleMoveCMD(byte[] s2cMSG)
    {
        int rolesID = BitConverter.ToInt32(s2cMSG, 8);
        //先確認 這條操作命令是哪個玩家發的
        //然後調用他的角色控制器 FSM狀態機 去處理這個事件
        playerCtrlDic[rolesID].playerFSM.HandleCMD(s2cMSG);

    }
    public void HandleHudCMD(byte[] s2cMSG)
    {
        int demage = BitConverter.ToInt32(s2cMSG, 0);
        if (demage != 0)
        {
            int rolesID = BitConverter.ToInt32(s2cMSG, 12);
            //先確認 這條操作命令是哪個玩家發的
            //然後調用他的角色控制器 FSM狀態機 去處理這個事件
            playerCtrlDic[rolesID].playerFSM.HandleHudCMD(s2cMSG);
        }       
    }
    public void HandlePropsCMD(byte[] s2cMSG)
    {
        int x = BitConverter.ToInt32(s2cMSG, 0);
        int propsID = BitConverter.ToInt32(s2cMSG, 4);
        ResManager.Instance.LoadProps(propsID, x);
    }
    void Update()
    {
        //輸出的控制 是從緩存的禎數據
        if (isInit == true)
        {
            BattleListener.Instance.PlayerFrame(HandleCMD);
            BattleListener.Instance.PlayerMove(HandleMoveCMD);
            BattleListener.Instance.PlayerHud(HandleHudCMD);
            BattleListener.Instance.RandomProps(HandlePropsCMD);
        }
    }
    public void SendGameStart(int posId)
    {
        byte[] RoomID = BitConverter.GetBytes(PlayerModel.Instance.roomInfo.ID);
        byte[] RolesID = BitConverter.GetBytes(PlayerModel.Instance.rolesInfo.RolesID);
        byte[] PosID = BitConverter.GetBytes(posId);
        byte[] data = new byte[12];
        Array.Copy(RoomID, 0, data, 0, 4);
        Array.Copy(RolesID, 0, data, 4, 4);
        Array.Copy(PosID, 0, data, 8, 4);
        BufferFactory.CreateAndSendPackage(4, data);
    }
    private void OnDestroy()
    {
        RoomModel.Instance.Clear();
    }
}
