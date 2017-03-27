using Assets.script.common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class WinCard : MonoBehaviour {
    public float speed = 0.01f;

    // Use this for initialization
    void Start () {
        loadDrawedCard();
	}

    void FixedUpdate()
    {
        float oldScale = this.gameObject.GetComponent<RectTransform>().localScale.x;
        if (oldScale < 1)
        {
            this.gameObject.GetComponent<RectTransform>().localScale = 
                new Vector3(oldScale + this.speed, oldScale + this.speed, oldScale + this.speed);
        }
    }


    private void loadDrawedCard()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/duel-controller/draw-card", paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] == 0)
        {
            int cardId = (int)jsonData["data"];
            GameObject cardObj = GameObject.Find("WinPanel/Card");
            cardObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardImage/" + (cardId - 1).ToString());
        }
        else
        {
            MsgBox.showMsg((string)jsonData["data"]);
        }

    }
}
