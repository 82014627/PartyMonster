using Assets.MCVlibrary.Model;
using Game.Model;
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

    bool isMove = false;
    bool isactive = true;
    bool first = false;

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
    }
    private void FixedUpdate()
    {
        input_x = Input.GetAxis("Horizontal");
        if (input_x == 0 && isactive == true)
        {
            isMove = false;
            isactive = false;
        }

        if (isMove == false && isactive == false)
        {
            input_x = 0;
            if (first == true)
            {
                SendMoveInputC2S(input_x, playerCtrl.playerFSM.transform.position.x, playerCtrl.playerFSM.transform.position.y, playerCtrl.playerFSM.transform.position.z);
                first = false;
            }           
            isMove = true;
        }
        if (isMove == true)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                input_x = -0.3f;
                SendMoveInputC2S(input_x, playerCtrl.playerFSM.transform.position.x, playerCtrl.playerFSM.transform.position.y, playerCtrl.playerFSM.transform.position.z);
                isactive = true;
                first = true;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                input_x = 0.3f;
                SendMoveInputC2S(input_x, playerCtrl.playerFSM.transform.position.x, playerCtrl.playerFSM.transform.position.y, playerCtrl.playerFSM.transform.position.z);
                isactive = true;
                first = true;
            }
        }
        if (playerCtrl.isJump == true && playerCtrl.playerFSM.currentState != FSMState.Skill && playerCtrl.playerFSM.currentState != FSMState.GetHit)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendInputCMD(KeyCode.Space);
                playerCtrl.isJump = false;
            }
        }
    }
    void SendInputCMD(KeyCode key)
    {
        BattleUserInputC2S c2sMSG = new BattleUserInputC2S();
        c2sMSG.RolesID = PlayerModel.Instance.rolesInfo.RolesID;
        c2sMSG.RoomID = PlayerModel.Instance.roomInfo.ID;
        c2sMSG.Key = key.GetHashCode();

        //Ray ray;
        //RaycastHit hit;

        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray, out hit))
        //{
        //    c2sMSG.MousePosition = hit.point.ToV3Info();
        //    if (LockTransform != null)
        //    {
        //        c2sMSG.LockTag = LockTransform.tag;
        //    }
        //}
        BufferFactory.CreateAndSendPackage(1500, c2sMSG);
    }
    void SendMoveInputC2S(float x,float position_x, float position_y, float position_z) //移動輸入
    {
        byte[] input_X = BitConverter.GetBytes(x);
        byte[] Position_x = BitConverter.GetBytes(position_x);
        byte[] Position_y = BitConverter.GetBytes(position_y);
        byte[] Position_z = BitConverter.GetBytes(position_z);
        byte[] RoomID = BitConverter.GetBytes(PlayerModel.Instance.roomInfo.ID);
        byte[] RolesID = BitConverter.GetBytes(PlayerModel.Instance.rolesInfo.RolesID);
        byte[] data = new byte[24];
        Array.Copy(input_X, 0, data, 0, 4);
        Array.Copy(Position_x, 0, data, 4, 4);
        Array.Copy(Position_y, 0, data, 8, 4);
        Array.Copy(Position_z, 0, data, 12, 4);
        Array.Copy(RoomID, 0, data, 16, 4);
        Array.Copy(RolesID, 0, data, 20, 4);

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
    Transform LockTransform;
    public void MouseDownEvent(KeyCode inputType)
    {
        Ray ray;
        RaycastHit hit;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Ground"))
            {

            }
            else if (hit.transform.CompareTag("Player"))
            {
                this.LockTransform = hit.transform;//點擊到人物
            }
        }
    }

}
