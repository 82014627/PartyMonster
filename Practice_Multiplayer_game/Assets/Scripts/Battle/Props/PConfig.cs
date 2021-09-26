using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PConfig : MonoBehaviour
{
    public int PropsID;
    public int Effect_Value;
    public int DelayDestroy;
    public PlayerCtrl playerCtrl;
    private void Start()
    {
        Destroy(this.gameObject, DelayDestroy);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCtrl = other.transform.GetComponent<PlayerCtrl>();
            switch (PropsID)
            {
                case 101:
                    playerCtrl.currentAttribute.HP += Effect_Value;
                    if (playerCtrl.currentAttribute.HP > playerCtrl.totalAttribute.HP)
                    {
                        playerCtrl.currentAttribute.HP = playerCtrl.totalAttribute.HP;
                    }
                    playerCtrl.OnHeal(Effect_Value);
                    playerCtrl.HudUpdate();
                    Destroy(this.gameObject);
                    break;
                case 102:
                    playerCtrl.currentAttribute.Power = playerCtrl.currentAttribute.Power * 1.5f;
                    playerCtrl.PropsEffectOver(PropsID, DelayDestroy);
                    Destroy(this.gameObject);
                    break;
                case 103:
                    playerCtrl.currentAttribute.MoveSpeed = playerCtrl.currentAttribute.MoveSpeed * 1.5f;
                    playerCtrl.PropsEffectOver(PropsID, DelayDestroy);
                    Destroy(this.gameObject);
                    break;
                case 104:
                    playerCtrl.isInvincible = true;
                    playerCtrl.PropsEffectOver(PropsID, DelayDestroy);
                    Destroy(this.gameObject);
                    break;
                default:
                    break;
            }
        }
        if (other.gameObject.tag == "Ground")
        {
            if (PropsID == 105)
            {
                transform.GetComponent<BoxCollider>().isTrigger = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerCtrl = collision.transform.GetComponent<PlayerCtrl>();
            if (PropsID == 105)
            {
                if (playerCtrl.isSelf == true && playerCtrl.isInvincible == false)
                {
                    playerCtrl.inputCtrl.SendHudInputC2S(Effect_Value, 0);
                }
                Destroy(this.gameObject);
            }
        }   
    }
    private void Update()
    {
        if (!GetComponent<BoxCollider>())
        {
            if (playerCtrl != null)
            {
                Vector3 vector3;
                switch (PropsID)
                {
                    case 101:
                        vector3 = new Vector3(0, 0, 0);
                        break;
                    case 102:
                        vector3 = new Vector3(0, 0.01f, 0);
                        break;
                    case 103:
                        vector3 = new Vector3(0, 0.01f, 0);
                        break;
                    case 104:
                        vector3 = new Vector3(0, 0.5f, -1);
                        break;
                    case 105:
                        vector3 = new Vector3(0, 0.5f, -1);
                        break;
                    default:
                        vector3 = new Vector3(0, 0, 0);
                        break;
                }
                transform.position = playerCtrl.transform.position + vector3;
            }
        }
    }
}
