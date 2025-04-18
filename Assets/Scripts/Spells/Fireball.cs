using UnityEngine;

public class Fireball : Spell
{
    protected override void Apply(ISpellTarget target)
    {
        Debug.Log("FIREBALLING");
    }
}
