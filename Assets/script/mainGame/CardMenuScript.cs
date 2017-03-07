using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.script.utils;

public class CardMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
            {
                Debug.Log(this.gameObject.name);
                // 移动菜单到卡牌边
                GameObject menuPanelObj = GameObject.Find("Canvas").GetComponent<MainGame>().menuPanelObj;
                menuPanelObj.SetActive(true);
                // 设置菜单大小
                float menuWidth = GameObject.Find("MMagicPanel/Extra").GetComponent<RectTransform>().rect.width;
                // menuPanelObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors()
                menuPanelObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, menuWidth);
                menuPanelObj.GetComponent<GridLayoutGroup>().cellSize = new Vector2(menuWidth, menuWidth / 160 * 50);
                menuPanelObj.transform.position = new Vector3(
                    this.gameObject.transform.position.x + this.gameObject.GetComponent<RectTransform>().rect.width,
                    this.gameObject.transform.position.y + this.gameObject.GetComponent<RectTransform>().rect.height);
                // 设置菜单需要操作的目标卡片
                menuPanelObj.GetComponent<MenuAction>().operateCardId = this.gameObject.GetComponent<ShowCardInfo>().cardId;
            }
        }
    }

}
