using System;

[Serializable]
public enum DamageType
{
    Basic,
    Fire,
    Electric,
    Ice,
    Water,
}

[Serializable]
public enum DamageEffect
{
    None,
    Melt,
    Electrocute,
}

[Serializable]
public class Damage
{
    private const float MELT_MULTIPLIER = 2.0f;
    private float _baseAmount;
    private DamageType _type;
    private DamageEffect _effect;

    public float Amount
    {
        get
        {
            float multiplier = 1.0f;
            if (_effect == DamageEffect.Melt)
                multiplier = MELT_MULTIPLIER;
            return _baseAmount * multiplier;
        }
    }

    public DamageType Type => _type;
    public DamageEffect Effect => _effect;


    public Damage(float baseAmount, DamageType type, DamageEffect effect)
    {
        _baseAmount = baseAmount;
        _type = type;
        _effect = effect;
    }
}