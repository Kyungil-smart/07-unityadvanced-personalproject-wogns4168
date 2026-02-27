using UnityEngine;

public class CardView : MonoBehaviour, IPoolable
{
    private CardData _cardData;

    public void Setup(CardData cardData)
    {
        _cardData = cardData;
    }
    
    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        _cardData = null;
        gameObject.SetActive(false);
    }

    public void UseCard()
    {
        if (_cardData == null) return;
        
        PoolManager.Instance.Despawn(gameObject, this.gameObject);
    }
}
