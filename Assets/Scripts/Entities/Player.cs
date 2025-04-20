using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera headCamera;
    [SerializeField] private Transform launchPosition;
    private Game _game;
    private int _health;
    private int _maxHealth;
    public Action<int> OnHealthChanged;
    public int Health => _health;
    
    public void Init(Game game)
    {
        _maxHealth = game.Database.settings.playerHealth;
        _health = _maxHealth;
        _game = game;
    }
    
    public Transform GetLaunchPosition()
    {
        return launchPosition;
    }

    public Transform GetHead()
    {
        return headCamera.transform;
    }

    public void Heal()
    {
        ChangeHealth(1);
    }

    public void Damage()
    {
        ChangeHealth(-1);
    }

    private void ChangeHealth(int change)
    {
        if (change == 0) return;
        _health += change;
        if (_health > _maxHealth)
            _health = _maxHealth;
        OnHealthChanged?.Invoke(_health);
        if (_health <= 0)
        {
            _game.EndGame();
        }
    }
}

