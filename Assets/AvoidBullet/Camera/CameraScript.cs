using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class CameraScript : MonoBehaviour {

    /* 베팅 창 관련 변수 */
    [SerializeField]
    private GameObject bettingWindow;

    private bool isClosedBettingWindow;

    public static CameraScript instance = null;

    private int betting = 0;

    public int Betting
    {
        get { return betting; }
        set { betting = value; }
    }

    [SerializeField]
    private GameObject ExitWindow;
    [SerializeField]
    private GameObject WinWindow;
    [SerializeField]
    private GameObject LoseWindow;

    public string file_path;
    public StreamWriter stream_write;

    // Use this for initialization
    void Start ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        ExitWindow.SetActive(false);
        WinWindow.SetActive(false);
        LoseWindow.SetActive(false);
        isClosedBettingWindow = false;

        Time.timeScale = 0;

        SoundManager.PlaySound("bgm_floor3");
    }

    public void CheckBettingWindow()
    {
        //다시 게임을 시작할떄 isBetting을 false로 만들어줘야함
        if (AvoidBettingControl.instance.isBetting && !isClosedBettingWindow)
        {
            bettingWindow.SetActive(false);
            isClosedBettingWindow = true;
        }
        else if (!AvoidBettingControl.instance.isBetting && isClosedBettingWindow)
        {
            bettingWindow.SetActive(true);
            isClosedBettingWindow = false;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }

    public void StoreHealth()
    {
        // 현재 체력 정보를 저장.
        file_path = "Health.txt";
        stream_write = new StreamWriter(file_path);
        stream_write.WriteLine(WholeGameManager.instance.MoneyCheck);

        for (int i = 0; i < 4; i++)
            stream_write.WriteLine(WholeGameManager.instance.getFloor(i));

        stream_write.Close();
    }

    void OnTriggerEnter2D(Collider2D other)
    //rigidBody가 무언가와 충돌할때 호출되는 함수 입니다.
    //Collider2D other로 부딪힌 객체를 받아옵니다.
    {
        if (other.gameObject.tag.Equals("bullet")) {
            Destroy(other.gameObject);
        }

    }

    // WinWindow -> 예 버튼 에서 사용할 함수
    public void Restart()
    {
        if (WholeGameManager.instance.MoneyCheck == 0)
            SceneManager.LoadScene("Ending");
        else
            SceneManager.LoadScene("SampleScene");
    }

    public void GoToMainSelect()
    {
        SceneManager.LoadScene("MainGameScreen");
    }

    /*
     * ExitWindow => 예 버튼 (베팅한 돈 회수 후 메인 선택 화면으로), 아니오 버튼(창 비활성화)
     * LoseWindow => 예 버튼(메인 선택 화면으로), 아니오 버튼(창 비활성화)
     * WinWindow => 예 버튼 (블랙잭 재시작), 아니오 버튼(창 비활성화)
     * */

    public void ShowExitWindow()
    {
        ExitWindow.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowExitWindowYes()
    {
        WholeGameManager.instance.MoneyCheck -= Betting;

        StoreHealth();

        if (WholeGameManager.instance.MoneyCheck == 0)
            SceneManager.LoadScene("Ending");
        else
            GoToMainSelect();
    }

    public void ShowExitWindowNo()
    {
        ExitWindow.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowLoseWindow()
    {
        LoseWindow.SetActive(true);
    }

    public void ShowLoseWindowNo()
    {
        if (WholeGameManager.instance.MoneyCheck == 0)
            SceneManager.LoadScene("Ending");
        else
            GoToMainSelect();
    }

    public void ShowWinWindow()
    {
        WinWindow.SetActive(true);
    }

    public void ShowWinWindowNo()
    {
        GoToMainSelect();
    }
}
