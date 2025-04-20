using Kuroneko.UtilityDelivery;

public interface IGameManager : IGameService
{
    public GameDatabase GetGameDatabase();
    public Game GetGame();
}