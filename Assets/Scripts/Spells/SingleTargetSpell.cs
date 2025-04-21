using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public abstract class SingleTargetSpell : Spell
{
    protected abstract bool CanAffect(Enemy enemy);
    
    protected override bool CanApply()
    {
        return GetTargets().Count > 0;
    }
    
    protected override List<Enemy> GetTargets()
    {
        Enemy currentTarget = GetCurrentTarget();
        if (currentTarget && currentTarget.IsVulnerable && CanAffect(currentTarget))
            return new List<Enemy> { currentTarget };
        return new();
    }
    
    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetGame().Enemies;
        for (int i = 0; i < enemies.Count; ++i)
        {
            if(CanAffect(enemies[i]) && enemies[i].IsVulnerable)
                enemies[i].SetOutline(settings.idleColour, settings.outlineSize);
        }

        Enemy targetedEnemy = GetCurrentTarget();
        if (targetedEnemy && CanAffect(targetedEnemy) && targetedEnemy.IsVulnerable)
        {
            targetedEnemy.SetOutline(settings.selectColour, settings.outlineSize);
        }
    }
}
