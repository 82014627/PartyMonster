using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot 
{
    static Transform transform;
    static Transform recyclePool; //回收的窗體 隱藏
    static Transform workstation; //前台顯示/工作的窗體
    static Transform noticestation; //提示類型的窗體
    static bool isInit = false;
    public static void Init()
    {
        if (transform == null)
        {
            var obj = Resources.Load<GameObject>("UI/UIRoot");
            transform = GameObject.Instantiate(obj).transform;
            GameObject.DontDestroyOnLoad(transform);
        }

        if (recyclePool == null)
        {
            recyclePool = transform.Find("recyclePool");
        }

        if (workstation == null)
        {
            workstation = transform.Find("workstation");
        }

        if (noticestation == null)
        {
            noticestation = transform.Find("noticestation");
        }
        isInit = true;
    }
    public static void SetParent(Transform window, bool isOpen,bool isTipsWindow = false)
    {
        if (isInit == false)
        {
            Init();
        }

        if (isOpen == true)
        {
            if (isTipsWindow)
            {
                window.SetParent(noticestation, false);
            }
            else
            {
                window.SetParent(workstation, false);
            }
        }
        else
        {
            window.SetParent(recyclePool, false);
        }
    }
}
