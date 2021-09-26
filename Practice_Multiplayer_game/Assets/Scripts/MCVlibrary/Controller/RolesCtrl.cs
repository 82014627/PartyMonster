using Assets.MCVlibrary.Model;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolesCtrl : Singleton<RolesCtrl>
{
    public void SaveRolesInfo(RolesInfo rolesInfo)
    {
        PlayerModel.Instance.rolesInfo = rolesInfo;
    }
    public RolesInfo GetRolesInfo()
    {
        return PlayerModel.Instance.rolesInfo;
    }

    internal void SaveRoomInfo(RoomInfo roomInfo)
    {
        PlayerModel.Instance.roomInfo = roomInfo;
    }
    public RoomInfo GetRoomInfo()
    {
        return PlayerModel.Instance.roomInfo;
    }
}
