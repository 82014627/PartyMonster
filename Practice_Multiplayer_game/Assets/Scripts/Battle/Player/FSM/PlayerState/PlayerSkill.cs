using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : EntityFSM
{
    public PlayerSkill(Transform transform ,PlayerFSM fsm)
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
        if (fsm.SkillCMD.CMD.Key == KeyCode.Q.GetHashCode())
        {
            fsm.playerCtrl.animatorManager.Play(PlayerAnimationClip.skill1);
        }
        else if (fsm.SkillCMD.CMD.Key == KeyCode.E.GetHashCode())
        {
            fsm.playerCtrl.animatorManager.Play(PlayerAnimationClip.skill2);
        }
        else if (fsm.SkillCMD.CMD.Key == KeyCode.Mouse0.GetHashCode())
        {
            fsm.playerCtrl.animatorManager.Play(PlayerAnimationClip.attack);
        }
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
        Debug.Log("請等待本次技能施放完畢");
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
        int damage = BitConverter.ToInt32(data, 0);
        int hitPlayerRolesID = BitConverter.ToInt32(data, 4);
        if (damage != 0)
        {
            fsm.playerCtrl.currentAttribute.HP -= damage;
            fsm.playerCtrl.OnSkillHit(damage);
            if (fsm.playerCtrl.currentAttribute.HP <= 0)
            {
                fsm.playerCtrl.currentAttribute.HP = 0;
                fsm.playerCtrl.HudUpdate();
                //进入到死亡状态
                fsm.playerCtrl.playerFSM.ToNext(FSMState.Dead);
                if (fsm.playerCtrl.PlayerInfo.TeamID != 0)
                {
                    BattleWindow battleWindow = (BattleWindow)GameObject.Find("MonoSingletonObject").GetComponent<WindowManager>().GetWindow(WindowType.BattleWindow);
                    battleWindow.A_score += 1;
                }
                else
                {
                    BattleWindow battleWindow = (BattleWindow)GameObject.Find("MonoSingletonObject").GetComponent<WindowManager>().GetWindow(WindowType.BattleWindow);
                    battleWindow.B_score += 1;
                }
                BattleCtrl.Instance.SaveKills(hitPlayerRolesID, 1);
            }
            else
            {
                fsm.playerCtrl.HudUpdate();
            }
            BattleCtrl.Instance.SaveDamage(hitPlayerRolesID, damage);
        }
    }
}
