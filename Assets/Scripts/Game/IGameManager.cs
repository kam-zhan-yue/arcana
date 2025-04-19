using System.Collections.Generic;
using Kuroneko.UtilityDelivery;

public interface IGameManager : IGameService
{
    public Player GetPlayer();
    public List<Enemy> GetActiveEnemies();
    public void AddActiveEnemy(Enemy enemy);
    public void RemoveActiveEnemy(Enemy enemy);
}