using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WholeGameManager : MonoBehaviour {

    public static WholeGameManager instance = null;

    private static int playerMoney = 1000;

    public int MoneyCheck
    {
        get { return playerMoney; }
        set { playerMoney = value; }
    }

    /* UI */
    [SerializeField]
    private GameObject[] play_Floor;
    [SerializeField]
    private GameObject[] help_Floor;
    [SerializeField]
    private Text moneyText;

    void Start()
    {
        
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        SetPlayFloor();
        SetHelpFloor();
        moneyText.text = "현재 소지금 : " + MoneyCheck;
    }

    public void SetPlayFloor()
    {
        for (int i = 0; i < play_Floor.Length; i++)
        {
            play_Floor[i].SetActive(false);
        }
    }

    public void SetHelpFloor()
    {
        for (int i = 0; i < help_Floor.Length; i++)
        {
            help_Floor[i].SetActive(false);
        }
    }

    public void Floor4_Click()
    {
        SetPlayFloor();
        SetHelpFloor();
        play_Floor[3].SetActive(true);
        help_Floor[3].SetActive(true);
    }

    public void Floor3_Click()
    {
        SetPlayFloor();
        SetHelpFloor();
        play_Floor[2].SetActive(true);
        help_Floor[2].SetActive(true);
    }

    public void Floor2_Click()
    {
        SetPlayFloor();
        SetHelpFloor();
        play_Floor[1].SetActive(true);
        help_Floor[1].SetActive(true);
    }

    public void Floor1_Click()
    {
        SetPlayFloor();
        SetHelpFloor();
        play_Floor[0].SetActive(true);
        help_Floor[0].SetActive(true);
    }

    public void StartFloor4()
    {
        SceneManager.LoadScene("BlackJack");
    }

    public void StartFloor3()
    {
        // 총알피하기 게임  불러오기
    }

    public void StartFloor2()
    {
        // 블록뺴기 불러오기
    }

    public void StartFloor1()
    {
        // 러시안 룰렛 불러오기
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
