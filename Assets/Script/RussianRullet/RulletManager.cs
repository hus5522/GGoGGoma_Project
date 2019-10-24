using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class RulletManager : MonoBehaviour {
    
    public static RulletManager instance = null;

    /* 베팅 창 관련 변수 */
    [SerializeField]
    private GameObject bettingWindow;

    private bool isClosedBettingWindow;

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
    [SerializeField]
    private GameObject WhoIsFirstWindow;

    public string file_path;
    public StreamWriter stream_write;

    /* 게임 로직 관련 변수 */
    [SerializeField]
    private GameObject dealerPosition;
    [SerializeField]
    private GameObject userPosition;
    [SerializeField]
    private GameObject gun;
    [SerializeField]
    private GameObject shootEffect;
    [SerializeField]
    private GameObject dealerObject;
    [SerializeField]
    private GameObject userObject;
    [SerializeField]
    private Text bulletCount;

    private int[] bullet;             // 장전된 총알들
    private int remainBullet;    //남은 총알 수
    private int totalBullet;        //전체 총알 수
    private int rulletPoint;        //총알 순서
    private bool myTurn;        // 내차례
    private bool yourTurn;      // 상대 차례
    private bool isShooted;    // 발사를 했는가의 여부
    private bool isMovedGun;    // 총이 움직이는 중인가 여부

    private bool deadline;      // 총 발사 직전 여부
    private bool gameover;    // 결과창 발생 여부

    private bool realStart;
    
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

    // Use this for initialization
    void Start () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        ExitWindow.SetActive(false);
        WinWindow.SetActive(false);
        LoseWindow.SetActive(false);
        WhoIsFirstWindow.SetActive(false);
        shootEffect.SetActive(false);
        isClosedBettingWindow = false;
        bullet = new int[6];
        remainBullet = 6;
        totalBullet = 6;
        rulletPoint = 0;
        myTurn = false;
        yourTurn = false;
        isShooted = false;
        isMovedGun = false;
        deadline = false;
        gameover = false;
        realStart = false;
    }

    void Update()
    {
        HandleInput();

        if (yourTurn && isShooted && gun.transform.position != userPosition.transform.position && !deadline )
        {
            gun.transform.position = Vector3.MoveTowards(gun.transform.position, userPosition.transform.position, 3.2f * Time.deltaTime);
            
            isMovedGun = true;
            if (gun.transform.position == userPosition.transform.position)
            {
                myTurn = true;
                yourTurn = false;
                isShooted = false;
                isMovedGun = false;
            }
        }
        else if (myTurn && isShooted && gun.transform.position != dealerPosition.transform.position && !deadline )
        {
            gun.transform.position = Vector3.MoveTowards(gun.transform.position, dealerPosition.transform.position, 3.2f * Time.deltaTime);

            isMovedGun = true;
            if (gun.transform.position == dealerPosition.transform.position)
            {
                myTurn = false;
                yourTurn = true;
                isShooted = false;
                isMovedGun = false;
            }
        }
    }

    public void StartGame()
    {
        WhoIsFirstWindow.SetActive(true);
        bool isReloadRealBullet = false;

        //총알 장전 로직
        for (int i = 0; i < 6; i++)
        {
            int random = Random.Range(0, 3);
            if (random == 1 && !isReloadRealBullet)
            {
                // 0~2까지 랜덤으로해서 1이 나왔으면 실탄 장전
                bullet[i] = 1;
                isReloadRealBullet = true;  //실탄 장전됨
            } else if (i == 5 && !isReloadRealBullet)
            {
                // 5번째 총알까지 공기탄이면, 6번째는 자동으로 실탄 장전
                bullet[i] = 1;
            }
            else
            {
                // 1이 아니면 공기탄 장전
                bullet[i] = 0;
            }
        }

        for (int i = 0; i < 6; i++)
        {
            Debug.Log("bullet[" + i + "] = " + bullet[i]);
        }
        
        bulletCount.text = "남은 총알 수 : " + remainBullet + " / " + totalBullet;
    }

    public void ChoiceYourFate()
    {

        if (bullet[rulletPoint] == 0 && myTurn)
        {
            rulletPoint++;
            remainBullet--;
            bulletCount.text = "남은 총알 수 : " + remainBullet + " / " + totalBullet;
            SoundManager.PlaySound("tick");
        }
        else if (bullet[rulletPoint] == 0 && yourTurn)
        {
            rulletPoint++;
            remainBullet--;
            bulletCount.text = "남은 총알 수 : " + remainBullet + " / " + totalBullet;
            SoundManager.PlaySound("tick");
        }
        else if (bullet[rulletPoint] == 1 && myTurn)
        {
            deadline = true;
            SoundManager.PlaySound("shootGun");
            StartCoroutine(LoadResult());
        }
        else if (bullet[rulletPoint] == 1 && yourTurn)
        {
            deadline = true;
            SoundManager.PlaySound("shootGun");
            StartCoroutine(LoadResult());
        }

        isShooted = true;

    }

    IEnumerator LoadResult()
    {
        if (myTurn)
        {
            shootEffect.transform.position = userObject.transform.position;
            shootEffect.SetActive(true);

            WholeGameManager.instance.MoneyCheck -= Betting;
            StoreHealth();

            gameover = true;

            yield return new WaitForSeconds(0.5f);

            ShowLoseWindow();
        }
        else if (yourTurn)
        {
            shootEffect.transform.position = dealerObject.transform.position;
            shootEffect.SetActive(true);

            WholeGameManager.instance.MoneyCheck += Betting;
            StoreHealth();

            gameover = true;

            yield return new WaitForSeconds(0.5f);

            ShowWinWindow();
        }
    }

    public void MeFirst()
    {
        if (!realStart)
        {
            WhoIsFirstWindow.SetActive(false);
            gun.transform.position = userPosition.transform.position;
            myTurn = true;
            yourTurn = false;
            realStart = true;
        }
    }

    public void YouFirst()
    {
        if (!realStart)
        {
            WhoIsFirstWindow.SetActive(false);
            gun.transform.position = dealerPosition.transform.position;
            myTurn = false;
            yourTurn = true;
            realStart = true;
        }
    }


    public void CheckBettingWindow()
    {
        //다시 게임을 시작할떄 isBetting을 false로 만들어줘야함
        if (RulletBettingControl.instance.isBetting && !isClosedBettingWindow)
        {
            bettingWindow.SetActive(false);
            isClosedBettingWindow = true;
        }
        else if (!RulletBettingControl.instance.isBetting && isClosedBettingWindow)
        {
            bettingWindow.SetActive(true);
            isClosedBettingWindow = false;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMovedGun && !gameover && RulletBettingControl.instance.isGameStarted && realStart)
        {
            //space바를 눌렀을때, 격발하고, 공기탄이면 다음차례로, 실탄이면 게임결과창 띄우기
            ChoiceYourFate();
        }
    }

    // WinWindow -> 예 버튼 에서 사용할 함수
    public void Restart()
    {
        if (WholeGameManager.instance.MoneyCheck == 0)
            SceneManager.LoadScene("Ending");
        else
            SceneManager.LoadScene("RussianRullet");
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
        Time.timeScale = 0; // 일시정지
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
