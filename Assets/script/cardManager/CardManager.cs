using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class CardManager : MonoBehaviour {

    private static int cardTotal = 100; //5933;
    private GameObject[] cards = new GameObject[CardManager.cardTotal];

	// Use this for initialization
	void Start () {
        GameObject cardPrefab = Resources.Load<GameObject>("fab/CardPrefab");
        GameObject scrollViewObj = GameObject.Find("Canvas/CardsPanel/Scroll View");
        GameObject scrollViewContent = GameObject.Find("Canvas/CardsPanel/Scroll View/Viewport/Content");
        for (int i = 0;i < CardManager.cardTotal;i++)
        {
            this.cards[i] = Instantiate(cardPrefab);
            this.cards[i].name = "card" + (i + 1).ToString();
            this.cards[i].GetComponent<ShowCardInfo>().cardId = (i + 1);
            this.cards[i].GetComponent<ShowCardInfo>().cardInfoImageObj = GameObject.Find("Canvas/CardInfoPanel/Image");
            this.cards[i].GetComponent<ShowCardInfo>().cardInfoTextObj = GameObject.Find("Canvas/CardInfoPanel/Scroll View/Viewport/Content/Text");
            this.cards[i].transform.SetParent(scrollViewContent.transform);
            this.cards[i].GetComponent<Image>().sprite =
                    Resources.Load<Sprite>("image/CardImage/" + (i + 1).ToString());
        }
        // 计算content高度
        GridLayoutGroup gridLayoutGroup = scrollViewContent.GetComponent<GridLayoutGroup>();
        float childCount = scrollViewContent.transform.childCount;
        Debug.Log(scrollViewObj.GetComponent<RectTransform>().rect.width);
        int colNum = (int)((scrollViewObj.GetComponent<RectTransform>().rect.width - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right + 20))
            / (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x));
        float height = (int)Math.Ceiling(childCount / colNum) * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y)
            + gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;
        scrollViewContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void returnMenu()
    {
        SceneManager.LoadScene("Menu");
    }
   
}
