using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CardStack))] // 이 DeckView는 Deck이라는 요소가 항상 필요하다고 컴파일러에 명시
public class CardStackView : MonoBehaviour {

    CardStack deck;
    // int = Key, GameObject = value
    Dictionary<int, CardView> fetchedCards;
    int lastCount;

    public Vector3 start;   //start position
    public float cardOffset;
    public bool faceUp = false; //덱이 보이지않게 뒤집어진상태로 만들기위한 변수선언
    public bool reverseLayerOrder = false;  //덱의 위에서부터 카드를 뽑는것처럼 연출하기 위한 변수 선언
    public GameObject cardPrefab;
    
    public void Toggle(int card, bool isFaceUp)
    {
        fetchedCards[card].IsFaceUp = isFaceUp;
    }

    void Start()
    {
        fetchedCards = new Dictionary<int, CardView>();
        deck = GetComponent<CardStack>();
        ShowCard();
        lastCount = deck.CardCount;

        //이벤트 추가
        deck.CardRemoved += deck_CardRemoved;
        deck.CardAdded += deck_CardAdded;

    }

    private void deck_CardAdded(object sender, CardEventArgs e)
    {
        float co = cardOffset * deck.CardCount;
        Vector3 temp = start + new Vector3(co, 0f, 0f);
        AddCard(temp, e.CardIndex, deck.CardCount);
    }

    private void deck_CardRemoved(object sender, CardEventArgs e)
    {
        //해당 cardIndex가 Dictionary(fetchedCards)에 있으면 지우기.
        if (fetchedCards.ContainsKey(e.CardIndex))
        {
            Destroy(fetchedCards[e.CardIndex].Card);     // scene에서의 cardIndex에 해당하는 오브젝트 제거
            fetchedCards.Remove(e.CardIndex);     // dictionary에서의 cardIndex를 Key로 갖는 요소 제거
        }
    }

    void Update()
    {
        
        if (lastCount != deck.CardCount)
        {
            lastCount = deck.CardCount;
            ShowCard();
        }
        
    }

    public void ShowCard()
    {
        int cardCount = 0;

        if (deck.HasCards)
        {
            foreach (int i in deck.GetCards())
            {
                float co = cardOffset * cardCount;  // co = cardOffset

                Vector3 temp = start + new Vector3(co, 0f, 0f);
                AddCard(temp, i, cardCount);

                cardCount++;
            }
        }
    }

    void AddCard(Vector3 position, int cardIndex, int positionalIndex)
    {
        //이미 cardIndex에 해당하는 카드가 있으면, 넣지 않음.
        if (fetchedCards.ContainsKey(cardIndex))
        {
            if (!faceUp)
            {
                CardModel model = fetchedCards[cardIndex].Card.GetComponent<CardModel>();
                model.ToggleFace(fetchedCards[cardIndex].IsFaceUp);
                
            }

            return;
        }

        //cardPrefab에 해당하는 게임오브젝트를 생성하고 그것을 cardCopy라고 명명
        GameObject cardCopy = (GameObject)Instantiate(cardPrefab);
        cardCopy.transform.position = position; //카드를 한장씩 cardOffset만큼 밀리게함

        //cardPrefab 안에는 CardModel script가 있음
        CardModel cardModel = cardCopy.GetComponent<CardModel>();
        cardModel.cardIndex = cardIndex;
        cardModel.ToggleFace(faceUp);

        SpriteRenderer spriteRenderer = cardCopy.GetComponent<SpriteRenderer>();

        if (reverseLayerOrder)
        {
            spriteRenderer.sortingOrder = 51 - positionalIndex;
        }
        else
        {
            spriteRenderer.sortingOrder = positionalIndex;
        }

        fetchedCards.Add(cardIndex, new CardView(cardCopy));

        Debug.Log("Hand Value = " + deck.HandValue());
    }

}
