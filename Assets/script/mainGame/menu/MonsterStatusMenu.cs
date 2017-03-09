using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.script.utils;
using Assets.script.common;
using LitJson;

class MonsterStatusMenu : MenuAction
{
    public void ChangeStatus()
    {
        // 获取当前状态
        int status;
        Debug.Log("rotation z:" + this.operateCardObj.GetComponent<RectTransform>().rotation.z);
        float rotationAngle = 0;
        Vector3 rotationVector = new Vector3(0, 0, 1);
        this.operateCardObj.GetComponent<RectTransform>().rotation.ToAngleAxis(out rotationAngle, out rotationVector);
        Debug.Log("rotation z angle:" + rotationAngle);
        if (this.operateCardObj.GetComponent<RectTransform>().rotation.z != 0){
            status = 2;
        }else
        {
            status = 1;
        }
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", "ChangeMonsterStatus");
        string monsterParentName = this.operateCardObj.transform.parent.gameObject.name;
        paramsMap.Add("MonsterIdx", monsterParentName.Substring(monsterParentName.Length - 1));
        paramsMap.Add("Status", status.ToString());
        string response = HttpClient.sendPost(App.serverPath + "YgoService/duel-controller/action",
            paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] != 0)
        {
            GUIOp.showMsg((string)responseResult["data"]);
        }
        closeMyself();
    }
   
}

