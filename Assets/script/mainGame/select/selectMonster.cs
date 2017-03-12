using Assets.script.common;
using Assets.script.utils;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class selectMonster: MonoBehaviour
{

    void Start()
    {

    }
    
    void Update()
    {
        // 右键选中
        if (Input.GetMouseButtonDown(1) && App.TurnState.Equals("BP"))
        {
            if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
            {
                // 判断是敌方怪兽还是我方怪兽
                if (this.gameObject.transform.parent.parent.parent.parent.gameObject.name.Equals("MyPanel"))
                {
                    if (App.selectList.Count != 0)
                    {
                        clearSelectCursor();
                        return;
                    }
                }else
                {
                    if (App.selectList.Count == 0)
                    {
                        GUIOp.showMsg("先选择我方怪兽");
                        return;
                    }
                }
                showSelectCursor();
                App.selectList.Add(this.gameObject.transform.parent.gameObject.name);
                Debug.Log("aaa");
                // 判断敌方是否有怪兽
                if (!existEnemyMonster())
                {
                    AttackMonsterAction(true);
                    clearSelectCursor();
                }
                else
                {
                    Debug.Log("App.selectList.Count:" + App.selectList.Count);
                    Debug.Log("App.selectLimit:" + App.selectLimit);
                    if (App.selectList.Count == App.selectLimit)
                    {
                        AttackMonsterAction();
                        clearSelectCursor();
                    }
                }
            }
        }
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

    private bool existEnemyMonster()
    {
        GameObject enemyDeck = GameObject.Find("EnemyPanel/DuelDeck/Monster");
        for (int i = 0;i < enemyDeck.transform.childCount;i++)
        {
            if (enemyDeck.transform.GetChild(i).childCount != 0)
            {
                return true;
            }
        }
        return false;
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

    public void AttackMonsterAction(bool isAtkPlayer = false)
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", "AtkMonster");
        paramsMap.Add("SrcMonsterIdx", App.selectList[0].Substring(App.selectList[0].Length - 1));
        if (isAtkPlayer)
        {
            paramsMap.Add("DesMonsterIdx", "-1");
        }
        else
        {
            paramsMap.Add("DesMonsterIdx", App.selectList[1].Substring(App.selectList[1].Length - 1));
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
