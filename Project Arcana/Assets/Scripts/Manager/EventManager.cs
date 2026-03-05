using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    
    [SerializeField] private List<Sprite> eventSprites;

    private List<EventData> _events;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        InitEvents();
    }

private void InitEvents()
{
    _events = new List<EventData>
    {
        new EventData
        {
            title = "신비한 샘",
            description = "맑은 물이 흐르는 샘을 발견했다. 마시면 기운이 날 것 같지만...",
            spriteIndex = 0,
            choice1 = new EventChoice { choiceText = "물을 마신다 (HP 30% 회복)", effectType = EventEffectType.HealHp, value = 0.3f },
            choice2 = new EventChoice { choiceText = "샘 주변을 뒤진다 (HP 15 잃고 카드 획득)", effectType = EventEffectType.LoseHpAndGainCard, value = 15f }
        },
        new EventData
        {
            title = "버려진 무기고",
            description = "오래된 무기고가 있다. 뭔가 쓸만한 게 있을지도 모른다.",
            spriteIndex = 1,
            choice1 = new EventChoice { choiceText = "무기를 챙긴다 (HP 10 잃고 카드 획득)", effectType = EventEffectType.LoseHpAndGainCard, value = 10f },
            choice2 = new EventChoice { choiceText = "팔만한 걸 챙긴다 (골드 40 획득)", effectType = EventEffectType.GainGold, value = 40f }
        },
        new EventData
        {
            title = "악마의 거래",
            description = "어둠 속에서 악마가 거래를 제안한다.",
            spriteIndex = 2,
            choice1 = new EventChoice { choiceText = "계약한다 (HP 절반 잃고 카드 2장 획득)", effectType = EventEffectType.LoseHalfHpAndGainCards, value = 0.5f },
            choice2 = new EventChoice { choiceText = "거절한다 (HP 20 잃고 골드 30 획득)", effectType = EventEffectType.LoseHpAndGainGold, value = 20f }
        },
        new EventData
        {
            title = "낡은 도서관",
            description = "먼지 쌓인 도서관에서 마법 서적을 발견했다.",
            spriteIndex = 3,
            choice1 = new EventChoice { choiceText = "서적을 구입한다 (골드 60 잃고 카드 2장 획득)", effectType = EventEffectType.LoseGoldAndGainCards, value = 60f },
            choice2 = new EventChoice { choiceText = "한 권만 슬쩍한다 (카드 1장 획득)", effectType = EventEffectType.GainCard, value = 1f }
        },
        new EventData
        {
            title = "수상한 상인",
            description = "수상한 차림의 상인이 특별한 물건을 팔겠다고 한다.",
            spriteIndex = 4,
            choice1 = new EventChoice { choiceText = "구입한다 (골드 80 잃고 카드 2장 획득)", effectType = EventEffectType.LoseGoldAndGainCards, value = 80f },
            choice2 = new EventChoice { choiceText = "거절한다 (골드 20 획득)", effectType = EventEffectType.GainGold, value = 20f }
        },
        
        new EventData
        {
            title = "신비한 오브",
            description = "빛나는 오브가 당신의 힘을 시험합니다.",
            spriteIndex = 5, // 새 스프라이트 추가하거나 기존 인덱스 사용
            choice1 = new EventChoice
            {
                choiceText = "피를 바친다 (HP 20 잃고 최대 에너지 +1)",
                effectType = EventEffectType.LoseHpAndGainEnergy,
                value = 20
            },
            choice2 = new EventChoice
            {
                choiceText = "골드를 바친다 (골드 80 잃고 최대 에너지 +1)",
                effectType = EventEffectType.LoseGoldAndGainEnergy,
                value = 80
            }
        },
        
        new EventData
        {
            title = "신비한 오브",
            description = "빛나는 오브가 당신의 힘을 시험합니다.",
            spriteIndex = 5, // 새 스프라이트 추가하거나 기존 인덱스 사용
            choice1 = new EventChoice
            {
                choiceText = "피를 바친다 (HP 20 잃고 최대 에너지 +1)",
                effectType = EventEffectType.LoseHpAndGainEnergy,
                value = 20
            },
            choice2 = new EventChoice
            {
                choiceText = "골드를 바친다 (골드 80 잃고 최대 에너지 +1)",
                effectType = EventEffectType.LoseGoldAndGainEnergy,
                value = 80
            }
        }
        
        
    };
}

    public EventData GetRandomEvent()
    {
        return _events[Random.Range(0, _events.Count)];
    }
    
    public Sprite GetSprite(int index)
    {
        if (index >= 0 && index < eventSprites.Count)
            return eventSprites[index];
        return null;
    }
}