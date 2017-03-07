using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            if (isInGUI(Input.mousePosition, this.gameObject))
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
            }
        }
    }

    private bool isInGUI(Vector3 mousePostion, GameObject guiObj)
    {
        float halfWidth = guiObj.GetComponent<RectTransform>().rect.width / 2;
        float halfHeight = guiObj.GetComponent<RectTransform>().rect.height / 2;
        if (mousePostion.x >= guiObj.transform.position.x - halfWidth && mousePostion.x <= guiObj.transform.position.x + halfWidth &&
            mousePostion.y >= guiObj.transform.position.y - halfHeight && mousePostion.y <= guiObj.transform.position.y + halfHeight)
        {
            return true;
        }
        return false;
    }
}
