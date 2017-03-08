using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Assets.script.common;

public class Room : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	}

    public void joinRoom()
    {
        int roomIdx = int.Parse(this.gameObject.name.Substring(4, this.gameObject.name.Length - 4));
        if (UserInfo.joinedRoom == roomIdx)
        {
            // 再按一次表示退出房间
            Dictionary<string, object> paramsMap = new Dictionary<string, object>();
            paramsMap.Add("token", UserInfo.token);
            string response = HttpClient.sendGet(App.serverPath + "YgoService/hall/exit-room", paramsMap);
            ResponseResult responseResult = JsonUtility.FromJson<ResponseResult>(response);
            if (responseResult.code == 0)
            {
                UserInfo.joinedRoom = -1;
                Debug.Log("exit Room" + roomIdx.ToString());
            }
            else
            {
                Debug.Log(responseResult.data);
            }
        }
        else
        {
            Dictionary<string, object> paramsMap = new Dictionary<string, object>();
            paramsMap.Add("roomIdx", roomIdx);
            paramsMap.Add("token", UserInfo.token);
            string response = HttpClient.sendGet(App.serverPath + "YgoService/hall/join-room", paramsMap);
            ResponseResult responseResult = JsonUtility.FromJson<ResponseResult>(response);
            if (responseResult.code == 0)
            {
                UserInfo.joinedRoom = responseResult.data;
                Debug.Log("join Room" + roomIdx.ToString());
            }
            else
            {
                Debug.Log(responseResult.data);
            }
        }
    }

    class ResponseResult
    {
        public int code;
        public int data;
    }
}
