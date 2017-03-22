using UnityEngine;
using System.Collections;
using Assets.script.utils;
using System.Collections.Generic;

public class JumpCardPage : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
        {
            if (Input.GetMouseButtonDown(0))
            {
                jumpPage(this.gameObject.GetComponent<ShowCardInfo>().cardId);
            }
        }
    }

    private void jumpPage(int cardId)
    {
        CardBox desCardBox = GameObject.Find("CardsBox").GetComponent<CardBox>();
        List<int> enableCards = desCardBox.enableCards;
        int cardIdx = enableCards.IndexOf(cardId);
        desCardBox.pageIdx = cardIdx / desCardBox.eachPageNum;
        desCardBox.loadCurPageCards();
    }
}
