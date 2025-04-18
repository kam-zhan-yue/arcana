using System.Collections.Generic;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    [SerializeField] public int cardsToSpawn;
    [SerializeField] public CardSlot cardSlotPrefab;
    // Temporary
    [SerializeField] public Card cardPrefab;
    [SerializeField] public VisualCard visualCardPrefab;
    [SerializeField] public VisualCardContainer visualCardContainer;
    
    private readonly List<Card> _cards = new List<Card>();
    private Card _selectedCard = null;

    private void Start()
    {
        for (int i = 0; i < cardsToSpawn; ++i)
        {
            AddCard();
        }
    }

    private void AddCard()
    {
        int cardSlots = transform.childCount;
        int cards = _cards.Count;
        // If we don't have enough card slots, then make a new card slot
        if (cardSlots < cards + 1)
        {
            Instantiate(cardSlotPrefab, transform);
        }
        
        // Then, create a card and put it at the end of the card slot
        Card card = Instantiate(cardPrefab, transform.GetChild(transform.childCount - 1));
        card.BeginDrag += OnBeginDrag;
        card.EndDrag += OnEndDrag;
        
        // Also, create a new visual card for the card
        VisualCard visualCard = Instantiate(visualCardPrefab, visualCardContainer.transform);
        visualCard.Init(card);
        _cards.Add(card);
    }


    private void Update()
    {
        if (_selectedCard == null)
            return;

        for (int i = 0; i < _cards.Count; ++i)
        {
            // If the selected card is to the right of the card, and it was previously to the left, then swap 
            if (_selectedCard.transform.position.x > _cards[i].transform.position.x)
            {
                if (_selectedCard.GetParentIndex() < _cards[i].GetParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (_selectedCard.transform.position.x < _cards[i].transform.position.x)
            {
                if (_selectedCard.GetParentIndex() > _cards[i].GetParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    private void Swap(int index)
    {
        (_selectedCard.transform.parent, _cards[index].transform.parent) = (_cards[index].transform.parent, _selectedCard.transform.parent);
    }

    private void OnBeginDrag(Card card)
    {
        _selectedCard = card;
    }

    private void OnEndDrag(Card card)
    {
        _selectedCard = null;
    }
}
