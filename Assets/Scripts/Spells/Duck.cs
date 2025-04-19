using UnityEngine;

public class Duck : SingleTargetSpell
{
    protected override void Apply(Enemy spellTarget)
    {
        Debug.Log("Make into duck.");
    }

    protected override bool CanAffect(Enemy enemy)
    {
        return true;
    }
}
