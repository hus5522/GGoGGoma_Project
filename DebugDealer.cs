using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDealer : MonoBehaviour {

    public CardStack dealer;
    public CardStack player;

    /* debug test codes
    int count = 0;
    int[] cards = new int[] { 12, 7, 0 };
    */

    //dealer로부터 player로 카드 뽑기
    public void DrawCardToPlayer()
    {
        player.Push(dealer.Pop());
        //player.Push(cards[count++]);
    }

}
