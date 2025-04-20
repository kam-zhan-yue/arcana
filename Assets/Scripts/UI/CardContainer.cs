using System.Collections.Generic;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class CardContainer : MonoBehaviour
{
    [SerializeField] public CardDatabase cardDatabase;
    [SerializeField] public UISettings uiSettings;
    [SerializeField] public CardSlot cardSlotPrefab;
    [SerializeField] public CardPopupItem cardPopupItemPrefab;
    [SerializeField] public VisualCardContainer visualCardContainer;

    private CardPopup _cardPopup;
    private readonly List<CardPopupItem> _cards = new List<CardPopupItem>();
    private CardPopupItem _selectedCardPopupItem = null;

    private void Start()
    {
        Game game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
        game.OnCardAdded += AddCard;
        for (int i = 0; i < cardDatabase.startingHand.Length; ++i)
        {
            AddCard(cardDatabase.startingHand[i]);
        }
    }

    public void Init(CardPopup cardPopup)
    {
        _cardPopup = cardPopup;
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

        Card card = cardDatabase.GetCard(cardType);
        
        // Then, create a card and put it at the end of the card slot
        CardPopupItem cardPopupItem = Instantiate(cardPopupItemPrefab, transform.GetChild(transform.childCount - 1));
        cardPopupItem.BeginDrag += OnBeginDrag;
        cardPopupItem.EndDrag += OnEndDrag;
        
        // Also, create a new visual card for the card
        Spell spellPrefab = card.spellConfig.prefab;
        Spell spell = Instantiate(spellPrefab, visualCardContainer.transform);
        spell.Init(cardType, card.spellConfig, cardPopupItem, _cardPopup, uiSettings);
        _cards.Add(cardPopupItem);
    }

    public void RemoveCard(CardPopupItem cardPopupItem)
    {
        _cards.Remove(cardPopupItem);
        Destroy(cardPopupItem.transform.parent.gameObject);
    }


    private void Update()
    {
        if (_selectedCardPopupItem == null)
            return;
        Transform sharedParent = _selectedCardPopupItem.Rect.parent;

        for (int i = 0; i < _cards.Count; ++i)
        {
            Vector3 selectedLocalPos = sharedParent.InverseTransformPoint(_selectedCardPopupItem.Rect.position);
            Vector3 cardLocalPos = sharedParent.InverseTransformPoint(_cards[i].Rect.position);

            if (selectedLocalPos.x > cardLocalPos.x)
            {
                if (_selectedCardPopupItem.GetParentIndex() < _cards[i].GetParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedLocalPos.x < cardLocalPos.x)
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
        Transform tempParent = _selectedCardPopupItem.transform.parent;
        _selectedCardPopupItem.transform.SetParent(_cards[index].transform.parent);
        _cards[index].transform.SetParent(tempParent);
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
