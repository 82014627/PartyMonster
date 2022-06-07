using Assets.MCVlibrary.Model;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolesCtrl : Singleton<RolesCtrl>
{
    /// <summary>
    /// 保存角色數據
    /// </summary>
    /// <param name="rolesInfo"></param>
    public void SaveRolesInfo(RolesInfo rolesInfo)
    {
        PlayerModel.Instance.rolesInfo = rolesInfo;
    }
    /// <summary>
    /// 獲取角色數據
    /// </summary>
    /// <returns></returns>
    public RolesInfo GetRolesInfo()
    {
        return PlayerModel.Instance.rolesInfo;
    }
    /// <summary>
    /// 保存房間數據
    /// </summary>
    /// <param name="roomInfo"></param>
    public void SaveRoomInfo(RoomInfo roomInfo)
    {
        PlayerModel.Instance.roomInfo = roomInfo;
    }
    /// <summary>
    /// 獲取房間數據
    /// </summary>
    /// <returns></returns>
    public RoomInfo GetRoomInfo()
    {
        return PlayerModel.Instance.roomInfo;
    }
}
