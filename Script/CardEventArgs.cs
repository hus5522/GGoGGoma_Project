using System;

public class CardEventArgs : EventArgs
{
    //property
    public int CardIndex { get; private set; }

    //constructor
    public CardEventArgs(int cardIndex)
    {
        CardIndex = cardIndex;
    }
}