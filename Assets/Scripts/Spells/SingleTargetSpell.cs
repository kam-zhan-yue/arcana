using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public abstract class SingleTargetSpell : Spell
{
    protected abstract bool CanAffect(Enemy enemy);
    protected override List<Enemy> GetTargets()
    {
        Enemy currentTarget = GetCurrentTarget();
        if (currentTarget && CanAffect(currentTarget))
            return new List<Enemy> { currentTarget };
        return new();
    }
    
    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        TypeSetting typeSetting = settings.GetSettingForType(type);
        
        for (int i = 0; i < enemies.Count; ++i)
        {
            if(Burn.CanAffect(enemies[i]))
                enemies[i].SetOutline(typeSetting.colour, settings.outlineSize);
        }

        Enemy targetedEnemy = GetCurrentTarget();
        if (targetedEnemy && Burn.CanAffect(targetedEnemy))
        {
            targetedEnemy.SetOutline(settings.selectColour, settings.outlineSize);
        }
    }

    protected override void OnStopInteracting()
    {
        base.OnStopInteracting();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].DisableOutline();
        }
    }
}
