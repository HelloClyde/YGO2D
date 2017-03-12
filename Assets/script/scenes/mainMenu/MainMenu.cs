using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    private GameObject audioObj;

	// Use this for initialization
	void Start () {
        // 设置主题音乐
        this.audioObj = GameObject.Find("ThemeMusicSource");
        if (this.audioObj == null)
        {
            this.audioObj = Instantiate(Resources.Load<GameObject>("prefab/ThemeMusicSource"));
            this.audioObj.name = "ThemeMusicSource";
        }
        // TODO生成登陆框
        InitLoginBox();
	}

    private void InitLoginBox()
    {

    }
}
