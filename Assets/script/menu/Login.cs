using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.script.common;
using System.Collections.Generic;

public class Login : MonoBehaviour {
    GameObject emailInput;
    GameObject passwordInput;
    GameObject mainMenuPanel;

	// Use this for initialization
	void Start () {
        if (UserInfo.isLogined == false)
        {
            // 先显示登陆界面
            this.mainMenuPanel = GameObject.Find("Canvas/Panel");
            mainMenuPanel.SetActive(false);
            // debug用，设置默认账号
            this.emailInput = GameObject.Find("LoginPanel/EmailInputField");
            this.passwordInput = GameObject.Find("LoginPanel/PasswordInputField");
            this.emailInput.GetComponent<InputField>().text = "test@qq.com";
            this.passwordInput.GetComponent<InputField>().text = "123456a";
        }else
        {
            // 直接显示主菜单
            this.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void login()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("email", this.emailInput.GetComponent<InputField>().text);
        paramsMap.Add("password", this.passwordInput.GetComponent<InputField>().text);
        string response = HttpClient.sendPost(App.serverPath + "YgoService/user-op/login", paramsMap);
        ResponseResult responseResult = JsonUtility.FromJson<ResponseResult>(response);
        if (responseResult.code == 0)
        {
            UserInfo.token = responseResult.data;
            UserInfo.email = new string(this.emailInput.GetComponent<InputField>().text.ToCharArray());
            this.gameObject.SetActive(false);
            this.mainMenuPanel.SetActive(true);
            UserInfo.isLogined = true;
            Debug.Log("login success.token:" + responseResult.data);
        }else
        {
            Debug.Log(responseResult.data);
        }
    }

    class ResponseResult
    {
        public int code;
        public string data;
    }
}

