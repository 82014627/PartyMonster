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
    /// <summary>
    /// 出生的位置
    /// </summary>
    public Vector3[] spawnPosition = new Vector3[10] 
    { 
        //A隊伍的位置 0-4
        new Vector3(25.26f,2.1f,-19.9f),
        new Vector3(25.26f,2.1f,-17.14f),
        new Vector3(-6.71f,0,-4.01f ),
        new Vector3(-4.28f,0,-5.89f ),
        new Vector3(-2.02f,0,-8.23f), 
        //B隊伍的位置 5-9
        new Vector3(30,2.1f,30),
        new Vector3(30,2.1f,27.9f),
        new Vector3(-95.432f,0,-101.49f),
        new Vector3(-97.692f,0,-99.409f),
        new Vector3(-99.7443f,0,-96.884f), 
    };

    /// <summary>
    /// 初始的角度
    /// </summary>
    public Vector3[] spawnRotation = new Vector3[10] 
    {
        //A隊伍的角度
        new Vector3(0,90,0),
        new Vector3(0,90,0),
        new Vector3( 0,-152.659f,0 ),
        new Vector3(0,230.56f,0),
        new Vector3( 0,-149.089f,0 ),

        //B隊伍的角度
        new Vector3(0,-90,0 ),
        new Vector3(0,-90,0 ),
        new Vector3( 0,-327.746f,0),
        new Vector3(0,55.473f,0),
        new Vector3(0,-324.176f,0), 
    };

    public Dictionary<int, PlayerCtrl> playerCtrlDic = new Dictionary<int, PlayerCtrl>();
    bool isInit = false;
    private void Awake()
    {
        inputCtrl = this.gameObject.AddComponent<InputCtrl>();
        //取得緩存的數據 房間裡面的所有角色
        foreach (var playerInfo in RoomModel.Instance.playerInfos)
        {
            //創建角色(模型)
            GameObject hero = ResManager.Instance.LoadHero(playerInfo.HeroID);
            //設置它的位置
            switch (playerInfo.PosID)
            {
                case 0:
                    hero.transform.position = new Vector3(7, 1, 2);
                    hero.transform.eulerAngles = new Vector3(0, 90, 0);
                    break;
                case 1:
                    hero.transform.position = new Vector3(11, 1, 2);
                    hero.transform.eulerAngles = new Vector3(0, 90, 0);
                    break;
                case 5:
                    hero.transform.position = new Vector3(15, 1, 2);
                    hero.transform.eulerAngles = new Vector3(0, -90, 0);
                    break;
                case 6:
                    hero.transform.position = new Vector3(19, 1, 2);
                    hero.transform.eulerAngles = new Vector3(0, -90, 0);
                    break;
                default:
                    break;
            }
            //hero.transform.eulerAngles = spawnRotation[playerInfo.PosID];
            //添加控制器
            PlayerCtrl playerCtrl = hero.AddComponent<PlayerCtrl>();
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
        int rolesID = BitConverter.ToInt32(s2cMSG, 20);
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

    private void OnDestroy()
    {
        RoomModel.Instance.Clear();
    }
}
