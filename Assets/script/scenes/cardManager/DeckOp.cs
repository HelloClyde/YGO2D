using UnityEngine;
using System.Collections;
using Assets.script.utils;
using System.Collections.Generic;
using UnityEngine.UI;

public class DeckOp : MonoBehaviour {
    private List<int> deckCards = null;
    public GameObject cardNumIconObj = null;

	// Use this for initialization
	void Start () {
        this.deckCards = GameObject.Find("CardsBox").GetComponent<CardBox>().deckCards;
	}
	
	// Update is called once per frame
	void Update () {
        // 检测右键
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
            this.deckCards.Add(cardId);
            resetCardNum();
            Debug.Log(this.gameObject.GetComponent<ShowCardInfo>().cardId.ToString() + "add");
        }
    }

    public void subCardFromDeck()
    {
        int cardId = this.gameObject.GetComponent<ShowCardInfo>().cardId;
        if (getCardNum(cardId) > 0)
        {
            this.deckCards.Remove(cardId);
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
        int n = 0;
        for (int i = 0;i < this.deckCards.Count;i++)
        {
            if (this.deckCards[i] == cardId)
            {
                n ++;
            }
        }
        return n;
    }
}
