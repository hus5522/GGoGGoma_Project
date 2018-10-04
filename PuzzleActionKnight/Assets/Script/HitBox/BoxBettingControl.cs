using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxBettingControl : MonoBehaviour {

    public static BoxBettingControl instance = null;

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
    private bool hundredExcess;
    private int tenUnit;
    private bool tenExcess;
    private int oneUnit;
    private bool oneExcess;
    private int bettingPrice;

    public bool isBetting;

    private bool overHundred;
    private bool overTen;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        hundredUnit = 0;
        tenUnit = 0;
        oneUnit = 0;
        hundredExcess = false;
        tenExcess = false;
        oneExcess = false;
        bettingPrice = 0;
        isBetting = false;
        hundredDownArrow.image.color = Color.gray;
        tenDownArrow.image.color = Color.gray;
        oneDownArrow.image.color = Color.gray;
        overHundred = false;
        overTen = false;

        if (WholeGameManager.instance.MoneyCheck >= 100)
        {
            overHundred = true;
        }

        if (WholeGameManager.instance.MoneyCheck >= 10)
        {
            overTen = true;
        }
    }

    public void Betting()
    {
        bettingPrice = 100 * hundredUnit + 10 * tenUnit + oneUnit;

        if (WholeGameManager.instance.MoneyCheck < bettingPrice || bettingPrice == 0) //수정된 라인
        {
            return;
        }

        bettingText.text = "현재 베팅 : " + bettingPrice;
        isBetting = true;

        BoxGameManager.instance.CheckBettingWindow();
        BoxGameManager.instance.Betting = bettingPrice;    // 베팅 값 넘겨줌
        BoxGameManager.instance.StartGame();

        Debug.Log("게임시작");
    }

    public void exitButton()
    {
        BoxGameManager.instance.GoToMainSelect();
    }

    public void HundredUpCount()
    {
        if (hundredUnit != 9 && !hundredExcess && hundredUnit != (WholeGameManager.instance.MoneyCheck / 100))
        {
            hundredUnit++;
            hundredText.text = "" + hundredUnit;

            if (hundredUnit == (WholeGameManager.instance.MoneyCheck / 100))
            {
                hundredExcess = true;
                hundredUpArrow.image.color = Color.gray;
                return;
            }
        }
        else
        {
            hundredExcess = true;
            hundredUpArrow.image.color = Color.gray;
            return;
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
            hundredExcess = false;
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

        if (hundredUnit == (WholeGameManager.instance.MoneyCheck / 100) && (WholeGameManager.instance.MoneyCheck >= 100) && (WholeGameManager.instance.MoneyCheck / 10) == 0)
        {
            return;
        }

        if (overHundred && tenUnit != (WholeGameManager.instance.MoneyCheck - (WholeGameManager.instance.MoneyCheck / 100 * 100)) / 10 && tenUnit != 9)
        {
            tenUnit++;
            tenText.text = "" + tenUnit;
        }
        else
        {
            if (hundredUnit == (WholeGameManager.instance.MoneyCheck / 100) && tenUnit == (WholeGameManager.instance.MoneyCheck - (WholeGameManager.instance.MoneyCheck / 100 * 100)) / 10)
            {
                tenExcess = true;
                tenUpArrow.image.color = Color.gray;
                return;
            }
            else
            {
                if (tenUnit == 9)
                    return;

                tenUnit++;
                tenText.text = "" + tenUnit;
            }
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
            tenExcess = false;
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

        if (WholeGameManager.instance.MoneyCheck == 0)
        {
            return;
        }

        if ((overHundred || overTen) && oneUnit != (WholeGameManager.instance.MoneyCheck % 10) && oneUnit != 9)
        {
            oneUnit++;
            oneText.text = "" + oneUnit;
        }
        else
        {
            if (oneUnit == (WholeGameManager.instance.MoneyCheck % 10) && hundredUnit == (WholeGameManager.instance.MoneyCheck / 100) && tenUnit == (WholeGameManager.instance.MoneyCheck - (WholeGameManager.instance.MoneyCheck / 100 * 100)) / 10)
            {
                oneExcess = true;
                oneUpArrow.image.color = Color.gray;
                return;
            }
            else
            {
                if (oneUnit == 9)
                    return;

                oneUnit++;
                oneText.text = "" + oneUnit;
            }
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
            oneExcess = false;
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
