using Assets.MCVlibrary.Model;
using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : EntityFSM
{
    float input_X = 0;
    float beforex, beforey, beforez;
    float x, y, z;
    Vector3 forwardDirection;
    public PlayerMove(Transform transform, PlayerFSM fsm)
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
        isMove = true;
    }
    bool isMove = false;
    public override void Exit()
    {
        base.Exit();
        isMove = false;
    }

    public override void HandleCMD(BattleUserInputS2C s2cMSG)
    {
        base.HandleCMD(s2cMSG);
        if (s2cMSG.CMD.Key == KeyCode.K.GetHashCode())
        {
            HandleSkillEvent(s2cMSG);
        }
        else if (s2cMSG.CMD.Key == KeyCode.L.GetHashCode())
        {
            HandleSkillEvent(s2cMSG);
        }
        else if (s2cMSG.CMD.Key == KeyCode.J.GetHashCode())
        {
            //普通攻擊
            HandleSkillEvent(s2cMSG);
        }
        if (s2cMSG.CMD.Key == KeyCode.Space.GetHashCode())
        {
            fsm.ToNext(FSMState.Jump);
        }
    }

    public override void RemoveListener()
    {
        base.RemoveListener();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate(); 
    }
    public override void HandleMoveEvent(byte[] data)
    {
        base.HandleMoveEvent(data);
        input_X = BitConverter.ToSingle(data, 0);

        if (isMove == true)
        {
            Vector3 globalDirectionForward = transform.TransformDirection(Vector3.forward);
            if (input_X == 0)
            {
                fsm.rigidbody.MovePosition(this.transform.position);
                fsm.ToNext(FSMState.Idle);
                return;
            } 
            fsm.playerCtrl.animatorManager.Play(PlayerAnimationClip.walk);
            if (input_X == 0.3f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                forwardDirection = input_X * globalDirectionForward * Time.deltaTime;
            }
            else if (input_X == -0.3f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                forwardDirection = -input_X * globalDirectionForward * Time.deltaTime;
            }
            Vector3 maindis = forwardDirection;
            fsm.rigidbody.MovePosition(fsm.rigidbody.position + maindis * fsm.playerCtrl.currentAttribute.MoveSpeed);
        }
    }
    public override void HandleSkillEvent(BattleUserInputS2C s2cMSG)
    {
        base.HandleSkillEvent(s2cMSG);
        //進入技能狀態
        if (fsm.playerCtrl.isCanJump == false)
        {
            if (fsm.playerCtrl.PlayerInfo.HeroID == 1004)
            {
                if (s2cMSG.CMD.Key == KeyCode.E.GetHashCode())
                {
                    return;
                }
            }
        }
        fsm.ToNext(FSMState.Skill);
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
                fsm.ToNext(FSMState.GetHit);
                fsm.playerCtrl.HudUpdate();
            }
            BattleCtrl.Instance.SaveDamage(hitPlayerRolesID, damage);
        }
    }
}
