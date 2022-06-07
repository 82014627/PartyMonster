using ProtoMsg;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EHit : MonoBehaviour
{
    //碰撞:角色

    //所有的特效都采用物理触发机制 而非碰撞 避免使用物理效果带来的bug
    public void OnTriggerEnter(Collider other)
    {
        if (eConfig==null)
        {
            return;
        }
        //判断 如果不是同个阵营的 就产生爆炸 设置爆炸物体的生层位置 等于碰撞点的位置
        //然后通知属性管理器控制掉血

        //不同的ID 触发的效果可能不一样!
        if (eConfig.hitAction != null)
        {
            eConfig.hitAction(eConfig,other.gameObject);
        }
        
    }

    //bool isDestroy = false;
    EConfig eConfig;
    internal void Init(EConfig eConfig)
    {
        this.eConfig = eConfig;
    }
}
