using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerRelive : EntityFSM
{
    public PlayerRelive(Transform transform, PlayerFSM fsm)
    {
        this.transform = transform;
        this.fsm = fsm;
    }
    public override void AddListener()
    {
        base.AddListener();
    }

    public override void Enter()
    {
        base.Enter();
        Quit();
    }
    private async void Quit()
    {
        await Task.Delay(5000);
        //復活狀態
        fsm.playerCtrl.Relive();
        fsm.ToNext(FSMState.Idle);
    }
    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleCMD(BattleUserInputS2C s2cMSG)
    {
        base.HandleCMD(s2cMSG);
    }

    public override void RemoveListener()
    {
        base.RemoveListener();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void HandleMoveEvent(byte[] data)
    {
        base.HandleMoveEvent(data);
    }

    public override void HandleSkillEvent(BattleUserInputS2C s2cMSG)
    {
        base.HandleSkillEvent(s2cMSG);
    }
}
