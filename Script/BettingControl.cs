using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BettingControl : MonoBehaviour {

    public static BettingControl instance = null;

    public Button hundredUpArrow;
    public Button hundredDownArrow;
    public Button tenUpArrow;
    public Button tenDownArrow;
    public Button oneUpArrow;
    public Button oneDownArrow;

    [SerializeField]
    private Text hundredText;

    [SerializeField]
    private Text tenText;

    [SerializeField]
    private Text oneText;

    [SerializeField]
    private Text bettingText;
    

    private int hundredUnit;
    private int tenUnit;
    private int oneUnit;
    private int bettingPrice;

    public bool isBetting;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        hundredUnit = 0;
        tenUnit = 0;
        oneUnit = 0;
        bettingPrice = 0;
        isBetting = false;
        hundredDownArrow.image.color = Color.gray;
        tenDownArrow.image.color = Color.gray;
        oneDownArrow.image.color = Color.gray;
    }

    public void Betting()
    {
        bettingPrice = 100 * hundredUnit + 10 * tenUnit + oneUnit;

        if (WholeGameManager.instance.MoneyCheck < bettingPrice)
        {
            return;
        }

        bettingText.text = "현재 베팅 : " + bettingPrice;
        isBetting = true;

        GameManager.instance.CheckBettingWindow();
        GameManager.instance.Betting = bettingPrice;    // 베팅 값 넘겨줌
        GameManager.instance.WhosWin.enabled = true;
        GameManager.instance.PlayerScore.enabled = true;
        GameManager.instance.DealerScore.enabled = true;
        GameManager.instance.StartGame();
    }

    public void exitButton()
    {
        GameManager.instance.GoToMainSelect();
    }

    public void HundredUpCount()
    {
        if (hundredUnit != 9)
        {
            hundredUnit++;
            hundredText.text = "" + hundredUnit;
        }
        if (hundredUnit != 0)
        {
            hundredDownArrow.image.color = Color.white;
        }
        if (hundredUnit == 9)
        {
            hundredUpArrow.image.color = Color.gray;
        }
    }

    public void HundredDownCount()
    {
        if (hundredUnit != 0)
        {
            hundredUnit--;
            hundredText.text = "" + hundredUnit;
        }
        if (hundredUnit == 0)
        {
            hundredDownArrow.image.color = Color.gray;
        }
        if (hundredUnit != 9)
        {
            hundredUpArrow.image.color = Color.white;
        }
    }

    public void TenUpCount()
    {
        if (tenUnit != 9)
        {
            tenUnit++;
            tenText.text = "" + tenUnit;
        }
        if (tenUnit != 0)
        {
            tenDownArrow.image.color = Color.white;
        }
        if (tenUnit == 9)
        {
            tenUpArrow.image.color = Color.gray;
        }
    }

    public void TenDownCount()
    {
        if (tenUnit != 0)
        {
            tenUnit--;
            tenText.text = "" + tenUnit;
        }
        if (tenUnit == 0)
        {
            tenDownArrow.image.color = Color.gray;
        }
        if (tenUnit != 9)
        {
            tenUpArrow.image.color = Color.white;
        }
    }

    public void OneUpCount()
    {
        if (oneUnit != 9)
        {
            oneUnit++;
            oneText.text = "" + oneUnit;
        }
        if (oneUnit != 0)
        {
            oneDownArrow.image.color = Color.white;
        }
        if (oneUnit == 9)
        {
            oneUpArrow.image.color = Color.gray;
        }
    }

    public void OneDownCount()
    {
        if (oneUnit != 0)
        {
            oneUnit--;
            oneText.text = "" + oneUnit;
        }
        if (oneUnit == 0)
        {
            oneDownArrow.image.color = Color.gray;
        }
        if (oneUnit != 9)
        {
            oneUpArrow.image.color = Color.white;
        }
    }
}
