using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LitJson;

public class HandContentFit : MonoBehaviour {
    private GridLayoutGroup gridLayoutGroup;
    public GameObject desSizeObj;

    // Use this for initialization
    void Start ()
    {
        this.gridLayoutGroup = this.gameObject.GetComponent<GridLayoutGroup>();
    }
	
	// Update is called once per frame
	void Update () {
        // 设置content宽度一致
        float childCount = this.gameObject.transform.childCount;
        float width = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right +
            (gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x) * childCount;
        this.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

        // 设置卡片宽高一致
        // 获取桌面标准宽高
        Rect extraCardRect = desSizeObj.GetComponent<RectTransform>().rect;
        // Debug.Log("w:" + extraCardRect.width + ",h:" + extraCardRect.height);
        this.gridLayoutGroup.cellSize = new Vector2(extraCardRect.width, extraCardRect.height);
    }
}
