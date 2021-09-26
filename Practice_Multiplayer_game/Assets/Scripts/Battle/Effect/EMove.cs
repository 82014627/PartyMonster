using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EMove : MonoBehaviour
{
    Transform effectPos;
    float posy;
    private void Start()
    {
        if (eConfig.effectID == 100402)
        {
            posy = effectPos.position.y;
        }   
    }
    void FixedUpdate()
    {
        if (eConfig!=null)
        {
            if (eConfig.moveSpeed == 0)
            {
                if (eConfig.effectID==100101)
                {
                    transform.position = eConfig.releaser.transform.position + new Vector3(0, 0.5f, 0);
                }
                else if (eConfig.effectID == 100201)
                {
                    transform.position = eConfig.releaser.transform.position + new Vector3(0, 0.8f, -2);
                }
                else if (eConfig.effectID == 100402)
                {          
                    if ((transform.position.y - posy) < 0.5)
                    {
                        StartCoroutine(Skill100402());               
                    }
                    else
                    {
                        StopCoroutine(Skill100402());
                    }
                }
                else if (eConfig.effectID == 100501)
                {
                    if (this.transform.eulerAngles.y > 90)
                    {
                        transform.position = eConfig.releaser.transform.position + new Vector3(-1f, 0.75f, 0);
                    }
                    else
                    {
                        transform.position = eConfig.releaser.transform.position + new Vector3(1f, 0.75f, 0);
                    }
                }
                else if (eConfig.effectID == 100502)
                {
                    transform.position = eConfig.releaser.transform.position + new Vector3(0, 0.001f, 0);
                }
                else
                {
                    return;
                }
            }
            if (eConfig.moveType == MoveType.DirectMove)
            {
                if (eConfig.effectID==100200 || eConfig.effectID == 100302)
                {
                    if (transform.position.x < eConfig.releaser.transform.position.x)
                    {
                        transform.position += (new Vector3(-transform.position.x, 0, 0)) * (eConfig.moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.position += (new Vector3(transform.position.x, 0, 0)) * (eConfig.moveSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    transform.position += (transform.forward) * (eConfig.moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (Vector3.Distance(eConfig.trackObject.position, transform.position) >= 0.01f)
                {
                    transform.transform.LookAt(eConfig.trackObject);
                    transform.position += (transform.forward) * (eConfig.moveSpeed * Time.deltaTime);
                }
            }
        }
        
       
    }

    EConfig eConfig;
    internal void Init(EConfig eConfig,Transform effectPos)
    {
        this.eConfig = eConfig;
        this.effectPos = effectPos;
    }
    IEnumerator Skill100402()
    {
        if (eConfig.effectID == 100402)
        {
            for (int i = 0; i < 20; i++)
            {
                transform.position += new Vector3(0, 0.005f, 0) * Time.deltaTime;
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
