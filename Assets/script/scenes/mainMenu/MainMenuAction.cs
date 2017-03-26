using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Assets.script.utils;
using LitJson;
using Assets.script.common;

public class MainMenuAction : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        if (UserInfo.token == null || UserInfo.token.Equals(""))
        {
            GameObject loginBox = Instantiate(Resources.Load<GameObject>("prefab/LoginPanel"));
            loginBox.transform.SetParent(GameObject.Find("Canvas").transform);
            loginBox.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            loginBox.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            loginBox.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            loginBox.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        }
    }

    public void startGameAction()
    {
        SceneManager.LoadScene("GameHall");
    }

    public void cardManagerAction()
    {
        SceneManager.LoadScene("CardManager");
    }

    public void duelInfoAction()
    {
        MsgBox.showMsg("个人战绩暂时还不能查看");
    }

    public void exitGameAction()
    {
        Application.Quit();
    }
}
