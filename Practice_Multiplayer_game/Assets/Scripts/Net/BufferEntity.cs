using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Game.Net
{
    public class BufferEntity
    {
        public int recurCount; //重發次數
        public IPEndPoint endPoint; //發送的目標終端
        public int protoSize; //報文大小
        public int session; //會話ID
        public int sn; //序號
        public int moduleID; //模塊ID
        public long time; //發送時間
        public int messageType; //協議類型
        public int messageID; //協議ID

        public byte[] proto; //業務報文

        public byte[] buffer; //報文數據
        /// <summary>
        /// 構造函數請求報文
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="session"></param>
        /// <param name="sn"></param>
        /// <param name="moduleID"></param>
        /// <param name="messageType"></param>
        /// <param name="messageID"></param>
        /// <param name="proto"></param>
        public BufferEntity(IPEndPoint endPoint,int session,int sn,int moduleID,int messageType,int messageID,byte[] proto)
        {
            this.endPoint = endPoint;
            this.session = session;
            this.sn = sn;
            this.moduleID = moduleID;
            this.messageType = messageType;
            this.messageID = messageID;
            this.proto = proto;
            this.protoSize = proto.Length;
        }
        /// <summary>
        /// 構造函數接收報文實體
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="buffer"></param>
        public BufferEntity(IPEndPoint endPoint,byte[] buffer)
        {
            this.endPoint = endPoint;
            this.buffer = buffer;
            Decode();
        }
        /// <summary>
        /// 構造函數一個ACK報文實體
        /// </summary>
        /// <param name="package"></param>
        public BufferEntity(BufferEntity package)
        {
            protoSize = 0;
            this.endPoint = package.endPoint;
            this.session = package.session;
            this.sn = package.sn;
            this.moduleID = package.moduleID;
            this.time = 0;
            this.messageType = 0;
            this.messageID = package.messageID;

            buffer = Encoder(true);

        }
        /// <summary>
        /// 將報文序列化
        /// </summary>
        /// <param name="isACK"></param>
        /// <returns></returns>
        public byte[] Encoder(bool isACK)
        {
            byte[] data = new byte[32 + protoSize];
            if (isACK)
            {
                protoSize = 0;
            }
            byte[] proto_length = BitConverter.GetBytes(protoSize);
            byte[] _session = BitConverter.GetBytes(session);
            byte[] _sn = BitConverter.GetBytes(sn);
            byte[] _moduleID = BitConverter.GetBytes(moduleID);
            byte[] _time = BitConverter.GetBytes(time);
            byte[] _messageType = BitConverter.GetBytes(messageType);
            byte[] _messageID = BitConverter.GetBytes(messageID);

            Array.Copy(proto_length, 0, data, 0, 4);
            Array.Copy(_session, 0, data, 4, 4);
            Array.Copy(_sn, 0, data, 8, 4);
            Array.Copy(_moduleID, 0, data, 12, 4);
            Array.Copy(_time, 0, data, 16, 8);
            Array.Copy(_messageType, 0, data, 24, 4);
            Array.Copy(_messageID, 0, data, 28, 4);

            if (isACK)
            {
                
            }
            else
            {
                //業務報文
                Array.Copy(proto, 0, data, 32, proto.Length);
            }
            buffer = data;
            return data;
        }
        /// <summary>
        /// 將報文反序列化
        /// </summary>
        public bool isFull = false;
        public void Decode()
        {
            if (buffer.Length >= 4)
            {
                //字節數組 轉化成int 或者是long
                protoSize = BitConverter.ToInt32(buffer, 0);
                if (buffer.Length == protoSize + 32)
                {
                    isFull = true;
                }
                else
                {
                    isFull = false;
                    return;
                }
            }
            session = BitConverter.ToInt32(buffer, 4);
            sn = BitConverter.ToInt32(buffer, 8);
            moduleID = BitConverter.ToInt32(buffer, 12);
            time = BitConverter.ToInt64(buffer, 16);
            messageType = BitConverter.ToInt32(buffer, 24);
            messageID = BitConverter.ToInt32(buffer, 28);

            if (messageType == 0)
            {

            }
            else
            {
                proto = new byte[protoSize];
                //將buffer裡剩下的數據 複製到proto 得到最終的業務數據
                Array.Copy(buffer, 32, proto, 0, protoSize);
            }
        }
    }
}

