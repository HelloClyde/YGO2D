using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Assets.script.common;
using Assets.script.utils;

public class ShowCardInfo : MonoBehaviour {
    public GameObject cardInfoImageObj;
    public GameObject cardInfoTextObj;
    public int cardId = -1;
    // 使用延迟
    private long duration;// -1表示上一次在该部件外
    public long durationLimit = (long)(0.2 * 10000000); // 1s延迟就触发


    // Use this for initialization
    void Start () {
        this.duration = -1;
    }
	
	// Update is called once per frame
	void Update () {
        if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
        {
            if (this.duration == -1)
            {
                this.duration = DateTime.Now.Ticks;
            }
            else
            {
                if (DateTime.Now.Ticks > this.duration + this.durationLimit)
                {
                    showCardInfo();
                }
            }
        }
        else
        {
            this.duration = -1;
        }
    }

    public void showCardInfo()
    {
        string imagePath;
        string text;
        // 从服务器获取卡片信息
        if (this.cardId == -1)
        {
            imagePath = "image/CardBack";
            text = "暂无信息";
        }else
        {
            imagePath = "image/CardImage/" + (this.cardId - 1).ToString();
            text = getCardInfo(this.cardId).ToString();
        }
        // 设置图片
        this.cardInfoImageObj.GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
        // 设置文字描述
        this.cardInfoTextObj.GetComponent<Text>().text = text;
        // 设置scroll view高度
        GameObject scrollViewContent = this.cardInfoTextObj.transform.parent.gameObject;
        scrollViewContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            cardInfoTextObj.GetComponent<RectTransform>().rect.height);
    }
    

    [Serializable]
    class ResponseResult
    {
        public int code;
        public CardInfo data;

    }

    static public CardInfo getCardInfo(int cardId)
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("cardId", cardId);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/card-manager/card-info", paramsMap);
        ResponseResult responseResult = JsonUtility.FromJson<ResponseResult>(response);
        return responseResult.data;
    }
}



[Serializable]
public class CardInfo
{
    public int id;
    public string name;
    public string type;
    public string race;
    public string attribute;
    public int starNum;
    public int atk;
    public int def;
    public string depict;

    public override string ToString()
    {
        string content = "";
        content += "卡片ID：" + id + "\n";
        content += "名称：" + name + "\n";
        content += "类型：" + type + "\n";
        content += "种族：" + race + "\n";
        content += "属性：" + attribute + "\n";
        content += "星级：" + starNum + "\n";
        content += "攻击力：" + atk + "\n";
        content += "防御力：" + def + "\n";
        content += depict;
        return content;
    }
}
