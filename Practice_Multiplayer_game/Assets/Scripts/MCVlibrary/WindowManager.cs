using Game.View;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoSingleton<WindowManager>
{
    Dictionary<WindowType, BaseWindow> windowDic = new Dictionary<WindowType, BaseWindow>();
    public WindowManager()
    {
        windowDic.Add(WindowType.LoginWindow, new LoginWindow());
        windowDic.Add(WindowType.TipsWindow, new TipsWindow());
        windowDic.Add(WindowType.RolesWindow, new RolesWindow());
        windowDic.Add(WindowType.LobbyWindow, new LobbyWindow());
        windowDic.Add(WindowType.RoomWindow, new RoomWindow());
        windowDic.Add(WindowType.BattleWindow, new BattleWindow());
        windowDic.Add(WindowType.CharacterWindow, new CharacterWindow());
        windowDic.Add(WindowType.TeachWindow, new TeachWindow());
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
    /// <summary>
    /// 打開窗口
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
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
    /// <summary>
    /// 關閉窗口
    /// </summary>
    /// <param name="type"></param>
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
    /// <summary>
    /// 預加載
    /// </summary>
    /// <param name="type"></param>
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

    /// <summary>
    /// 隱藏掉某個類型的所有窗口
    /// </summary>
    /// <param name="type">類型</param>
    /// <param name="isDestroy">是否刪除</param>
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
    /// <summary>
    /// 打開提示窗體
    /// </summary>
    /// <param name="text">提示文字</param>
    /// <param name="EnterOKBtn">提示結束事件</param>
    public void ShowTips(string text, Action EnterOKBtn = null)
    {
        TipsWindow tipsWindow = (TipsWindow)Instance.OpenWindow(WindowType.TipsWindow);
        tipsWindow.ShowTips(text, EnterOKBtn);
    }
    /// <summary>
    /// 獲取當前窗體
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
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
