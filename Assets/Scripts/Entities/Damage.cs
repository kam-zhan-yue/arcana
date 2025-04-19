using System;

[Serializable]
public class Damage
{
    private float _amount;

    public float Amount => _amount;

    public Damage(float amount)
    {
        _amount = amount;
    }
}