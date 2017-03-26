using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Assets.script.common;
using LitJson;
using UnityEngine.SceneManagement;
using System;

public class GameHallMain : MonoBehaviour {
    GameObject roomPrefab;
    public long tickDelta = (long)(1 * 10000000);// 单位是100毫微秒，即1s更新一次
    private long oldTick;

    // Use this for initialization
    void Start () {
        this.roomPrefab = (GameObject)Resources.Load("prefab/RoomPrefab");
        // 第一次读取房间信息
        loadRoomsInfo(true);
        calContentHeight();
        this.oldTick = DateTime.Now.Ticks;
    }

    private void calContentHeight()
    {
        GridLayoutGroup gridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
        float childCount = this.gameObject.transform.childCount;
        int colNum = (int)((this.gameObject.transform.parent.parent.gameObject.GetComponent<RectTransform>().rect.width - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right))
            / (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x));
        float height = (int)Math.Ceiling(childCount / colNum) * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y)
            + gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;
        this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    private void loadRoomsInfo(bool isInit = false)
    {
        string response = HttpClient.sendPost(App.serverPath + "YgoService/hall/lists",
                new Dictionary<string, object>());
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] == 0)
        {
            for (int roomIdx = 0; roomIdx < responseResult["data"].Count; roomIdx++) {
                GameObject roomObj;
                if (isInit)
                {
                    // 创建roomObj
                    roomObj = Instantiate(this.roomPrefab);
                    roomObj.name = roomIdx.ToString();
                    roomObj.transform.SetParent(this.gameObject.transform);
                }else
                {
                    roomObj = this.gameObject.transform.GetChild(roomIdx).gameObject;
                }
                // 将房间数据传入roomObj
                roomObj.GetComponent<RoomAction>().roomData = responseResult["data"][roomIdx];
                roomObj.GetComponent<RoomAction>().roomIdx = roomIdx;
                int playerNum = responseResult["data"][roomIdx]["playerInfos"].Count;
                // 根据房间信息贴图
                if (roomObj.GetComponent<RoomAction>().roomContainMe())
                {
                    roomObj.GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("image/ygo-room-" + (playerNum + 2).ToString());
                    // 如果两个玩家就绪了，就开始游戏
                    if (playerNum == 2)
                    {
                        SceneManager.LoadScene("MainGame");
                    }
                }else
                {
                    roomObj.GetComponent<Image>().sprite =
                        Resources.Load<Sprite>("image/ygo-room-" + playerNum.ToString());
                }
            }
        }else
        {
            MsgBox.showMsg((string)responseResult["data"]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTime.Now.Ticks > this.oldTick + this.tickDelta)
        {
            loadRoomsInfo();
            this.oldTick = DateTime.Now.Ticks;
        }
    }

    private bool roomContainMe(JsonData jsonData)
    {
        for (int i = 0;i < jsonData["playerInfos"].Count;i++)
        {
            if ((string)jsonData["playerInfos"][i]["userName"] == UserInfo.email)
            {
                return true;
            }
        }
        return false;
    }

    public void returnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

