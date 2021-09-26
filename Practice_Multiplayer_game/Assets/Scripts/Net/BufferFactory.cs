using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Net
{
    public class BufferFactory 
    {
        enum MessageType
        {
            ACK = 0,//確認報文
            Login = 1,//業務報文
        }

        public static BufferEntity CreateAndSendPackage(int messageID,IMessage message)
        {
            //打印protobuf 按Json格式
            JsonHelper.Log(messageID, message);

            BufferEntity bufferEntity = new BufferEntity(USocket.local.server, USocket.local.sessionID, 0, 0, MessageType.Login.GetHashCode(), messageID, ProtobufHelper.ToBytes(message));
            USocket.local.Send(bufferEntity);
            return bufferEntity;
        }
        public static BufferEntity CreateAndSendPackage(int messageID, byte[] message)
        {
            BufferEntity bufferEntity = new BufferEntity(USocket.local.server, USocket.local.sessionID, 0, 0, MessageType.Login.GetHashCode(), messageID, message);
            USocket.local.Send(bufferEntity);
            return bufferEntity;
        }
    }
}

