using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CardContainer : MonoBehaviour
{
    [SerializeField] public CardDatabase cardDatabase;
    [SerializeField] public CardType[] startingHand;
    [SerializeField] public CardSlot cardSlotPrefab;
    [SerializeField] public CardPopupItem cardPopupItemPrefab;
    [SerializeField] public VisualCardContainer visualCardContainer;
    
    private readonly List<CardPopupItem> _cards = new List<CardPopupItem>();
    private CardPopupItem _selectedCardPopupItem = null;

    private void Start()
    {
        for (int i = 0; i < startingHand.Length; ++i)
        {
            AddCard(startingHand[i]);
        }
    }

    private void AddCard(CardType cardType)
    {
        int cardSlots = transform.childCount;
        int cards = _cards.Count;
        // If we don't have enough card slots, then make a new card slot
        if (cardSlots < cards + 1)
        {
            Instantiate(cardSlotPrefab, transform);
        }
        
        // Then, create a card and put it at the end of the card slot
        CardPopupItem cardPopupItem = Instantiate(cardPopupItemPrefab, transform.GetChild(transform.childCount - 1));
        cardPopupItem.BeginDrag += OnBeginDrag;
        cardPopupItem.EndDrag += OnEndDrag;
        
        // Also, create a new visual card for the card
        Spell spellPrefab = cardDatabase.GetCard(cardType).spell;
        Spell spell = Instantiate(spellPrefab, visualCardContainer.transform);
        spell.Init(cardPopupItem);
        _cards.Add(cardPopupItem);
    }


    private void Update()
    {
        if (_selectedCardPopupItem == null)
            return;

        for (int i = 0; i < _cards.Count; ++i)
        {
            // If the selected card is to the right of the card, and it was previously to the left, then swap 
            if (_selectedCardPopupItem.transform.position.x > _cards[i].transform.position.x)
            {
                if (_selectedCardPopupItem.GetParentIndex() < _cards[i].GetParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (_selectedCardPopupItem.transform.position.x < _cards[i].transform.position.x)
            {
                if (_selectedCardPopupItem.GetParentIndex() > _cards[i].GetParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    private void Swap(int index)
    {
        (_selectedCardPopupItem.transform.parent, _cards[index].transform.parent) = (_cards[index].transform.parent, _selectedCardPopupItem.transform.parent);
    }

    private void OnBeginDrag(CardPopupItem cardPopupItem)
    {
        _selectedCardPopupItem = cardPopupItem;
    }

    private void OnEndDrag(CardPopupItem cardPopupItem)
    {
        _selectedCardPopupItem = null;
    }
}
