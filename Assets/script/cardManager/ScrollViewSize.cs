using UnityEngine;
using System.Collections;

public class ScrollViewSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Content始终随着Text大小改变而改变
        GameObject scrollViewContent = this.gameObject;
        GameObject cardInfoTextObj = GameObject.Find("Canvas/MainPanel/InfoPanel/CardInfoPanel/Scroll View/Viewport/Content/Text");
        scrollViewContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            cardInfoTextObj.GetComponent<RectTransform>().rect.height);
    }
}
