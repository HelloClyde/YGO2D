using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.script.utils
{
    class GUIOp
    {
        static public bool isInGUI(Vector3 mousePostion, GameObject guiObj)
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
}
