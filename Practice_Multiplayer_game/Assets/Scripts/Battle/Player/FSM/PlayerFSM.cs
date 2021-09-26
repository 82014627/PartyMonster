using ProtoMsg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 角色有限状态机的管理类
/// </summary>
public class PlayerFSM : MonoBehaviour
{
    Dictionary<FSMState, EntityFSM> playerState = new Dictionary<FSMState, EntityFSM>();
    public FSMState currentState = FSMState.None;
    public PlayerCtrl playerCtrl;
    public Rigidbody rigidbody;

    public void Init(PlayerCtrl playerCtrl) {
        this.playerCtrl = playerCtrl;
        rigidbody = GetComponent<Rigidbody>();
        playerState[FSMState.Idle] = new PlayerIdle(transform, this);
        playerState[FSMState.Move] = new PlayerMove(transform, this);
        playerState[FSMState.Dead] = new PlayerDead(transform, this);
        playerState[FSMState.Skill] = new PlayerSkill(transform, this);
        playerState[FSMState.Relive] = new PlayerRelive(transform, this);
        playerState[FSMState.Jump] = new PlayerJump(transform, this);
        playerState[FSMState.GetHit] = new PlayerGetHit(transform, this);
        ToNext(FSMState.Idle);
    }

    private void Update()
    {       
        if (currentState != FSMState.None)
        {
            playerState[currentState].Update();
        }
    }
    private void FixedUpdate()
    {
        if (currentState != FSMState.None)
        {
            playerState[currentState].FixedUpdate();
        }
    }
    /// <summary>
    /// 切換到下一個狀態
    /// </summary>
    /// <param name="nextState"></param>
    public void ToNext(FSMState nextState) {
        if (currentState!=FSMState.None)
        {
            playerState[currentState].Exit();
        }
        playerState[nextState].Enter();
        currentState = nextState;
    }
    public byte[] moveCMD;
    public BattleUserInputS2C SkillCMD;
    //處理每一禎的網路事件
    public void HandleCMD(BattleUserInputS2C s2cMSG)
    {
        playerState[currentState].HandleCMD(s2cMSG);
    }
    public void HandleCMD(byte[] message)
    {
        playerState[currentState].HandleCMD(message);
    }
    public void HandleHudCMD(byte[] message)
    {
        playerState[currentState].HandleHudCMD(message);
    }
}
