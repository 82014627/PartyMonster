using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : EntityFSM
{
    public PlayerJump(Transform transform, PlayerFSM fsm)
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
        fsm.playerCtrl.animatorManager.Play(PlayerAnimationClip.jump);
        fsm.rigidbody.AddForce(transform.up * 1800);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void HandleCMD(BattleUserInputS2C s2cMSG)
    {
        base.HandleCMD(s2cMSG);
        if (s2cMSG.CMD.Key == KeyCode.Q.GetHashCode())
        {
            HandleSkillEvent(s2cMSG);
        }
        else if (s2cMSG.CMD.Key == KeyCode.E.GetHashCode())
        {
            HandleSkillEvent(s2cMSG);
        }
        else if (s2cMSG.CMD.Key == KeyCode.Mouse0.GetHashCode())
        {
            //普通攻擊
            HandleSkillEvent(s2cMSG);
        }
        if (s2cMSG.CMD.Key == KeyCode.Space.GetHashCode())
        {
            fsm.ToNext(FSMState.Jump);
        }
    }

    public override void HandleCMD(byte[] data)
    {
        base.HandleCMD(data);
    }

    public override void HandleHudCMD(byte[] data)
    {
        base.HandleHudCMD(data);
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
                fsm.ToNext(FSMState.GetHit);
                fsm.playerCtrl.HudUpdate();
            }
            BattleCtrl.Instance.SaveDamage(hitPlayerRolesID, damage);
        }
    }

    public override void HandleMoveEvent(byte[] data)
    {
        base.HandleMoveEvent(data);
        //float input_X = BitConverter.ToSingle(data, 0);
        //float beforex = BitConverter.ToSingle(data, 4);
        //float beforey = BitConverter.ToSingle(data, 8);
        //float beforez = BitConverter.ToSingle(data, 12);
        //if (input_X == 0)
        //{
        //    transform.position = new Vector3(beforex, beforey, beforez);
        //    return;
        //}
        //transform.position = new Vector3(beforex, beforey, beforez);
        fsm.ToNext(FSMState.Move);
    }

    public override void HandleSkillEvent(BattleUserInputS2C s2cMSG)
    {
        base.HandleSkillEvent(s2cMSG);
        if (fsm.playerCtrl.PlayerInfo.HeroID == 1004)
        {
            if (s2cMSG.CMD.Key == KeyCode.E.GetHashCode())
            {
                return;
            }
        }
        fsm.ToNext(FSMState.Skill);
    }

    public override void RemoveListener()
    {
        base.RemoveListener();
    }

    public override void Update()
    {
        base.Update();
    }
}
