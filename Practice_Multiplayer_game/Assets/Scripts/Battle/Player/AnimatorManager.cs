using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 動畫狀態
/// </summary>
public enum PlayerAnimationClip
{
    None,
    Idle,
    walk,
    die,
    attack,
    skill1,
    skill2,
    gethit,
    victory,
    jump,
}
public class AnimatorManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;
    PlayerInfo playerInfo;
    Animator animator;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="playerCtrl"></param>
    public void Init(PlayerCtrl playerCtrl)
    {
        this.playerCtrl = playerCtrl;
        this.playerInfo = playerCtrl.PlayerInfo;
        animator = transform.GetComponent<Animator>();
    }
    /// <summary>
    /// 播放動畫
    /// </summary>
    /// <param name="clip"></param>
    public void Play(PlayerAnimationClip clip)
    {
        ResetState();
        animator.SetBool(clip.ToString(), true);
    }
    public string[] clip = new string[] { "None", "Idle","walk", "die", "attack", "skill1", "skill2", "gethit", "victory", "jump"};
    /// <summary>
    /// 重置動畫狀態
    /// </summary>
    public void ResetState()
    {
        for (int i = 0; i < clip.Length; i++)
        {
            animator.SetBool(clip[i], false);
        }
    }
    /// <summary>
    /// 技能音效
    /// </summary>
    /// <param name="Key">技能按鍵</param>
    void EffectAudio(string Key)
    {
        int keycode = 0;
        if (Key == "J")
        {
            keycode = 0;
        }
        else if (Key == "K")
        {
            keycode = 1;
        }
        else if (Key == "L")
        {
            keycode = 2;
        }
        
        BattleAudioManager audioManager = GameObject.Find("BattleAudioManager").GetComponent<BattleAudioManager>();
        switch (playerInfo.HeroID)
        {
            case 1001:
                audioManager.audioSource.PlayOneShot(audioManager.HeroClips[1001][keycode]);
                break;
            case 1002:
                audioManager.audioSource.PlayOneShot(audioManager.HeroClips[1002][keycode]);
                break;
            case 1003:
                audioManager.audioSource.PlayOneShot(audioManager.HeroClips[1003][keycode]);
                break;
            case 1004:
                audioManager.audioSource.PlayOneShot(audioManager.HeroClips[1004][keycode]);
                break;
            case 1005:
                audioManager.audioSource.PlayOneShot(audioManager.HeroClips[1005][keycode]);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// K技能事件
    /// </summary>
    public void DoSkillKEvent()
    {
        SpawnEffect("K");
        EffectAudio("K");
    }

    /// <summary>
    /// L技能事件
    /// </summary>
    public void DoSkillLEvent()
    {
        SpawnEffect("L");
        EffectAudio("L");
    }

    /// <summary>
    /// J普攻事件
    /// </summary>
    public void DoSkillJEvent()
    {
        SpawnEffect("J");
        EffectAudio("J");
    }

    EConfig eConfig;
    bool isSkill100201 = false;
    bool isSkill100502 = false;
    Animator effectAnim;

    /// <summary>
    /// 生成特效
    /// </summary>
    /// <param name="key"></param>
    public void SpawnEffect(string key)
    {
        GameObject effect = ResManager.Instance.LoadEffect(playerInfo.HeroID, key);
        eConfig = effect.transform.GetComponent<EConfig>();

        Vector3 position;
        if (this.transform.eulerAngles.y > 90)
        {
            position = transform.position + new Vector3(-0.75f, 0.5f, 0);
        }
        else
        {
            position = transform.position + new Vector3(0.75f, 0.5f, 0);
        }

        switch (eConfig.effectID)
        {
            case 100101:
                effect.transform.position = position;
                break;
            case 100102:
                effect.transform.position = position + new Vector3(0, 0.5f, 0);
                effect.transform.eulerAngles = transform.eulerAngles;
                break;
            case 100200:
                if (this.transform.eulerAngles.y > 90)
                {
                    effect.transform.eulerAngles = new Vector3(0, 0, 90);
                    effect.transform.position = position;
                }
                else
                {
                    effect.transform.position = position;
                }
                break;
            case 100201:
                effect.transform.position = position + new Vector3(0, 0.3f, -2);
                playerCtrl.currentAttribute.Armor = playerCtrl.currentAttribute.Armor * 1.5f;
                Invoke("Skill100201", 5);
                isSkill100201 = true;
                break;
            case 100202:
                if (this.transform.eulerAngles.y > 90)
                {
                    effect.transform.position = transform.position + new Vector3(-2f, 0.3f, 0);
                    ParticleSystem particle = eConfig.GetComponent<ParticleSystem>();
                    var main = particle.main;
                    main.startRotationZ = -1.3f; //   x * 180度 / 3.14 = 角度
                }
                else
                {
                    effect.transform.position = transform.position + new Vector3(2f, 0.3f, 0);
                }
                effect.transform.eulerAngles = transform.eulerAngles;
                break;
            case 100301:
                effectAnim = effect.GetComponent<Animator>();
                if (this.transform.eulerAngles.y > 90)
                {
                    effect.transform.position = transform.position + new Vector3(-0.75f, 0.5f, 0);
                    effectAnim.Play("左Q");
                }
                else
                {
                    effect.transform.position = transform.position + new Vector3(0.75f, 0.5f, 0);
                    effectAnim.Play("右Q");
                }
                break;
            case 100302:
                effect.transform.position = position + new Vector3(0, 0.25f, 0);
                break;
            case 100400:
                if (this.transform.eulerAngles.y > 90)
                {
                    effect.transform.position = transform.position + new Vector3(-1.2f, 0.5f, 0);
                }
                else
                {
                    effect.transform.position = transform.position + new Vector3(1.2f, 0.5f, 0);
                }
                break;
            case 100401:
                if (this.transform.eulerAngles.y > 90)
                {
                    effect.transform.position = transform.position + new Vector3(-1.5f, 1f, 0);
                }
                else
                {
                    effect.transform.position = transform.position + new Vector3(1.5f, 1f, 0);
                }
                break;
            case 100402:
                EConfig smoke = effect.transform.Find("smoke").GetComponent<EConfig>();
                BattleUserInputC2S skillCMD1 = playerCtrl.playerFSM.SkillCMD.CMD;
                smoke.Init(skillCMD1.RolesID, playerCtrl, skillCMD1.LockTag, skillCMD1.LockID,
                transform.forward, transform.position, playerCtrl.OnSkillTrigger);
                if (this.transform.eulerAngles.y > 270)
                {                  
                    effect.transform.position = transform.position + new Vector3(-5f, -0.5f, 0);
                }
                else if (this.transform.eulerAngles.y > 90)
                {
                    effect.transform.position = transform.position + new Vector3(3f, -0.5f, 0);
                }               
                break;
            case 100500:
                if (this.transform.eulerAngles.y > 90)
                {
                    effect.transform.position = transform.position + new Vector3(-1f, 0.5f, 0);
                    ParticleSystem particle = eConfig.GetComponent<ParticleSystem>();
                    var main = particle.main;
                    var shape = particle.shape;
                    shape.rotation = new Vector3(180, 90, 0);
                    main.startRotationZ = 1.57f; //   x * 180度 / 3.14 = 角度
                    effect.transform.GetComponent<BoxCollider>().center = new Vector3(-1, effect.transform.GetComponent<BoxCollider>().center.y, effect.transform.GetComponent<BoxCollider>().center.z);
                }
                else
                {
                    effect.transform.position = transform.position + new Vector3(1f, 0.5f, 0);
                }
                break;
            case 100501:
                if (this.transform.eulerAngles.y > 90)
                {
                    effect.transform.position = transform.position + new Vector3(-1f, 0.75f, 0);
                    effect.transform.eulerAngles = transform.eulerAngles + new Vector3(0, 10, 0);
                }
                else
                {
                    effect.transform.position = transform.position + new Vector3(1f, 0.75f, 0);
                    effect.transform.eulerAngles = transform.eulerAngles + new Vector3(0, -10, 0);
                }
                break;
            case 100502:
                playerCtrl.currentAttribute.Power = playerCtrl.currentAttribute.Power * 1.5f;
                playerCtrl.currentAttribute.MoveSpeed = playerCtrl.currentAttribute.MoveSpeed * 1.5f;
                Invoke("Skill100502", 8);
                isSkill100502 = true;
                effect.transform.position = transform.position + new Vector3(0, 0.001f, 0);
                break;
            default:
                effect.transform.position = position;
                effect.transform.eulerAngles = transform.eulerAngles;
                break;
        }

        effect.gameObject.SetActive(true);
        BattleUserInputC2S skillCMD = playerCtrl.playerFSM.SkillCMD.CMD;
        eConfig.Init(skillCMD.RolesID, playerCtrl, skillCMD.LockTag, skillCMD.LockID,
        transform.forward, transform.position, playerCtrl.OnSkillTrigger);
    }

    /// <summary>
    /// 技能結束的事件
    /// </summary>
    public void EndSkill()
    {
        Debug.Log("技能釋放結束");
        playerCtrl.playerFSM.ToNext(FSMState.Idle);
    }
    /// <summary>
    /// 受傷結束的事件
    /// </summary>
    public void EndGetHit()
    {
        Debug.Log("受傷結束");
        playerCtrl.playerFSM.ToNext(FSMState.Idle);
    }
    void Skill100201()
    {
        if (isSkill100201 == true)
        {
            playerCtrl.currentAttribute.Armor = playerCtrl.currentAttribute.Armor / 1.5f;
            isSkill100201 = false;
        }
    }
    void Skill100502()
    {
        if (isSkill100502 == true)
        {
            playerCtrl.currentAttribute.Power = playerCtrl.currentAttribute.Power / 1.5f;
            playerCtrl.currentAttribute.MoveSpeed = playerCtrl.currentAttribute.MoveSpeed / 1.5f;
            isSkill100502 = false;
        }
    }
}
