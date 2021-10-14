using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Game.Net
{
    public class USocket
    {
        UdpClient udpClient;
        public string ip = "10.120.2.185";
        int port = 8899;
        public static UClient local;
        public static IPEndPoint server;
        /// <summary>
        /// 構造函數創建一個socket
        /// </summary>
        /// <param name="dispatchNetEvent"></param>
        public USocket(Action<BufferEntity> dispatchNetEvent)
        {
            udpClient = new UdpClient(0);
            server = new IPEndPoint(IPAddress.Parse(ip), port);
            local = new UClient(this, server, 0, 0, 0, dispatchNetEvent);
            ReceiveTask();//接收消息
        }
        /// <summary>
        /// 接收報文
        /// </summary>
        ConcurrentQueue<UdpReceiveResult> awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
        public async void ReceiveTask()
        {
            while (udpClient != null)
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync(); //可以用来等待，因為是異步的接口               
                    awaitHandle.Enqueue(result); //當有客户端發送消息的时候，要缓存起来，再来進行處理
                    Debug.Log("接收到了消息!");
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
        /// <summary>
        /// 發送報文
        /// </summary>
        /// <param name="data">要發送的資料</param>
        /// <param name="endPoint">伺服器位址</param>
        public async void Send(byte[] data , IPEndPoint endPoint)
        {
            if (udpClient != null)
            {
                try
                {
                    int length = await udpClient.SendAsync(data,data.Length,endPoint);
                }
                catch (Exception e)
                {

                    Debug.LogError($"發送異常:{e.Message}");
                }
            }
        }
        /// <summary>
        /// 發送ACK報文
        /// </summary>
        /// <param name="bufferentity">要發送的報文</param>
        public void SendACK(BufferEntity bufferentity)
        {
            Send(bufferentity.buffer, server);
        }
        /// <summary>
        /// 處理報文
        /// </summary>
        public void Handle()
        {
            if (awaitHandle.Count > 0)
            {
                UdpReceiveResult data;
                if (awaitHandle.TryDequeue(out data))
                {
                    BufferEntity bufferEntity = new BufferEntity(data.RemoteEndPoint, data.Buffer);
                    if (bufferEntity.isFull)
                    {
                        Debug.Log($"處理消息,id:{bufferEntity.messageID},序號:{bufferEntity.sn}");
                        //處理業務邏輯 交給客戶端代理去處理
                        local.Handle(bufferEntity);
                    }
                }
            }
        }
        /// <summary>
        /// 關閉客戶端代理跟UdpClient
        /// </summary>
        public void Close()
        {
            if (local != null)
            {
                local = null;
            }
            if (udpClient != null)
            {
                udpClient = null;
            }
        }
    }
}

