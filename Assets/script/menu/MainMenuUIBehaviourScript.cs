using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void exitApplication()
    {
        Debug.Log("exit application");
        Application.Quit();
    }

    public void goHall()
    {
        Debug.Log("go scene 'Hall'");
        SceneManager.LoadScene("Hall");
    }

    public void goCardManager()
    {
        Debug.Log("go scene 'CardManager'");
        SceneManager.LoadScene("CardManager");
    }
    // 5933张卡片
}
