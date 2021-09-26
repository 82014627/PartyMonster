using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsManager : Singleton<PropsManager>
{
    public void Props102(PlayerCtrl playerCtrl)
    {
        playerCtrl.currentAttribute.Power = playerCtrl.currentAttribute.Power / 1.5f;
    }
    public void Props103(PlayerCtrl playerCtrl)
    {
        playerCtrl.currentAttribute.MoveSpeed = playerCtrl.currentAttribute.MoveSpeed / 1.5f;
    }
    public void Props104(PlayerCtrl playerCtrl)
    {
        playerCtrl.isInvincible = false;
    }
}
