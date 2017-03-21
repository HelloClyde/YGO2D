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
    public List<int> deckCards;
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
                this.deckCards.Add(int.Parse(jsonData["data"]["decks"][i].ToString()));
            }
        }
    }

    private void loadCurPageCards()
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
        SceneManager.LoadScene("MainMenu");
    }

    private int getCardNum(int cardId)
    {
        int n = 0;
        for (int i = 0; i < this.deckCards.Count; i++)
        {
            if (this.deckCards[i] == cardId)
            {
                n++;
            }
        }
        return n;
    }

}
