using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    private List<Card> _cards = new List<Card>();
    private Card _selectedCard = null;

    private void Awake()
    {
        Card[] cardArray = GetComponentsInChildren<Card>();
        _cards = cardArray.ToList();
        for (int i = 0; i < _cards.Count; ++i)
        {
            _cards[i].BeginDrag += OnBeginDrag;
            _cards[i].EndDrag += OnEndDrag;
        }
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
