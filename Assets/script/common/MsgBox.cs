using UnityEngine;
using UnityEngine.UI;

public class MsgBox : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OkButtonAction()
    {
        Destroy(GameObject.Find("Canvas/MsgPanel"));
    }

    static public void showMsg(string msg)
    {
        // 生成消息框
        GameObject msgPrefab = (GameObject)Resources.Load("prefab/MsgPanel");
        GameObject msgObj = UnityEngine.Object.Instantiate(msgPrefab);
        msgObj.name = "MsgPanel";
        // 加入canvas中
        GameObject canvas = GameObject.Find("Canvas");
        msgObj.transform.SetParent(canvas.transform);
        msgObj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        msgObj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        // 修改文字
        GameObject text = GameObject.Find("Canvas/MsgPanel/MsgBox/Msg");
        text.GetComponent<Text>().text = msg;
    }
}
