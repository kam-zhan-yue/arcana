using System;
using Sirenix.OdinInspector;

[Serializable]
public class Card
{
    [TableColumnWidth(100, Resizable = false)]
    public CardType cardType;
    [InlineEditor] public SpellConfig spellConfig;
}