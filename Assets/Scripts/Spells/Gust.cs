using System;
using System.Collections.Generic;
using System.Linq;
using Kuroneko.UtilityDelivery;
using UnityEngine;

public class Gust : Spell
{
    private static readonly Dictionary<Status, DamageType> Map = new Dictionary<Status, DamageType>
    {
        { Status.None, DamageType.Basic },
        { Status.Frozen, DamageType.Ice },
        { Status.Burned, DamageType.Fire },
        { Status.Wet, DamageType.Water },
    };
    protected override void InitConfig(SpellConfig config)
    {
        base.InitConfig(config);
        GustSpellConfig spellConfig = config as GustSpellConfig;
        if (spellConfig == null)
            throw new InvalidCastException("Config must be of type GustSpellConfig.");
    }

    protected override bool CanApply()
    {
        return GetActiveTargets().Count > 0 && cardPopup.CanActivate;
    }

    protected override void ApplySpell()
    {
        var targets = GetActiveTargets();
        foreach ((Enemy enemy, StatusEffect effect) in targets)
        {
            effect.Refresh();
            Debug.Log($"Applying {effect} to {enemy}");
            enemy.ApplyStatus(effect);
            
            // Deal 0 damage to apply the pulse effect
            Damage spellDamage = new Damage(0f, Map[effect.status], DamageEffect.None);
            enemy.Damage(spellDamage);
        }
        Use();
    }

    protected override void Use()
    {
        base.Use();
        AudioManager.instance.Play("SFX_PROPAGATE");
    }

    protected override void OnStartDragging()
    {
        base.OnStartDragging();
        cardPopup.EnableActivationZone();
    }

    private List<(Enemy enemy, StatusEffect effect)> GetActiveTargets()
    {
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetGame().Enemies;
        Queue<(Enemy enemy, StatusEffect statusEffect)> queue = new();
        HashSet<Enemy> visited = new();

        List<(Enemy enemy, StatusEffect effect)> targets = new();
        // Initialise the queue with enemies that have already have statuses
        foreach (Enemy enemy in enemies)
        {
            if (enemy.Status != Status.None)
            {
                queue.Enqueue((enemy, enemy.StatusEffect));

                StatusEffect clonedEffect = enemy.StatusEffect.Clone();
                targets.Add((enemy, clonedEffect));
                visited.Add(enemy);
            }
        }
        
        // Spread the status to the nearest unvisited enemy
        while (queue.Count > 0)
        {
            (Enemy enemy, StatusEffect effect) = queue.Dequeue();
            // Get the nearest unvisited enemy with no status
            Enemy nearest = enemies.Where(e => !visited.Contains(e) && e.Status == Status.None).OrderBy(e => Vector3.Distance(enemy.transform.position, e.transform.position)).FirstOrDefault();
            if (nearest != null)
            {
                StatusEffect clonedEffect = effect.Clone();
                visited.Add(nearest);
                targets.Add((nearest, clonedEffect));
                queue.Enqueue((nearest, effect));
            }
        }

        return targets;
    }

    protected override void OnInteracting()
    {
        base.OnInteracting();
        var targets = GetActiveTargets();
        foreach ((Enemy enemy, StatusEffect effect) in targets)
        {
            Color color = cardPopup.CanActivate
                ? settings.selectColour
                : settings.idleColour;
            enemy.SetOutline(color, settings.outlineSize);
        }
    }

    protected override void OnStopInteracting()
    {
        base.OnStopInteracting();
        cardPopup.DisableActivationZone();
        List<Enemy> enemies = ServiceLocator.Instance.Get<IGameManager>().GetGame().Enemies;
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].DisableOutline();
        }
    }

    // This is not used
    protected override List<Enemy> GetTargets()
    {
        throw new NotImplementedException();
    }

    protected override void Apply(Enemy spellTarget)
    {
        throw new NotImplementedException();
    }
}
