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

    protected override void Hover()
    {
        throw new System.NotImplementedException();
    }

    protected override void UnHover()
    {
        throw new System.NotImplementedException();
    }
}
