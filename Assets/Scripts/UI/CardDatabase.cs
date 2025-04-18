using System;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Card Database", fileName = "Card Database")]
public class CardDatabase : ScriptableObject
{
    [TableList] public Card[] cards = Array.Empty<Card>();

    public Card GetCard(CardType cardType)
    {
        for (int i = 0; i < cards.Length; ++i)
        {
            if (cards[i].cardType == cardType)
                return cards[i];
        }
        return null;
    }
}
