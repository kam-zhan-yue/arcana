using Kuroneko.UtilityDelivery;

public interface ISpellService : IGameService
{
    public void Fireball(ISpellTarget target);
    public void Freeze(ISpellTarget target);
}