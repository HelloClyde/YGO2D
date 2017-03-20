using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.script.common;
using UnityEngine.UI;
using LitJson;
using Assets.script.utils;

public class LoginAction : MonoBehaviour {

    private GameObject emailInput;
    private GameObject passwordInput;

    // Use this for initialization
    void Start()
    {
        this.emailInput = GameObject.Find("EmailInputField");
        this.passwordInput = GameObject.Find("PasswordInputField");
        // debug用，设置默认账号
        this.emailInput.GetComponent<InputField>().text = "test@qq.com";
        this.passwordInput.GetComponent<InputField>().text = "123456a";
    }

    public void login()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("email", this.emailInput.GetComponent<InputField>().text);
        paramsMap.Add("password", this.passwordInput.GetComponent<InputField>().text);
        string response = HttpClient.sendPost(App.serverPath + "YgoService/user-op/login", paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] == 0)
        {
            UserInfo.token = (string)jsonData["data"];
            UserInfo.email = new string(this.emailInput.GetComponent<InputField>().text.ToCharArray());
            Destroy(this.gameObject.transform.parent.gameObject);
        }
        else
        {
            MsgBox.showMsg((string)jsonData["data"]);
        }
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void register()
    {
        MsgBox.showMsg("注册仍未开放");
    }
}
