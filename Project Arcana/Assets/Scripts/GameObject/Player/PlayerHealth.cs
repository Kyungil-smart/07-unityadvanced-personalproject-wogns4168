using UnityEngine;

public class PlayerHealth : Health
{
    readonly float _defaultShield = 0;
    private float _currentShield;
    private float _absShield;

    public override void Awake()
    {
        _currentShield = _defaultShield;
        base.Awake();
    }
    
    public void AddShield(float amount)
    {
        _currentShield += amount;
    }
    
    private float TakeDamageShield(float damage)
    {
        _currentShield -= damage;
        if (_currentShield <= 0)
        {
            _absShield = Mathf.Abs(_currentShield);
            _currentShield = _defaultShield;
            return _absShield;
        }
        return 0f;
    }

    public override void TakeDamage(float damage)
    {
        float remainingDamage = TakeDamageShield(damage); // 항상 호출
        if (remainingDamage > 0f)
            base.TakeDamage(remainingDamage);
    }

    public override void Die()
    {
        // 게임오버
    }
}