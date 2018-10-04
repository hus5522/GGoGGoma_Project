using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack : MonoBehaviour {

    List<int> cards;

    public bool isGameDeck;

    // 굳이 없어도 됨 (이걸 안쓰려면 CardStackView의 ShowCard() 안의 if문 지우면 됨
    public bool HasCards
    {
        get { return cards != null && cards.Count > 0; }
    }

    public event CardEventHandler CardRemoved;
    public event CardEventHandler CardAdded;

    public int CardCount
    {
        get
        {
            if (cards == null)
            {
                return 0;
            }
            else
            {
                return cards.Count;
            }
        }
    }

    //private한 cards의 List에 접근할 수 있게 하기 위함 (read-only)
    public IEnumerable<int> GetCards()
    {
        foreach (int i in cards)
        {
            yield return i;
        }
    }

    //덱의 맨 위의 카드부터 뽑는 함수
    public int Pop()
    {
        int temp = cards[0];
        cards.RemoveAt(0);

        //eventHandler를 통해 삭제된 cardIndex를 별도 저장변수에 저장함
        if (CardRemoved != null)
        {
            CardRemoved(this, new CardEventArgs(temp));
        }

        return temp;
    }
    

    public void Push(int card)
    {
        
        cards.Add(card);

        if (CardAdded != null)
        {
            CardAdded(this, new CardEventArgs(card));
        }
    }

    public int HandValue()
    {
        int total = 0;
        int aces = 0;

        foreach (int card in GetCards())
        {
            // 13으로 나눈 나머지 = 0 1 2 3 4 5 6 7 8  9  10 11 12
            // 실제 카드의 수치     = A 2 3 4 5 6 7 8 9 10  J   Q  K 
            int cardRank = card % 13;


            if (cardRank < 9 && cardRank > 0)
            {
                // 해당 카드가 2~9 라면
                cardRank += 1;
                total += cardRank;
            }
            else if (cardRank >= 9 && cardRank <= 12)
            {
                // 해당 카드가 J Q K 라면
                cardRank = 10;
                total += cardRank;
            }
            else
            {
                // 해당 카드가 A 라면 1 or 11의 실제 수치를 가짐
                aces++; // A를 받았다면 개수 한개 추가
            }
        }

        for (int i = 0; i < aces; i++)
        {
            // A의 값을 넣기 전의 합에 11을 더해서 21이 안넘으면 A의 값을 11로.
            if (total + 11 <= 21)
            {
                total += 11;
            }
            else
            {
                // 21이 넘는다면 A의 값을 1로..
                total += 1;
            }
        }

        return total;
    }

    public void CreateDeck()
    {
        cards.Clear();

        // List에 카드들을 섞이지않게 일단 넣음
        for (int i = 0; i < 52; i++)
        {
            cards.Add(i);
        }

        // n은 현재 섞어야할 카드 위치를 알려줌, k는 섞어야할 카드와 바꿀 카드의 위치를 알려줌
        // temp는 n번쨰와 k번째 카드를 바꾸기 위한 임시변수
        int n = cards.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            int temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;
        }

    }

	// Use this for initialization
	void Awake () {
        cards = new List<int>();
        if (isGameDeck)
        {
            CreateDeck();
        }
    }

}
