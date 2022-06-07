using Google.Protobuf;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class JsonHelper
{
    /// <summary>  
    /// 將對象序列化為JSON格式  
    /// </summary>  
    /// <param name="o">對象</param>  
    /// <returns>json字符串</returns>  
    public static string SerializeObject(object o)
    {
        JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
        //LitJson本身不支持float類型的數據  這裡將它進行轉換成double
        JsonMapper.RegisterImporter<double, float>(input => Convert.ToSingle(input));
        string json = JsonMapper.ToJson(o);
        return json;
    }

    public static void Log(int messageID, IMessage message)
    {
        Debug.Log($"報文ID:{messageID}\n包體{JsonHelper.SerializeObject(message)}");
    }

}