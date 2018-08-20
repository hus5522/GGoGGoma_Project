using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChangeCard : MonoBehaviour {

    CardFlipper flipper;
    CardModel cardModel;
    int cardIndex = 0;

    public GameObject card;

	// Use this for initialization
	void Awake () {
        cardModel = card.GetComponent<CardModel>();
        flipper = card.GetComponent<CardFlipper>();
	}
    /*
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 28), "Hit me!"))
        {

            if (cardIndex == 52)
            {
                cardIndex = 0;
                //마지막 장의 카드면 다시 뒷면으로 바꾸기
                flipper.FlipCard(cardModel.faces[cardModel.faces.Length - 1], cardModel.cardBack, -1);
            }
            else
            {
                if (cardIndex > 0)
                {
                    //이전카드에서 다음카드로 바꾸기
                    flipper.FlipCard(cardModel.faces[cardIndex - 1], cardModel.faces[cardIndex], cardIndex);
                }
                else
                {
                    //뒷면으로 되어있는카드를 뒤집어서 첫번째 카드의 앞면이 보이게 하기
                    flipper.FlipCard(cardModel.cardBack, cardModel.faces[cardIndex], cardIndex);
                }

                cardIndex++;
            }
            
        }
    }
    */
}
