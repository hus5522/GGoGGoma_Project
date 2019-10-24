using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class WholeGameManager : MonoBehaviour {

    public static WholeGameManager instance = null;

    private static int playerMoney;

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
    private Button[] Floor;
    [SerializeField]
    private Text moneyText;

    public string file_path;
    public StreamWriter stream_write;
    public StreamReader stream_read;

    private static int[] playerFloor = new int[4];

    /*
    public int[] FloorCheck(int i) 
    {
        get { return playerFloor[i]; }
        set { playerFloor[i] = value; }
    }
    */
    public int getFloor(int i)
    {
        return playerFloor[i];
    }
    
    public void setFloor(int i, int value)
    {
        playerFloor[i] = value;
    }
    
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        SetPlayFloor();
        SetHelpFloor();

        //Health.txt 로부터 체력 정보 가져오기
        file_path = "Health.txt";
        stream_read = new StreamReader(file_path);
        MoneyCheck = int.Parse(stream_read.ReadLine());

        moneyText.text = "용사의 체력 : " + MoneyCheck;

        for (int i = 0; i < 4; i++)
        {
            Floor[i].interactable = false;
        }

        for (int i = 0; i < 4; i++)
        {
            //FloorCheck[i] = int.Parse(stream_read.ReadLine());
            setFloor(i, int.Parse(stream_read.ReadLine()));
            //playerFloor[i] = int.Parse(stream_read.ReadLine());
        }

        stream_read.Close();

        if (MoneyCheck >= 100 || getFloor(0) == 1)
        {
            Floor[0].interactable = true;
            setFloor(0, 1);
        }

        if (MoneyCheck >= 200 || getFloor(1) == 1)
        {
            Floor[1].interactable = true;
            setFloor(1, 1);
        }

        if (MoneyCheck >= 250 || getFloor(2) == 1)
        {
            Floor[2].interactable = true;
            setFloor(2, 1);
        }

        if (MoneyCheck >= 400 || getFloor(3) == 1)
        {
            Floor[3].interactable = true;
            setFloor(3, 1);
        }


    }

    void Update()
    {
        Cheat();
    }

    public void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            MoneyCheck += 100;
            moneyText.text = "용사의 체력 : " + MoneyCheck;

            // 현재 체력 정보를 저장.
            file_path = "Health.txt";
            stream_write = new StreamWriter(file_path);
            stream_write.WriteLine(MoneyCheck);

            for (int i = 0; i < 4; i++)
                stream_write.WriteLine(getFloor(i));

            stream_write.Close();
        }
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
        //블랙잭 불러오기
        SceneManager.LoadScene("BlackJack");
    }

    public void StartFloor3()
    {
        // 총알피하기 게임  불러오기
        SceneManager.LoadScene("SampleScene");
    }

    public void StartFloor2()
    {
        // 블록뺴기 불러오기
        SceneManager.LoadScene("HitBox");
    }

    public void StartFloor1()
    {
        // 러시안 룰렛 불러오기
        SceneManager.LoadScene("RussianRullet");
    }

    public void ExitGame()
    {
        // 현재 체력 정보를 저장.
        file_path = "Health.txt";
        stream_write = new StreamWriter(file_path);
        stream_write.WriteLine(MoneyCheck);

        for (int i = 0; i < 4; i++)
            stream_write.WriteLine(getFloor(i));

        stream_write.Close();
        Application.Quit();
    }

    public void RechargeHealth()
    {
        if (MoneyCheck == 0)
        {
            MoneyCheck = 100;
            moneyText.text = "용사의 체력 : " + MoneyCheck;

            // 현재 체력 정보를 저장.
            file_path = "Health.txt";
            stream_write = new StreamWriter(file_path);
            stream_write.WriteLine(MoneyCheck);

            for (int i = 0; i < 4; i++)
                stream_write.WriteLine(getFloor(i));

            stream_write.Close();
        }
    }


}
