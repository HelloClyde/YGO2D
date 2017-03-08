using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.script.common;
using UnityEngine.SceneManagement;

public class HallScript : MonoBehaviour {
    private long oldTick;
    private static long tickDelta = 1 * 10000000;// 单位是100毫微秒，即2s更新一次
    private GameObject[] rooms;

    // Use this for initialization
    void Start () {
        // 加载预设体
        GameObject roomPrefab = (GameObject)Resources.Load("fab/RoomPrefab");
        GameObject scrollViewObj = GameObject.Find("Canvas/RoomsPanel/Scroll View");
        GameObject scrollViewContent = GameObject.Find("Canvas/RoomsPanel/Scroll View/Viewport/Content");
        // 从服务器获取初始化房间数据
        string response = HttpClient.sendPost(App.serverPath + "YgoService/hall/lists",
                new System.Collections.Generic.Dictionary<string, System.Object>());
        ResponseResult responseResult = JsonUtility.FromJson<ResponseResult>(response);
        rooms = new GameObject[responseResult.data.Count];
        for (int i = 0; i < responseResult.data.Count; i++)
        {
            rooms[i] = Instantiate(roomPrefab);
            rooms[i].name = "room" + i.ToString();
            rooms[i].GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("image/ygo-room-" + responseResult.data[i].playerNum.ToString());
            rooms[i].transform.SetParent(scrollViewContent.transform);
        }
        // 计算content高度
        GridLayoutGroup gridLayoutGroup = scrollViewContent.GetComponent<GridLayoutGroup>();
        float childCount = scrollViewContent.transform.childCount;
        int colNum = (int)((scrollViewObj.GetComponent<RectTransform>().rect.width - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right))
            / (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x));
        float height = (int)Math.Ceiling(childCount / colNum) * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y)
            + gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;
        scrollViewContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        // 初始化计时器
        this.oldTick = DateTime.Now.Ticks;
    }
	
	// Update is called once per frame
	void Update () {
	    if (DateTime.Now.Ticks > this.oldTick + HallScript.tickDelta)
        {
            // 更新数据
            Debug.Log("从服务器更新房间信息");
            string response = HttpClient.sendPost(App.serverPath + "YgoService/hall/lists", 
                new Dictionary<string, object>());
            ResponseResult responseResult = JsonUtility.FromJson<ResponseResult>(response);
            for (int i = 0;i < responseResult.data.Count;i++)
            {
                
                if (i == UserInfo.joinedRoom)
                {
                    // 如果是自己所在的房间，将使用不同的图标，以作区分
                    rooms[i].GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("image/ygo-room-" + (responseResult.data[i].playerNum + 2).ToString());
                    // 如果两个玩家就绪了，就开始游戏
                    if (responseResult.data[i].playerNum == 2)
                    {
                        SceneManager.LoadScene("MainGame");
                    }
                }
                else
                {
                    rooms[i].GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("image/ygo-room-" + responseResult.data[i].playerNum.ToString());
                }
            }
            // 更新时间间隔
            this.oldTick = DateTime.Now.Ticks;
        }
	}

    [Serializable]
    class ResponseResult
    {
        public int code;
        public List<RoomInfo> data;

    }
}
[Serializable]
class RoomInfo
{
    public int playerNum;

    override public String ToString()
    {
        return JsonUtility.ToJson(this);
    }
}
