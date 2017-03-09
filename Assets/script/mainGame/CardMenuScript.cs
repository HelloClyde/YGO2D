using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.script.utils;

public class CardMenuScript : MonoBehaviour {
    public GameObject menuPrefab;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
            {
                // 生成菜单
                GameObject menuPanelObj = Instantiate(this.menuPrefab);
                // 菜单挂到canvas下
                menuPanelObj.transform.SetParent(GameObject.Find("Canvas").transform);
                // 设置菜单大小
                menuPanelObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,0.5f);
                menuPanelObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                float menuWidth = GameObject.Find("MHandPanel/Scroll View/Viewport/Content").GetComponent<GridLayoutGroup>().cellSize.x;
                menuPanelObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, menuWidth);
                menuPanelObj.GetComponent<GridLayoutGroup>().cellSize = new Vector2(menuWidth, menuWidth / 160 * 50);
                // 设置菜单位置
                menuPanelObj.transform.position = new Vector3(
                    this.gameObject.transform.position.x,
                    this.gameObject.transform.position.y + this.gameObject.GetComponent<RectTransform>().rect.height);
                // 设置菜单需要操作的目标卡片
                menuPanelObj.GetComponent<MenuAction>().operateCardObj = this.gameObject;
            }
        }
    }

}
