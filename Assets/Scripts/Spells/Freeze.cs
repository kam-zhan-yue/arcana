using UnityEngine;

public class Freeze : Spell
{
    [SerializeField] private float freezeTime = 5f;
    
    protected override void Apply(ISpellTarget target)
    {
        if (target.GetTransform().TryGetComponent(out IFreezeTarget freezeTarget))
        {
            freezeTarget.Freeze(freezeTime);
        }
    }
}
