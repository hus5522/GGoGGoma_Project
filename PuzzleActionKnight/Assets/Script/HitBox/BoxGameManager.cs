using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


public class BoxGameManager : MonoBehaviour
{
    public GameObject upArrowPrefab;
    public GameObject downArrowPrefab;
    public GameObject leftArrowPrefab;
    public GameObject rightArrowPrefab;

    private int arrowCount;
    private int timeCount;

    Queue<GameObject> arrowObjectQ = new Queue<GameObject>();
    Queue<string> arrowStringQ = new Queue<string>();

    private string tempArrow;

    public Text whatTimesLeft;

    /* 베팅 창 관련 변수 */
    [SerializeField]
    private GameObject bettingWindow;

    private bool isClosedBettingWindow;

    public static BoxGameManager instance = null;

    private int betting = 0;

    public int Betting
    {
        get { return betting; }
        set { betting = value; }
    }

    /* 게임 결과창 관련 변수 */
    [SerializeField]
    private GameObject ExitWindow;
    [SerializeField]
    private GameObject WinWindow;
    [SerializeField]
    private GameObject LoseWindow;

    public string file_path;
    public StreamWriter stream_write;

    private bool getMoney;

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
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        ExitWindow.SetActive(false);
        WinWindow.SetActive(false);
        LoseWindow.SetActive(false);
        isClosedBettingWindow = false;
        arrowCount = 0;
        timeCount = 0;
        tempArrow = "";
        whatTimesLeft.text = "남은 시간 : " + (26 - timeCount);
        getMoney = false;
        SoundManager.PlaySound("bgm_floor2");
        //StartCoroutine(CreateArrowBox());
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCount <= 26 && !getMoney) 
            HandleInput();
            
            //화살표 생성 코루틴
        
    }

    public void StartGame()
    {
        getMoney = false;
        StartCoroutine(CreateArrowBox());
        
    }

    public void CheckBettingWindow()
    {
        //다시 게임을 시작할떄 isBetting을 false로 만들어줘야함
        if (BoxBettingControl.instance.isBetting && !isClosedBettingWindow)
        {
            bettingWindow.SetActive(false);
            isClosedBettingWindow = true;
        }
        else if (!BoxBettingControl.instance.isBetting && isClosedBettingWindow)
        {
            bettingWindow.SetActive(true);
            isClosedBettingWindow = false;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tempArrow = "Up";
            if (arrowStringQ.Peek().Equals(tempArrow))
            {
                DeleteArrows();
            }
            else if (!arrowStringQ.Peek().Equals(tempArrow))
            {
                WholeGameManager.instance.MoneyCheck -= Betting;
                getMoney = true;
                StoreHealth();
                Time.timeScale = 0;
                ShowLoseWindow();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tempArrow = "Down";
            if (arrowStringQ.Peek().Equals(tempArrow))
            {
                DeleteArrows();
            }
            else if (!arrowStringQ.Peek().Equals(tempArrow))
            {
                WholeGameManager.instance.MoneyCheck -= Betting;
                getMoney = true;
                StoreHealth();
                Time.timeScale = 0;
                ShowLoseWindow();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tempArrow = "Left";
            if (arrowStringQ.Peek().Equals(tempArrow))
            {
                DeleteArrows();
            }
            else if (!arrowStringQ.Peek().Equals(tempArrow))
            {
                WholeGameManager.instance.MoneyCheck -= Betting;
                getMoney = true;
                StoreHealth();
                Time.timeScale = 0;
                ShowLoseWindow();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tempArrow = "Right";
            if (arrowStringQ.Peek().Equals(tempArrow))
            {
                DeleteArrows();
            }
            else if (!arrowStringQ.Peek().Equals(tempArrow))
            {
                WholeGameManager.instance.MoneyCheck -= Betting;
                getMoney = true;
                StoreHealth();
                Time.timeScale = 0;
                ShowLoseWindow();
            }
        }
    }

    public void AddArrows()
    {
        int arrNum = Random.Range(0, 4);
        Vector3 temp = new Vector3(0f, 10f, 0f); ;

        if (arrNum == 0)
        {
            GameObject arrowCopy = (GameObject)Instantiate(upArrowPrefab);
            arrowCopy.transform.position = temp;
            arrowObjectQ.Enqueue(arrowCopy);
            arrowStringQ.Enqueue("Up");
        }
        else if (arrNum == 1)
        {
            GameObject arrowCopy = (GameObject)Instantiate(downArrowPrefab);
            arrowCopy.transform.position = temp;
            arrowObjectQ.Enqueue(arrowCopy);
            arrowStringQ.Enqueue("Down");
        }
        else if (arrNum == 2)
        {
            GameObject arrowCopy = (GameObject)Instantiate(leftArrowPrefab);
            arrowCopy.transform.position = temp;
            arrowObjectQ.Enqueue(arrowCopy);
            arrowStringQ.Enqueue("Left");
        }
        else if (arrNum == 3)
        {
            GameObject arrowCopy = (GameObject)Instantiate(rightArrowPrefab);
            arrowCopy.transform.position = temp;
            arrowObjectQ.Enqueue(arrowCopy);
            arrowStringQ.Enqueue("Right");
        }

        arrowCount++;

    } // addArrows()

    public void DeleteArrows()
    {
        GameObject tempObject = arrowObjectQ.Dequeue();
        Destroy(tempObject);
        string garbageInt = arrowStringQ.Dequeue();
        arrowCount--;
    }

    IEnumerator CreateArrowBox()
    {
        
        Time.timeScale = 0;
        Time.timeScale = 1;

        yield return new WaitForSeconds(0.1f);
        while (timeCount <= 26)
        {
            if (timeCount <= 8)
            {
                AddArrows();
                yield return new WaitForSeconds(1.0f);

                timeCount++;
            }
            else if (timeCount > 8 && timeCount <= 16)
            {
                AddArrows();
                yield return new WaitForSeconds(0.5f);
                AddArrows();
                yield return new WaitForSeconds(0.5f);

                timeCount++;
            }
            else if (timeCount > 16)
            {
                AddArrows();
                yield return new WaitForSeconds(0.3333f);
                AddArrows();
                yield return new WaitForSeconds(0.3333f);
                AddArrows();
                yield return new WaitForSeconds(0.3333f);

                timeCount++;

            }//if-else

            whatTimesLeft.text = "남은 시간 : " + (27 - timeCount);

            if (arrowCount == 8)
            {
                timeCount = 30;
                WholeGameManager.instance.MoneyCheck -= Betting;
                getMoney = true;
                StoreHealth();
                ShowLoseWindow();
                yield return null;
            }
            
        }//while
        

        if (timeCount > 26 && arrowCount < 8)
        {
            ShowWinWindow();
            WholeGameManager.instance.MoneyCheck += Betting;
            getMoney = true;
            StoreHealth();
            yield return null;
        }

    }//CreateArrowBox

    // WinWindow -> 예 버튼 에서 사용할 함수
    public void Restart()
    {
        if (WholeGameManager.instance.MoneyCheck == 0)
            SceneManager.LoadScene("Ending");
        else
            SceneManager.LoadScene("HitBox");
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

