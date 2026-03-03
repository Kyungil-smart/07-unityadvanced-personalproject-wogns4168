using System.Collections.Generic;

public class BattleModel
{
    public List<MonsterBase> Monsters { get; private set; }
    public Deck Deck { get; private set; }
    public List<CardData> CurrentHand => Deck.hand;
    public int MaxEnergy { get; private set; }
    public int CurrentEnergy { get; private set; }

    private Player _player;

    public BattleModel(List<MonsterBase> monsters, Deck deck, Player player)
    {
        Monsters = monsters;
        Deck = deck;
        _player = player;

        MaxEnergy = RunManager.Instance != null ? RunManager.Instance.baseMaxEnergy : 3;
        CurrentEnergy = MaxEnergy;
    }

    public void DrawCard() => Deck.Draw();
    public void UseCard(CardData card) => Deck.UseCard(card);

    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
            DrawCard();
    }

    public void DiscardHand()
    {
        var handCopy = new List<CardData>(Deck.hand);
        foreach (var card in handCopy)
            Deck.UseCard(card);
    }

    public void RefillEnergy() => CurrentEnergy = MaxEnergy;

    public bool UseEnergy(int amount)
    {
        if (CurrentEnergy < amount) return false;
        CurrentEnergy -= amount;
        return true;
    }

    // 전투 종료 체크
    public BattleResult CheckBattleResult()
    {
        // 플레이어 사망
        if (_player != null && _player.isDead)
            return BattleResult.Defeat;

        // 모든 몬스터 사망
        bool allDead = true;
        foreach (var monster in Monsters)
        {
            if (monster != null && !monster.isDead)
            {
                allDead = false;
                break;
            }
        }

        if (allDead) return BattleResult.Victory;

        return BattleResult.None;
    }
}