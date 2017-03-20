using UnityEngine;

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
    }
}
