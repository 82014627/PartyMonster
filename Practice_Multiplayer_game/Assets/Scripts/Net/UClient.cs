using Game.Net;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Net
{
    public class UClient 
    {
        USocket uSocket;
        public IPEndPoint server;
        public int sessionID;
        public int sendSN;
        public int handleSN;
        public Action<BufferEntity> handleAction;
        int overtime = 150;
        /// <summary>
        /// 構造函數創建一個UClient
        /// </summary>
        /// <param name="uSocket"></param>
        /// <param name="server"></param>
        /// <param name="sessionID"></param>
        /// <param name="sendSN"></param>
        /// <param name="handleSN"></param>
        /// <param name="dispatchNetEvent"></param>
        public UClient(USocket uSocket, IPEndPoint server, int sessionID, int sendSN, int handleSN, Action<BufferEntity> dispatchNetEvent)
        {
            this.uSocket = uSocket;
            this.server = server;
            this.sessionID = sessionID;
            this.sendSN = sendSN;
            this.handleSN = handleSN;
            this.handleAction = dispatchNetEvent;
            CheckOutTime();
        }
        ConcurrentDictionary<int, BufferEntity> sendPackage = new ConcurrentDictionary<int, BufferEntity>();
        public void Send(BufferEntity package)
        {
            package.time = TimeHelper.Now();
            sendSN += 1;
            package.sn = sendSN;

            package.Encoder(false);

            if (sessionID != 0)
            {
                sendPackage.TryAdd(sendSN, package);//緩存起來 因為可能需要重發
            }
            else
            {
                //還沒跟服務器建立連接的 所以不需要進行緩存
            }
            uSocket.Send(package.buffer,server);
        }
        /// <summary>
        /// 處理接收到的消息
        /// </summary>
        public void Handle(BufferEntity bufferEntity)
        {
            if (this.sessionID == 0 && bufferEntity.session != 0)
            {
                Debug.Log($"服務器發送給我們的會話ID是{bufferEntity.session}");
                this.sessionID = bufferEntity.session;
            }
            switch (bufferEntity.messageType)
            {
                case 0://ACK報文
                    BufferEntity buffer;
                    if (sendPackage.TryRemove(bufferEntity.sn, out buffer))
                    {
                        Debug.Log($"收到ACK確認報文，序號是:{buffer.sn}");
                    }
                    break;
                case 1://業務報文
                    BufferEntity ackPackage = new BufferEntity(bufferEntity);
                    uSocket.SendACK(ackPackage); 

                    HandleLogincPackage(bufferEntity);//處理業務報文
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 處理業務報文的邏輯
        /// </summary>
        /// <param name="bufferEntity"></param>
        ConcurrentDictionary<int, BufferEntity> waitHandle = new ConcurrentDictionary<int, BufferEntity>();
        public void HandleLogincPackage(BufferEntity bufferEntity)
        {
            if (bufferEntity.sn <= handleSN)
            {
                return;
            }
            if (bufferEntity.sn - handleSN > 1)//收到的報文是錯序的
            {
                if (waitHandle.TryAdd(bufferEntity.sn, bufferEntity))
                {
                    Debug.Log($"收到錯序的報文:{bufferEntity.sn}");
                }
                return;
            }
            handleSN = bufferEntity.sn;//更新handleSN的順序
            if (handleAction != null)
            {
                handleAction(bufferEntity);//派發 分發給遊戲模塊去處理
            }
            BufferEntity nextBuffer;
            if (waitHandle.TryRemove(handleSN + 1, out nextBuffer))//這裡是判斷緩存區有沒有存在下一條數據 
            {                
                HandleLogincPackage(nextBuffer);
            }
        }
        /// <summary>
        /// 超時重發檢測
        /// </summary>
        public async void CheckOutTime()
        {
            await Task.Delay(overtime);
            foreach (var package in sendPackage.Values)
            {
                if (package.recurCount >= 10) //判斷重發次數有無超過10
                {
                    Debug.Log($"重發次數超過10次，關閉socket");
                    OnDisconnect();
                    return;
                }
                if (TimeHelper.Now() - package.time > (package.recurCount + 1) * overtime)
                {
                    package.recurCount += 1;
                    Debug.Log($"超時重發，次數:{package.recurCount}");
                    uSocket.Send(package.buffer, server);
                }
            }
            CheckOutTime();
        }
        /// <summary>
        /// 斷開與伺服器的連接
        /// </summary>
        public void OnDisconnect()
        {
            handleAction = null;
            uSocket.Close();
        }
    }
}

