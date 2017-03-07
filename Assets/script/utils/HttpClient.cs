using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.IO;

public class HttpClient {

	static public string sendPost(string url,Dictionary<string, object> paramsMap)
    {
        HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
        httpRequest.Method = "POST";
        httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)";
        httpRequest.Accept = "*/*";
        httpRequest.ContentType = "application/json;charset=UTF-8";
        httpRequest.Timeout = 1000 * 30;
        httpRequest.KeepAlive = true;
        // 通过流写入请求数据
        // 传入json化的参数
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(HttpClient.dictoryToJson(paramsMap));
        httpRequest.ContentLength = bytes.Length;
        Stream requestStream = httpRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        // 获得应答报文
        HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        Stream responseStream = httpResponse.GetResponseStream();
        StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
        string strResponse = reader.ReadToEnd();
        reader.Close();
        responseStream.Close();
        return strResponse;
    }

    static public string sendGet(string url,Dictionary<string, object> paramsMap)
    {
        string urlParams = "";
        if (paramsMap.Count != 0)
        {
            urlParams = "?";
            foreach (KeyValuePair<string, object> kvp in paramsMap)
            {
                urlParams += kvp.Key + "=" + kvp.Value + "&";
            }
            urlParams = urlParams.Substring(0, urlParams.Length - 1);
        }
        HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url + urlParams);
        httpRequest.Method = "GET";
        httpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)";
        httpRequest.Accept = "*/*";
        httpRequest.ContentType = "application/json;charset=UTF-8";
        httpRequest.Timeout = 1000 * 30;
        httpRequest.KeepAlive = true;
        // 获得应答报文
        HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        Stream responseStream = httpResponse.GetResponseStream();
        StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
        string strResponse = reader.ReadToEnd();
        reader.Close();
        responseStream.Close();
        return strResponse;
    }

    static public string dictoryToJson(Dictionary<string,object> dict)
    {
        if (dict.Count == 0)
        {
            return "{}";
        }
        string json = "{";
        foreach(KeyValuePair<string, object> kvp in dict)
        {
            json += "\"" + kvp.Key + "\":";
            if (kvp.Value is string)
            {
                json += "\"" + kvp.Value + "\",";
            }else
            {
                json += kvp.Value.ToString() + ",";
            }
        }
        json = json.Substring(0, json.Length - 1);
        json += "}";
        return json;
    }
}
