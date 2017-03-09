using UnityEngine;
using System.Collections;
using Assets.script.utils;
using System.Collections.Generic;
using Assets.script.common;
using LitJson;

public class MenuAction : MonoBehaviour {
    public GameObject operateCardObj = null;

    // Use this for initialization
    void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
        // 点击左键取消菜单
        if (Input.GetMouseButtonDown(0))
        {
            // 如果不在自己的范围内
            if (!GUIOp.isInGUI(Input.mousePosition, this.gameObject))
            {
                closeMyself();
            }
        }else if (Input.GetMouseButtonDown(1)){
            // 右键直接取消菜单
            closeMyself();
        }
    }

    public void closeMyself()
    {
        Destroy(this.gameObject);
    }
}
