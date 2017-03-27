using UnityEngine;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // 计算图像位置
        calImagePosX();
        // 初始化you lose位置
        initLoseLogo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void calImagePosX()
    {
        GameObject imageObj = GameObject.Find("LosePanel/Image");
        // 获取高度
        float height = imageObj.GetComponent<RectTransform>().rect.height;
        float width = height * 1114 / 1740;
        imageObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        imageObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-width / 2, 0);
    }

    private void initLoseLogo()
    {
        GameObject textObj = GameObject.Find("LosePanel/Text");
        textObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, textObj.GetComponent<RectTransform>().rect.height * 3 / 2);
    }
}
