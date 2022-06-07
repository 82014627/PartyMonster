using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class EventBase<T,P,X> where T:new () where P:class
{
    private static T instance;
    public static T Instance {
        get {
            if (instance==null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    //儲存事件ID 還有方法(委託) 
    //使用線程安全的字典 避免以后多線程環境下出現問題
   public ConcurrentDictionary<X, List<Action<P>>> dic = new ConcurrentDictionary<X, List<Action<P>>>();

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="key">字典的KEY</param>
    /// <param name="handle">方法</param>
    public void AddEventListener(X key,Action<P> handle) {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(handle);
        }
        else
        {
            List<Action<P>> actions = new List<Action<P>>();
            actions.Add(handle);
            dic[key] = actions;
        }
    }


    /// <summary>
    /// 移除事件
    /// </summary>
    /// <param name="key">字典的KEY</param>
    /// <param name="handle">方法</param>
    public void RemoveEventListener(X key, Action<P> handle) {
        if (dic.ContainsKey(key))
        {
            List<Action<P>> actions = dic[key];
            actions.Remove(handle);

            if (actions.Count==0)
            {
                List<Action<P>> removeActions;
                dic.TryRemove(key,out removeActions);
            }
        }
    }

    /// <summary>
    /// 派發事件的接口-带有參數
    /// </summary>
    /// <param name="key"></param>
    /// <param name="p"></param>
    public void Dispatch(X key,P p) {
        if (dic.ContainsKey(key))
        {
            List<Action<P>> actions = dic[key];
            if (actions!=null && actions.Count > 0)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i]!=null)
                    {
                        actions[i](p);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 派發事件的接口-没有參數
    /// </summary>
    /// <param name="key"></param>
    public void Dispatch(X key) {
        Dispatch(key, null);
    }
}
