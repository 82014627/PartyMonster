using Assets.MCVlibrary.Model;
using Game.Net;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InputCtrl : MonoBehaviour
{
    float input_x;

    public bool isMove = false;
    public bool isactive = true;
    public bool first = false;

    public bool isJ = true;
    public bool isK = true;
    public bool isL = true;
    //只需要監聽用戶的按鍵 然後發送網路消息就可以了
    PlayerCtrl playerCtrl;
    public void Init(PlayerCtrl playerCtrl)
    {
        this.playerCtrl = playerCtrl;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && isK == true && playerCtrl.playerFSM.currentState != FSMState.Skill && playerCtrl.playerFSM.currentState != FSMState.GetHit)
        {
            SendInputCMD(KeyCode.K);
            playerCtrl.skillManager.DoCooling(KeyCode.K);
            isK = false;
        }
        else if (Input.GetKeyDown(KeyCode.L) && isL == true && playerCtrl.playerFSM.currentState != FSMState.Skill && playerCtrl.playerFSM.currentState != FSMState.GetHit)
        {
            SendInputCMD(KeyCode.L);
            playerCtrl.skillManager.DoCooling(KeyCode.L);
            isL = false;
        }
        else if (Input.GetKeyDown(KeyCode.J) && isJ == true && playerCtrl.playerFSM.currentState != FSMState.Skill && playerCtrl.playerFSM.currentState != FSMState.GetHit)
        {
            SendInputCMD(KeyCode.J);
            playerCtrl.skillManager.DoCooling(KeyCode.J);
            isJ = false;
        }
        if (playerCtrl.isCanJump == true && playerCtrl.playerFSM.currentState != FSMState.Skill && playerCtrl.playerFSM.currentState != FSMState.GetHit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendInputCMD(KeyCode.Space);
                playerCtrl.isCanJump = false;
            }
        }
        input_x = Input.GetAxis("Horizontal");
        if (input_x == 0 && isactive == true)
        {
            isMove = false;
            isactive = false;
            if (first == true)
            {
                SendMoveInputC2S(0);
                first = false;
            }
            isMove = true;
        }
        if (isMove == true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                input_x = -0.3f;
                SendMoveInputC2S(-0.3f);
                isactive = true;
                first = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                input_x = 0.3f;
                SendMoveInputC2S(0.3f);
                isactive = true;
                first = true;
            }
        }
    }
    void SendInputCMD(KeyCode key)
    {
        BattleUserInputC2S c2sMSG = new BattleUserInputC2S();
        c2sMSG.RolesID = PlayerModel.Instance.rolesInfo.RolesID;
        c2sMSG.RoomID = PlayerModel.Instance.roomInfo.ID;
        c2sMSG.Key = key.GetHashCode();
        BufferFactory.CreateAndSendPackage(1500, c2sMSG);
    }
    void SendMoveInputC2S(float x) //移動輸入
    {
        byte[] input_X = BitConverter.GetBytes(x);
        byte[] RoomID = BitConverter.GetBytes(PlayerModel.Instance.roomInfo.ID);
        byte[] RolesID = BitConverter.GetBytes(PlayerModel.Instance.rolesInfo.RolesID);
        byte[] data = new byte[12];
        Array.Copy(input_X, 0, data, 0, 4);
        Array.Copy(RoomID, 0, data, 4, 4);
        Array.Copy(RolesID, 0, data, 8, 4);

        BufferFactory.CreateAndSendPackage(1, data);
    }
    public void SendHudInputC2S(int demage, int hitRolesID)
    {
        byte[] Demage = BitConverter.GetBytes(demage);
        byte[] HitRolesID = BitConverter.GetBytes(hitRolesID);
        byte[] RoomID = BitConverter.GetBytes(PlayerModel.Instance.roomInfo.ID);
        byte[] RolesID = BitConverter.GetBytes(PlayerModel.Instance.rolesInfo.RolesID);
        byte[] data = new byte[16];
        Array.Copy(Demage, 0, data, 0, 4);
        Array.Copy(HitRolesID, 0, data, 4, 4);
        Array.Copy(RoomID, 0, data, 8, 4);
        Array.Copy(RolesID, 0, data, 12, 4);

        BufferFactory.CreateAndSendPackage(2, data);
    }
    public void SendBattleOverC2S(int level, int coin)
    {
        RolesCreateS2C c2sMSG = new RolesCreateS2C();
        c2sMSG.RolesInfo = PlayerModel.Instance.rolesInfo;
        c2sMSG.RolesInfo.Level = level;
        c2sMSG.RolesInfo.GoldCoin = coin;
        c2sMSG.RolesInfo.RoomID = PlayerModel.Instance.roomInfo.ID;
        BufferFactory.CreateAndSendPackage(3, c2sMSG);
    }
}
