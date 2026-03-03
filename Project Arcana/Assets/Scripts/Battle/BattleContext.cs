using System.Collections.Generic;

public class BattleContext
{
    public Player Player { get; private set; }
    public List<MonsterBase> Monsters { get; private set; }
    public BattleModel Model { get; private set; }

    public BattleContext(Player player, List<MonsterBase> monsters, BattleModel model)
    {
        Player = player;
        Monsters = monsters;
        Model = model;
    }
}