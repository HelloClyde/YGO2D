using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.script.common;
using UnityEngine.SceneManagement;

public class MsgBox : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OkButtonAction()
    {
        Destroy(GameObject.Find("Canvas/MsgPanel"));
    }
}
