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
        //打開登入介面
        WindowManager.Instance.OpenWindow(WindowType.LoginWindow);
    }
    void Update()
    {
        if (uSocket != null)
        {
            uSocket.Handle();
        }
    }
    private void DispatchNetEvent(BufferEntity buffer)
    {
        //進行報文分發事件
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
