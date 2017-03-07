using UnityEngine;
using System.Collections;
using Assets.script.utils;
using System.Collections.Generic;
using Assets.script.common;
using LitJson;

public class MenuAction : MonoBehaviour {
    public GameObject operateCardObj = null;
    //.GetComponent<ShowCardInfo>().cardId

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
        }
    }

    private void closeMyself()
    {
        // 关闭自己
        this.operateCardObj = null;
        this.gameObject.SetActive(false);
    }

    public void CallAtkMonster()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", "CallMonsterFromHand");
        paramsMap.Add("HandCardIdx", this.operateCardObj.transform.GetSiblingIndex().ToString());
        paramsMap.Add("MonsterStatus", "2");// 2表示攻击表示
        string response = HttpClient.sendPost("http://localhost:8080/YgoService/duel-controller/action",
            paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] != 0)
        {
            Debug.Log((string)responseResult["data"]);
        }
        closeMyself();
    }

    public void CallHideMonster()
    {
        closeMyself();
    }

    public void PutMagic()
    {
        closeMyself();
    }

    public void CallMagic()
    {
        closeMyself();
    }
}
