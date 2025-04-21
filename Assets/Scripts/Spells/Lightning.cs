using System;
using UnityEngine;

public class Lightning : MultiTargetSpell
{
    private const float HEIGHT_OFFSET = 3f;
    private ParticleSystem _lightningEffect;
    private float _knockbackForce;
    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        LightningSpellConfig spellConfig = config as LightningSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type LightningSpellConfig.");
        _lightningEffect = spellConfig.lightingEffect;
        _knockbackForce = spellConfig.knockbackForce;
    }

    protected override void Apply(Enemy enemy)
    {
        DamageEffect effect = DamageEffect.None;
        if (enemy.Status == Status.Wet)
        {
            effect = DamageEffect.Electrocute;
        }
        Damage spellDamage = new (damage, DamageType.Electric, effect, _knockbackForce);
        enemy.Damage(spellDamage);
        SpawnParticles(enemy);
    }

    private void SpawnParticles(Enemy enemy)
    {
        ParticleSystem lightning = Instantiate(_lightningEffect);
        Vector3 spawnPos = enemy.GetCenter();
        spawnPos.y += HEIGHT_OFFSET;
        lightning.transform.position = spawnPos;
        lightning.Play();
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return enemy.Status == Status.Wet;
    }
}