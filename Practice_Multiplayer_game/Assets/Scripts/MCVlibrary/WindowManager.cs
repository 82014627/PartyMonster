using Game.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoSingleton<WindowManager>
{
    Dictionary<WindowType, BaseWindow> windowDic = new Dictionary<WindowType, BaseWindow>();
    //構造函數 初始化
    public WindowManager()
    {
        //商店
        windowDic.Add(WindowType.StoreWindow, new StoreWindow());
        windowDic.Add(WindowType.LoginWindow, new LoginWindow());
        windowDic.Add(WindowType.TipsWindow, new TipsWindow());
        windowDic.Add(WindowType.RolesWindow, new RolesWindow());
        windowDic.Add(WindowType.LobbyWindow, new LobbyWindow());
        windowDic.Add(WindowType.RoomWindow, new RoomWindow());
        windowDic.Add(WindowType.BattleWindow, new BattleWindow());
    }
    public void Update()
    {
        foreach (var window in windowDic.Values)
        {
            if (window.GetVisable())
            {
                window.Update(Time.deltaTime);
            }
        }
    }
    //打開窗口
    public BaseWindow OpenWindow(WindowType type)
    {
        BaseWindow window;
        if (windowDic.TryGetValue(type, out window))
        {
            window.Open();
            return window;
        }
        else
        {
            Debug.LogError($"Open Error:{type}!");
            return null;
        }
    }
    //關閉窗口
    public void CloseWindow(WindowType type)
    {
        BaseWindow window;
        if (windowDic.TryGetValue(type, out window))
        {
            window.Close();
        }
        else
        {
            Debug.LogError($"Open Error:{type}!");
        }
    }
    //預加載
    public void PreLoadWindow(ScenesType type)
    {
        foreach (var item in windowDic.Values)
        {
            if (item.GetScenesType() == type)
            {
                item.PreLoad();
            }
        }
    }

    //隱藏調某個類型的所有窗口
    public void HideAllWindow(ScenesType type,bool isDestroy = false)
    {
        foreach (var item in windowDic.Values)
        {
            if (item.GetScenesType() == type)
            {
                item.Close(isDestroy);
            }
        }
    }
    public void ShowTips(string text, Action EnterOKBtn = null)
    {
        TipsWindow tipsWindow = (TipsWindow)Instance.OpenWindow(WindowType.TipsWindow);
        tipsWindow.ShowTips(text, EnterOKBtn);
    }

    public BaseWindow GetWindow(WindowType type)
    {
        BaseWindow window;
        if (windowDic.TryGetValue(type, out window))
        {
            return window;
        }
        else
        {
            return null;
        }
    }
}
