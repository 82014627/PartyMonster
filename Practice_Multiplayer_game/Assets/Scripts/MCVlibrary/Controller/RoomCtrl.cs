using Assets.MCVlibrary.Model;
using Google.Protobuf.Collections;
using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCtrl : Singleton<RoomCtrl>
{
    /// <summary>
    /// 獲取陣營訊息
    /// </summary>
    /// <param name="rolesID"></param>
    /// <returns></returns>
    public int GetTeamID(int rolesID)
    {
        for (int i = 0; i < PlayerModel.Instance.roomInfo.TeamA.Count; i++)
        {
            if (PlayerModel.Instance.roomInfo.TeamA[i].RolesID == rolesID)
            {
                return 0;
            }
            if (PlayerModel.Instance.roomInfo.TeamB[i].RolesID == rolesID)
            {
                return 1;
            }
        }
        return -1;
    }
    /// <summary>
    /// 檢查英雄是否自己的ID
    /// </summary>
    /// <param name="rolesID"></param>
    /// <returns></returns>
    public bool CheckIsSelfRoles(int rolesID)
    {
        return PlayerModel.Instance.rolesInfo.RolesID == rolesID;
    }
    /// <summary>
    /// 房間解散的時候調用
    /// </summary>
    /// <param name="roomInfo"></param>
    public void RemoveRoomInfo()
    {
        PlayerModel.Instance.roomInfo = null;
    }
    /// <summary>
    /// 獲取角色暱稱
    /// </summary>
    /// <param name="rolesID"></param>
    /// <returns></returns>
    public string GetNickName(int rolesID)
    {
        for (int i = 0; i < PlayerModel.Instance.roomInfo.TeamA.Count; i++)
        {
            if (PlayerModel.Instance.roomInfo.TeamA[i].RolesID == rolesID)
            {
                return PlayerModel.Instance.roomInfo.TeamA[i].NickName;
            }
        }
        for (int i = 0; i < PlayerModel.Instance.roomInfo.TeamB.Count; i++)
        {
            if (PlayerModel.Instance.roomInfo.TeamB[i].RolesID == rolesID)
            {
                return PlayerModel.Instance.roomInfo.TeamB[i].NickName;
            }
        }
        return "";
    }
    /// <summary>
    /// 保存所有玩家角色信息
    /// </summary>
    /// <param name="playerInfos"></param>
    public void SavePlayerList(RepeatedField<PlayerInfo> playerInfos)
    {
        RoomModel.Instance.playerInfos = playerInfos;
    }
    public PlayerInfo GetSelfPlayerInfo()
    {
        return RoomModel.Instance.playerObjects[PlayerModel.Instance.rolesInfo.RolesID].
            GetComponent<PlayerCtrl>().PlayerInfo;
    }
    internal PlayerCtrl GetSelfPlayerCtrl()
    {
        return RoomModel.Instance.playerObjects[PlayerModel.Instance.rolesInfo.RolesID].
            GetComponent<PlayerCtrl>();
    }
}
