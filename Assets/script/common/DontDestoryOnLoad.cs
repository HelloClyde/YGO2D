using UnityEngine;
using System.Collections;

public class DontDestoryOnLoad : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
}
