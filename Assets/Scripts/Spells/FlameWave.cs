using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public class FlameWave : Spell
{
    protected override List<Enemy> GetTargets()
    {
        if (!cardPopup.CanActivate)
            return new();
        
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        List<Enemy> targets = new();
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (Burn.CanAffect(enemies[i]))
                targets.Add(enemies[i]);
        }
        return targets;
    }

    protected override void Apply(Enemy enemy)
    {
        DamageEffect effect = DamageEffect.None;
        if (enemy.Status == Status.Frozen)
        {
            effect = DamageEffect.Melt;
        }
        Damage spellDamage = new(damage, DamageType.Fire, effect);
        
        
        enemy.Damage(spellDamage);
    }
}