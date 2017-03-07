using UnityEngine;
using System.Collections;
using Assets.script.utils;

public class MenuAction : MonoBehaviour {
    public int operateCardId = -1;

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
                this.operateCardId = -1;
                this.gameObject.SetActive(false);
            }
        }
    }

    public void CallAtkMonster()
    {

    }

    public void CallHideMonster()
    {

    }

    public void PutMagic()
    {

    }

    public void CallMagic()
    {

    }
}
