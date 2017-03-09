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

class HandMagicMenu : MenuAction
{
    public void CallMagic()
    {
        PutMagic(1);// 1代表表侧
        // 发动效果留空
        closeMyself();
    }

    public void PutMagic()
    {
        PutMagic(0);// 0代表里侧
        closeMyself();
    }

    private void PutMagic(int status)
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", "PutMagic");
        paramsMap.Add("HandCardIdx", this.operateCardObj.transform.GetSiblingIndex().ToString());
        paramsMap.Add("Status", status.ToString());
        string response = HttpClient.sendPost(App.serverPath + "YgoService/duel-controller/action",
            paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] != 0)
        {
            GUIOp.showMsg((string)responseResult["data"]);
        }
    }
}

