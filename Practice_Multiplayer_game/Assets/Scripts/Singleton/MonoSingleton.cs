using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonoSingleton<T> : MonoBehaviour where T:MonoBehaviour
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (MonoSingletonObject.go == null)
            {
                MonoSingletonObject.go = new GameObject("MonoSingletonObject");
                DontDestroyOnLoad(MonoSingletonObject.go);
            }
            if (MonoSingletonObject.go != null && instance == null)
            {
                instance = MonoSingletonObject.go.AddComponent<T>();
            }
            return instance;
        }
    }
    //有時候有的組件場景切換的時候要回收
    public static bool destroyOnLoad = false;

    /// <summary>
    /// 場景切換的事件
    /// </summary>
    public void AddSceneChangeEvent()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene arg0, Scene arg1)
    {
        if (destroyOnLoad == true)
        {
            if (instance != null)
            {
                DestroyImmediate(instance); //立即銷毀
                Debug.Log(instance == null);
            }
        }
    }
}

//緩存一個遊戲物件
public class MonoSingletonObject
{
    public static GameObject go;
}
