using System;
using Sirenix.OdinInspector;

[Serializable]
public class Card
{
    [TableColumnWidth(100, Resizable = false)]
    public CardType cardType;
    [TableColumnWidth(200, Resizable = false)]
    public VisualCardPopupItem visualPopupItem;
    [InlineEditor] public SpellConfig spellConfig;
}