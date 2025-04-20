using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public abstract class MultiTargetSpell : ActivationSpell
{
    protected override List<Enemy> GetTargets()
    {
        return !cardPopup.CanActivate ? new List<Enemy>() : GetFilteredTargets();
    }

    protected abstract bool CanAffect(Enemy enemy);
    
    protected override bool CanApply()
    {
        return base.CanApply() && GetTargets().Count > 0;
    }

    private List<Enemy> GetFilteredTargets()
    {
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetActiveEnemies();
        List<Enemy> targets = new();
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (CanAffect(enemies[i]) && enemies[i].IsVulnerable)
                targets.Add(enemies[i]);
        }
        return targets;
    }

    protected override void OnInteracting()
    {
        base.OnInteracting();
        List<Enemy> enemies = GetFilteredTargets();
        TypeSetting typeSetting = settings.GetSettingForType(type);
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].SetOutline(cardPopup.CanActivate ? settings.selectColour : typeSetting.colour, settings.outlineSize);
        }
    }
}
