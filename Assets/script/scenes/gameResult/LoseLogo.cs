using UnityEngine;
using System.Collections;
using System;

public class LoseLogo : MonoBehaviour {
    public float speed = 10; 

	// Use this for initialization
	void Start () {

    }
	
	void FixedUpdate() {
        float oldPosY = this.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        if (oldPosY > 0)
        {
            this.gameObject.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(0, oldPosY - speed);
        }
    }
}
