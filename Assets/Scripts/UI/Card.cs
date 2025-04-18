using System;
using Sirenix.OdinInspector;

[Serializable]
public class Card
{
    [TableColumnWidth(100, Resizable = false)]
    public CardType cardType;
    [TableColumnWidth(200, Resizable = false)]
    public Spell spell;
    [InlineEditor] public SpellConfig spellConfig;
}