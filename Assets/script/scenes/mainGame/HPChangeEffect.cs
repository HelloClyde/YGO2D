using UnityEngine;
using UnityEngine.UI;

public class HPChangeEffect : MonoBehaviour {
    private bool isMove;
    public float posYSpeed = -10;
    public float initPosY = 100;
    public float desPosY = -100;

    // Use this for initialization
    void Start () {
        this.isMove = false;
	}

    void FixedUpdate()
    {
        if (this.isMove)
        {
            float oldPosY = this.gameObject.GetComponent<RectTransform>().anchoredPosition.y;
            float oldPosX = this.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
            if (this.posYSpeed > 0)
            {
                if (oldPosY < desPosY)
                {
                    this.gameObject.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(oldPosX, oldPosY + this.posYSpeed);
                }
                else
                {
                    this.gameObject.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(oldPosX, this.initPosY);
                    this.isMove = false;
                }
            }
            else
            {
                if (oldPosY > desPosY)
                {
                    this.gameObject.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(oldPosX, oldPosY + this.posYSpeed);
                }
                else
                {
                    this.gameObject.GetComponent<RectTransform>().anchoredPosition =
                        new Vector2(oldPosX, this.initPosY);
                    this.isMove = false;
                }
            }
        }
    }
    

    public void setHp(int hp)
    {
        if (hp < 0)
        {
            this.gameObject.GetComponent<Text>().color = new Color(255, 0, 0);
            this.gameObject.GetComponent<Text>().text = hp.ToString();
        }
        else
        {
            this.gameObject.GetComponent<Text>().color = new Color(0, 255, 0);
            this.gameObject.GetComponent<Text>().text = "+" + hp.ToString();
        }
        this.isMove = true;
    }
}
