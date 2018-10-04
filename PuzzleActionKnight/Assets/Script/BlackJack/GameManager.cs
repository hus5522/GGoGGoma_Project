using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour {

    /* 베팅 창 관련 변수 */
    [SerializeField]
    private GameObject bettingWindow;

    private bool isClosedBettingWindow;

    public static GameManager instance = null;

    private int betting = 0;

    public int Betting
    {
        get { return betting; }
        set { betting = value; }
    }

    /* 게임 로직 관련 변수 */
    public CardStack player;
    public CardStack dealer;
    public CardStack deck;

    public Button DrawButton;
    public Button StopButton;

    private bool isStartedGame;

    int dealersFirstCard = -1;

    //private int playerMoney;

    /* 게임 결과 텍스트 관련 변수 */
    public Text WhosWin;
    public Text PlayerScore;
    public Text DealerScore;
    public int dealerScore;

    [SerializeField]
    private GameObject ExitWindow;
    [SerializeField]
    private GameObject WinWindow;
    [SerializeField]
    private GameObject LoseWindow;


    public string file_path;
    public StreamWriter stream_write;



    public void Hit()
    {
        player.Push(deck.Pop());
        SoundManager.PlaySound("drawCard");
        PlayerScore.text = "Player Score : " + player.HandValue();
        if (player.HandValue() > 21)
        {
            // To do : The Player is bust
            // You lose..  문구 띄우기
            // 문구 띄워지고, 아무 버튼이나 누르면 배너 없어지고 연속진행창 띄움
            DrawButton.interactable = false;
            StopButton.interactable = false;
            StartCoroutine(DealersTurn());
        }
    }

    public void Stick()
    {
        // Todo : dealer
        DrawButton.interactable = false;
        StopButton.interactable = false;
        StartCoroutine(DealersTurn());
    }

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
        isStartedGame = true;
        WhosWin.enabled = false;
        PlayerScore.enabled = false;
        DealerScore.enabled = false;
        dealerScore = 0;
        SoundManager.PlaySound("bgm_floor4");
        //playerMoney = WholeGameManager.instance.MoneyCheck;
    }


    public void CheckBettingWindow()
    {
        //다시 게임을 시작할떄 isBetting을 false로 만들어줘야함
        if (BettingControl.instance.isBetting && !isClosedBettingWindow)
        {
            WhosWin.enabled = true;
            PlayerScore.enabled = true;
            DealerScore.enabled = true;
            bettingWindow.SetActive(false);
            isClosedBettingWindow = true;
            isStartedGame = true;
        }
        else if(!BettingControl.instance.isBetting && isClosedBettingWindow)
        {
            WhosWin.enabled = false;
            PlayerScore.enabled = false;
            DealerScore.enabled = false;
            bettingWindow.SetActive(true);
            isClosedBettingWindow = false;
            isStartedGame = false;
        }
    }

    public void StartGame()
    {
        /*
        for (int i = 0; i < 2; i++)
        {
            player.Push(deck.Pop());
            PlayerScore.text = "Player Score : " + player.HandValue();
            HitDealer();
            dealerScore += dealer.HandValue();
        }
        */

        StartCoroutine(GiveCards());
        
    }

    IEnumerator GiveCards()
    {
        
        Time.timeScale = 0;
        Time.timeScale = 1;
        
        DrawButton.interactable = false;
        StopButton.interactable = false;

        for (int i = 0; i < 2; i++)
        {
            player.Push(deck.Pop());
            SoundManager.PlaySound("drawCard");
            PlayerScore.text = "Player Score : " + player.HandValue();
            yield return new WaitForSeconds(0.5f);

            HitDealer();
            dealerScore += dealer.HandValue();
            yield return new WaitForSeconds(0.5f);
        }

        DrawButton.interactable = true;
        StopButton.interactable = true;

        yield break;
        
    }

    void HitDealer()
    {
        int card = deck.Pop();

        // 딜러의 첫 번째 카드 오픈
        if (dealersFirstCard < 0)
        {
            dealersFirstCard = card;
        }

        dealer.Push(card);
        SoundManager.PlaySound("drawCard");
        // 딜러가 카드를 한번 뽑은적이 있다면, 앞면이 보이게 함.
        // 딜러의 카드 1장을 가리고, 나머지는 계속 보이게 하기 위함
        if (dealer.CardCount >= 2)
        {
            CardStackView view = dealer.GetComponent<CardStackView>();
            view.Toggle(card, true);
        }
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

    IEnumerator DealersTurn()
    {
        CardStackView view = dealer.GetComponent<CardStackView>();
        view.Toggle(dealersFirstCard, true);
        view.ShowCard();    // update함수에서 ShowCard를 불러오지못하므로, 딜러 카드를 수동으로 뒤집기
        DealerScore.text = "Dealer Score : " + dealer.HandValue();
        yield return new WaitForSeconds(1f);

        while (dealer.HandValue() < 17) {
            HitDealer();
            DealerScore.text = "Dealer Score : " + dealer.HandValue();
            yield return new WaitForSeconds(1f);
        }
        

        if (player.HandValue() > 21 || (dealer.HandValue() >= player.HandValue() && dealer.HandValue() <= 21))
        {
            isStartedGame = false;
            WholeGameManager.instance.MoneyCheck -= Betting;

            StoreHealth();

            WhosWin.enabled = false;
            PlayerScore.enabled = false;
            DealerScore.enabled = false;

            ShowLoseWindow();
        }
        else if (dealer.HandValue() > 21 || (player.HandValue() <= 21 && player.HandValue() > dealer.HandValue()))
        {
            isStartedGame = false;
            WholeGameManager.instance.MoneyCheck += Betting;

            StoreHealth();

            WhosWin.enabled = false;
            PlayerScore.enabled = false;
            DealerScore.enabled = false;

            ShowWinWindow();
        }
        else if (dealer.HandValue() > 17 && player.HandValue() == dealer.HandValue())
        {
            isStartedGame = false;
            WholeGameManager.instance.MoneyCheck -= Betting;

            StoreHealth();

            WhosWin.enabled = false;
            PlayerScore.enabled = false;
            DealerScore.enabled = false;

            ShowLoseWindow();
        }

        //Debug.Log(WholeGameManager.instance.MoneyCheck);
        
    }

    // WinWindow -> 예 버튼 에서 사용할 함수
    public void Restart()
    {
        if (WholeGameManager.instance.MoneyCheck == 0)
            SceneManager.LoadScene("Ending");
        else
            SceneManager.LoadScene("BlackJack");
            
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
        WhosWin.enabled = false;
        PlayerScore.enabled = false;
        DealerScore.enabled = false;
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
        WhosWin.enabled = true;
        PlayerScore.enabled = true;
        DealerScore.enabled = true;
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
