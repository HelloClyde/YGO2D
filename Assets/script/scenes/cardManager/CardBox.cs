using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using LitJson;
using Assets.script.common;
using System;
using UnityEngine.SceneManagement;

public class CardBox : MonoBehaviour {
    public List<int> enableCards;
    public List<int> userCards;
    public Dictionary<int,int> deckCards = new Dictionary<int, int>();
    public int eachPageNum;
    public int pageTotal;
    public int pageIdx = 0;

	// Use this for initialization
	void Start () {
        loadEnableCards();
        loadUserCards();
        loadUserDeck();
        calEachPageNum();
        loadCurPageCards();
        loadCurDeckCards();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void calEachPageNum()
    {
        Rect rect = this.gameObject.GetComponent<RectTransform>().rect;
        GridLayoutGroup gridLayout = this.gameObject.GetComponent<GridLayoutGroup>();
        int rowNum = (int)((rect.width - gridLayout.padding.left - gridLayout.padding.right)
            / (gridLayout.cellSize.x + gridLayout.spacing.x));
        int colNum = (int)((rect.height - gridLayout.padding.top - gridLayout.padding.bottom)
            / (gridLayout.cellSize.y + gridLayout.spacing.y));
        this.eachPageNum = rowNum * colNum;
        this.pageTotal = (int)Math.Ceiling((double)(this.enableCards.Count) / this.eachPageNum);
    }

    private void loadEnableCards()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        string response = HttpClient.sendGet(App.serverPath + "YgoService/card-manager/get-enable-cards", paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] == 0)
        {
            for (int i = 0; i < jsonData["data"].Count; i++)
            {
                this.enableCards.Add(int.Parse(jsonData["data"][i].ToString()));
            }
        }
    }

    private void loadUserCards() {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/user-op/get-cards", paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] == 0)
        {
            for (int i = 0; i < jsonData["data"].Count; i++)
            {
                this.userCards.Add(int.Parse(jsonData["data"][i].ToString()));
            }
        }
    }

    private void loadUserDeck()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/user-op/get-decks", paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] == 0)
        {
            for (int i = 0; i < jsonData["data"]["decks"].Count; i++)
            {
                int cardId = int.Parse(jsonData["data"]["decks"][i].ToString());
                if (this.deckCards.ContainsKey(cardId))
                {
                    this.deckCards[cardId]++;
                }
                else
                {
                    this.deckCards[cardId] = 1;
                }
            }
        }
    }

    private void loadCurDeckCards()
    {
        GameObject deckCardContentObj = GameObject.Find("DeckCardsPanel/Scroll View/Viewport/Content");
        GameObject cardPrefab = Resources.Load<GameObject>("prefab/CardPrefab");
        GameObject cardInfoImageObj = GameObject.Find("CardInfoPanel/CardImage");
        GameObject cardInfoTextObj = GameObject.Find("CardInfoPanel/CardDetailScrollView/Viewport/Content/Text");
        // 清空
        for (int i = 0;i < deckCardContentObj.transform.childCount;i++)
        {
            Destroy(deckCardContentObj.transform.GetChild(i).gameObject);
        }
        // 添加卡牌
        float posY = -105f/2 - 10;
        int posYDelta = 30;
        foreach (int cardId in this.deckCards.Keys)
        {
            if (this.deckCards[cardId] == 0) continue;
            GameObject deckCardObj = Instantiate(cardPrefab);
            deckCardObj.AddComponent<JumpCardPage>();
            deckCardObj.name = cardId.ToString();
            deckCardObj.transform.SetParent(deckCardContentObj.transform);
            // 设置锚点
            deckCardObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
            deckCardObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
            // 设置大小
            deckCardObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75);
            deckCardObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 105);
            // 设置位置
            deckCardObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
            posY -= posYDelta;
            // 设置卡片封面
            deckCardObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardImage/" + (cardId - 1).ToString());
            // 设置卡片id
            deckCardObj.GetComponent<ShowCardInfo>().cardId = cardId;
            deckCardObj.GetComponent<ShowCardInfo>().cardInfoImageObj = cardInfoImageObj;
            deckCardObj.GetComponent<ShowCardInfo>().cardInfoTextObj = cardInfoTextObj;
        }
        deckCardContentObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (deckCardContentObj.transform.childCount - 1) * posYDelta + 105 + 10*2);
    }

    public void loadCurPageCards()
    {
        // 清空
        for (int i = 0;i < this.gameObject.transform.childCount;i++)
        {
            Destroy(this.gameObject.transform.GetChild(i).gameObject);
        }
        // 添加卡片
        int startIdx = this.pageIdx * this.eachPageNum;
        GameObject cardPrefab = Resources.Load<GameObject>("prefab/CardPrefab");
        GameObject cardNumPrefab = Resources.Load<GameObject>("prefab/CardNumIcon");
        for (int cardIdx = startIdx; cardIdx < startIdx + this.eachPageNum && cardIdx < this.enableCards.Count; cardIdx++)
        {
            GameObject tempObj = Instantiate(cardPrefab);
            tempObj.GetComponent<ShowCardInfo>().cardInfoImageObj =
                GameObject.Find("CardInfoPanel/CardImage");
            tempObj.GetComponent<ShowCardInfo>().cardInfoTextObj =
                GameObject.Find("CardInfoPanel/CardDetailScrollView/Viewport/Content/Text");
            if (this.userCards.Contains(this.enableCards[cardIdx]))
            {
                tempObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardImage/" + (this.enableCards[cardIdx] - 1).ToString());
                tempObj.GetComponent<ShowCardInfo>().cardId = this.enableCards[cardIdx];
                tempObj.AddComponent<DeckOp>();
                // 添加卡片在卡组中的数量
                int cardNum = getCardNum(this.enableCards[cardIdx]);
                GameObject cardNumIcon = Instantiate(cardNumPrefab);
                cardNumIcon.transform.SetParent(tempObj.transform);
                cardNumIcon.transform.GetChild(0).gameObject.GetComponent<Text>().text = cardNum.ToString();
                tempObj.GetComponent<DeckOp>().cardNumIconObj = cardNumIcon;
                if (cardNum == 0)
                {
                    cardNumIcon.SetActive(false);
                }
            }
            else
            {
                tempObj.GetComponent<ShowCardInfo>().cardId = -1;
            }
            tempObj.transform.SetParent(this.gameObject.transform);
        }
        // 更新页码
        updatePageText();
        // 设置按钮状态
        resetButtonState();
    }

    private void updatePageText()
    {
        GameObject pageText = GameObject.Find("PageText");
        pageText.GetComponent<Text>().text = (this.pageIdx+1).ToString() + "/" + this.pageTotal.ToString();
    }

    public void prePage()
    {
        if (this.pageIdx > 0)
        {
            this.pageIdx--;
            loadCurPageCards();
        }
    }

    public void nextPage()
    {
        if (this.pageIdx < this.pageTotal - 1)
        {
            this.pageIdx++;
            loadCurPageCards();
        }
    }

    private void resetButtonState()
    {
        if (this.pageIdx <= 0)
        {
            GameObject.Find("PreButton").GetComponent<Button>().interactable = false;
        }else {
            GameObject.Find("PreButton").GetComponent<Button>().interactable = true;
        }
        if (this.pageIdx >= this.pageTotal - 1)
        {
            GameObject.Find("NextButton").GetComponent<Button>().interactable = false;
        }else
        {
            GameObject.Find("NextButton").GetComponent<Button>().interactable = true;
        }
    }

    public void returnMainMenu()
    {
        // 保存
        List<int> deckList = new List<int>();
        // 拍平
        foreach (int cardId in this.deckCards.Keys)
        {
            for (int i = 0;i < this.deckCards[cardId]; i++)
            {
                deckList.Add(cardId);
            }
        }
        // 上传
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("newDecks", deckList);
        string response = HttpClient.sendPost(App.serverPath + "YgoService/user-op/update-decks", paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] != 0)
        {
            MsgBox.showMsg((string)jsonData["data"]);
        }else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private int getCardNum(int cardId)
    {
        if (this.deckCards.ContainsKey(cardId))
        {
            return this.deckCards[cardId];
        }
        else
        {
            return 0;
        }
    }

}
