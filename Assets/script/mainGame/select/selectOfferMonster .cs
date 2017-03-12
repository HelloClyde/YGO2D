using Assets.script.common;
using Assets.script.utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class selectOfferMonster: MonoBehaviour
{

    void Start()
    {

    }
    
    void Update()
    {
        // 右键选中
        if (Input.GetMouseButtonDown(1) && App.isOfferState)
        {
            if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
            {
                showSelectCursor();
                // 判断自己是否已经存在
                if (isExistInArray(this.gameObject.transform.parent.gameObject.name))
                {
                    clearSelectCursor();
                    App.isOfferState = false;
                    return;
                }else
                {
                    App.selectList.Add(this.gameObject.transform.parent.gameObject.name);
                }
                if (App.selectList.Count == App.selectLimit)
                {
                    CallAction();
                    clearSelectCursor();
                    App.isOfferState = false;
                }
            }
        }
    }

    private bool isExistInArray(string name)
    {
        for (int i = 0;i < App.selectList.Count; i++)
        {
            if (App.selectList[i].Equals(name))
            {
                return true;
            }
        }
        return false;
    }

    private void showSelectCursor()
    {
        // 添加图标
        GameObject selectCursorObj = Instantiate(Resources.Load<GameObject>("prefab/SelectCursor"));
        // 挂在TempPanel下
        GameObject tempPanelObj = GameObject.Find("TempPanel");
        selectCursorObj.transform.SetParent(tempPanelObj.transform);
        // 获取卡牌位置
        float cardX = this.gameObject.GetComponent<RectTransform>().position.x;
        float cardY = this.gameObject.GetComponent<RectTransform>().position.y;
        float cardW = this.gameObject.GetComponent<RectTransform>().rect.width;
        // 位置设置在当前gameobject中心，宽度一致
        selectCursorObj.GetComponent<RectTransform>().position = new Vector3(cardX, cardY);
        selectCursorObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cardW);
        selectCursorObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cardW);

    }

    public static void clearSelectCursor()
    {
        GameObject tempPanelObj = GameObject.Find("TempPanel");
        App.selectList.Clear();
        // 删除全部临时图标
        for (int i = 0; i < tempPanelObj.transform.childCount; i++)
        {
            Destroy(tempPanelObj.transform.GetChild(i).gameObject);
        }
    }

    public void CallAction()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        if (App.selectList.Count == 1)
        {
            paramsMap.Add("action", "CallMiddleMonster");
            paramsMap.Add("MonsterIdx", App.selectList[0].Substring(App.selectList[0].Length - 1));
            paramsMap.Add("HandCardIdx", App.handIdx);
            paramsMap.Add("Status", App.monsterStatus);
        }
        else if (App.selectList.Count == 2)
        {
            paramsMap.Add("action", "CallHighMonster");
            paramsMap.Add("MonsterIdx1", App.selectList[0].Substring(App.selectList[0].Length - 1));
            paramsMap.Add("MonsterIdx2", App.selectList[1].Substring(App.selectList[1].Length - 1));
            paramsMap.Add("HandCardIdx", App.handIdx);
            paramsMap.Add("Status", App.monsterStatus);
        }
        string response = HttpClient.sendPost(App.serverPath + "YgoService/duel-controller/action",
            paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] != 0)
        {
            GUIOp.showMsg((string)responseResult["data"]);
        }
    }
    
}
