using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDead : EntityFSM
{
    public PlayerDead(Transform transform,PlayerFSM fsm)
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
        fsm.playerCtrl.isDead = true;
        if (fsm.playerCtrl.isSelf == true)
        {
            BattleWindow battleWindow = (BattleWindow)WindowManager.Instance.GetWindow(WindowType.BattleWindow);
            battleWindow.DeadHandle();
        }
        GameObject.Find("BattleAudioManager").GetComponent<BattleAudioManager>().Dead();
        fsm.playerCtrl.transform.GetComponent<BoxCollider>().enabled = false;
        fsm.playerCtrl.animatorManager.Play(PlayerAnimationClip.die);
        //進入到復活狀態
        Quit();
    }

    private async void Quit()
    {
        await Task.Delay(2000);
        //復活狀態
        fsm.playerCtrl.gameObject.SetActive(false);
        fsm.playerCtrl.hud.SetActive(false);
        fsm.ToNext(FSMState.Relive);
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

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void HandleCMD(byte[] data)
    {
        base.HandleCMD(data);
    }

    public override void HandleHudCMD(byte[] data)
    {
        base.HandleHudCMD(data);
    }
}
