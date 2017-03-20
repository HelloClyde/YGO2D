using UnityEngine;
using System.Collections;

public class ScrollViewSize : MonoBehaviour {
    private GameObject textObj;

	// Use this for initialization
	void Start () {
        this.textObj = this.gameObject.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        // Content始终随着Text大小改变而改变
        GameObject scrollViewContent = this.gameObject;
        scrollViewContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            this.textObj.GetComponent<RectTransform>().rect.height);
    }
}
