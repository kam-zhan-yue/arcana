using System;

[Serializable]
public enum DamageType
{
    Basic,
    Fire,
    Electric,
    Ice,
    Water,
    Air,
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
    private float _knockbackForce;
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
    public float KnockbackForce => _knockbackForce;

    public Damage(float baseAmount, DamageType type, DamageEffect effect, float knockbackForce = 0f)
    {
        _baseAmount = baseAmount;
        _type = type;
        _effect = effect;
        _knockbackForce = knockbackForce;
    }
}