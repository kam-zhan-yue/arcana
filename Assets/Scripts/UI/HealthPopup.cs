using System;
using System.Collections.Generic;
using Kuroneko.UIDelivery;
using Kuroneko.UtilityDelivery;
using UnityEngine;
using UnityEngine.UI;

public class HealthPopup : Popup
{
    [SerializeField] private RectTransform container;
    [SerializeField] private Image sampleHealthPopupItem;

    private List<Image> _items = new();
    private Player _player;
    private Game _game;
    
    protected override void InitPopup()
    {
        HidePopup();
        _game = ServiceLocator.Instance.Get<IGameManager>().GetGame();
        _game.OnGameStart += ShowPopup;
    }

    private void Start()
    {
        _player = ServiceLocator.Instance.Get<IGameManager>().GetGame().Player;
        int health = _player.Health;
        sampleHealthPopupItem.gameObject.SetActiveFast(true);
        for (int i = 0; i < health; ++i)
        {
            Image healthPopupItem = Instantiate(sampleHealthPopupItem, container);
            _items.Add(healthPopupItem);
        }
        sampleHealthPopupItem.gameObject.SetActiveFast(false);
        _player.OnHealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(int health)
    {
        for (int i = 0; i < _items.Count; ++i)
        {
            _items[i].gameObject.SetActiveFast(health > i);
        }
    }

    private void OnDestroy()
    {
        _player.OnHealthChanged -= OnHealthChanged;
        _game.OnGameStart -= ShowPopup;
    }
}
