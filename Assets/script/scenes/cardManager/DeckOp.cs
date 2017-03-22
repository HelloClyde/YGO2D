using UnityEngine;
using System.Collections;
using Assets.script.utils;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeckOp : MonoBehaviour {
    private Dictionary<int, int> deckCards = null;
    public GameObject cardNumIconObj = null;

	// Use this for initialization
	void Start () {
        this.deckCards = GameObject.Find("CardsBox").GetComponent<CardBox>().deckCards;
	}
	
	// Update is called once per frame
	void Update () {
        // 检测按键
        if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
        {
            if (Input.GetMouseButtonDown(0))
            {
                addCardToDeck();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                subCardFromDeck();
            }
        }
	}

    public void addCardToDeck()
    {
        int cardId = this.gameObject.GetComponent<ShowCardInfo>().cardId;
        if (getCardNum(cardId) < 3)
        {
            if (this.deckCards.ContainsKey(cardId))
            {
                this.deckCards[cardId]++;
            }else
            {
                this.deckCards[cardId] = 1;
            }
            if (this.deckCards[cardId] == 1)
            {
                // 添加卡片obj
                insertCardObjToDeck(cardId);
            }
            resetCardNum();
            Debug.Log(this.gameObject.GetComponent<ShowCardInfo>().cardId.ToString() + "add");
        }
    }

    private void insertCardObjToDeck(int cardId)
    {
        float posY = -105f / 2 - 10;
        int posYDelta = 30;
        GameObject deckCardContentObj = GameObject.Find("DeckCardsPanel/Scroll View/Viewport/Content");
        GameObject cardPrefab = Resources.Load<GameObject>("prefab/CardPrefab");
        GameObject cardInfoImageObj = GameObject.Find("CardInfoPanel/CardImage");
        GameObject cardInfoTextObj = GameObject.Find("CardInfoPanel/CardDetailScrollView/Viewport/Content/Text");
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
        posY -= posYDelta * (deckCardContentObj.transform.childCount - 1);
        deckCardObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
        // 设置卡片封面
        deckCardObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardImage/" + (cardId - 1).ToString());
        // 设置卡片id
        deckCardObj.GetComponent<ShowCardInfo>().cardId = cardId;
        deckCardObj.GetComponent<ShowCardInfo>().cardInfoImageObj = cardInfoImageObj;
        deckCardObj.GetComponent<ShowCardInfo>().cardInfoTextObj = cardInfoTextObj;
        deckCardContentObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (this.deckCards.Count - 1) * posYDelta + 105 + 10 * 2);
    }

    private void resetDeckObjPosition()
    {
        float posY = -105f / 2 - 10;
        int posYDelta = 30;
        GameObject deckCardContentObj = GameObject.Find("DeckCardsPanel/Scroll View/Viewport/Content");
        for (int i = 0;i < deckCardContentObj.transform.childCount;i++)
        {
            GameObject deckCardObj = deckCardContentObj.transform.GetChild(i).gameObject;
            deckCardObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
            // Debug.Log("name:" + deckCardObj.name + " posY:" + posY);
            posY -= posYDelta;
        }
        deckCardContentObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (deckCardContentObj.transform.childCount - 1) * posYDelta + 105 + 10 * 2);
    }

    public void subCardFromDeck()
    {
        int cardId = this.gameObject.GetComponent<ShowCardInfo>().cardId;
        if (getCardNum(cardId) > 0)
        {
            this.deckCards[cardId] --;
            if (this.deckCards[cardId] == 0)
            {
                // 删除牌组obj中卡片
                DestroyImmediate(GameObject.Find("DeckCardsPanel/Scroll View/Viewport/Content/" + cardId));
                resetDeckObjPosition();
            }
            resetCardNum();
            Debug.Log(this.gameObject.GetComponent<ShowCardInfo>().cardId.ToString() + "sub");
        }
    }

    private void resetCardNum()
    {
        int cardNum = getCardNum(this.gameObject.GetComponent<ShowCardInfo>().cardId);
        if (cardNum == 0)
        {
            this.cardNumIconObj.SetActive(false);
        }else
        {
            this.cardNumIconObj.SetActive(true);
            this.gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = cardNum.ToString();
        }
    }

    private int getCardNum(int cardId)
    {
        if (this.deckCards.ContainsKey(cardId))
        {
            return this.deckCards[cardId];
        }else
        {
            return 0;
        }
    }
}
