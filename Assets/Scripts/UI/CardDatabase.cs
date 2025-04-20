using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Card
{
    [TableColumnWidth(100, Resizable = false)]
    public CardType cardType;
    [InlineEditor] public SpellConfig spellConfig;
}

[Serializable]
public enum CardType
{
    Fireball,
    Freeze,
    WaterBucket,
    FlameWave,
    Gust,
    Snowball,
    Lightning,
    Thunder,
    Duck,
    Duplicate,
    Heal,
}

[CreateAssetMenu(menuName = "ScriptableObjects/Card Database", fileName = "Card Database")]
public class CardDatabase : ScriptableObject
{
    public CardType[] startingHand;
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
