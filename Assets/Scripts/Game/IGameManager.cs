using Kuroneko.UtilityDelivery;

public interface IGameManager : IGameService
{
    public Game GetGame();
    public void StartGame();
}