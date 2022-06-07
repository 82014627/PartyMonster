using Game.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static USocket uSocket;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        uSocket = new USocket(DispatchNetEvent);
        Debug.Log($"網路初始化完成，IP位址:{uSocket.ip} Port:{uSocket.port}");
        //打開登入介面
        WindowManager.Instance.OpenWindow(WindowType.LoginWindow);
        AudioManager.Instance.Init();
    }
    void Update()
    {
        if (uSocket != null)
        {
            uSocket.Handle();
        }
    }
    /// <summary>
    /// 進行報文分發事件
    /// </summary>
    /// <param name="buffer"></param>
    private void DispatchNetEvent(BufferEntity buffer)
    {
        NetEvent.Instance.Dispatch(buffer.messageID, buffer);
    }

    /// <summary>
    /// 應用程式關閉 發送給伺服器 Socket關閉
    /// </summary>
    private void OnApplicationQuit()
    {
        if (USocket.local.sessionID != 0)
        {
            byte[] message = new byte[1];
            BufferFactory.CreateAndSendPackage(0, message);
            uSocket.Close();
            uSocket = null;
        }
    }
}
