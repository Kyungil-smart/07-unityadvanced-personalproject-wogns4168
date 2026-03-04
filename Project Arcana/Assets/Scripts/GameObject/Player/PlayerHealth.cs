using System;
using UnityEngine;

public class PlayerHealth : Health
{
    private float _currentShield;
    public event Action<float> OnShieldChanged; // 이게 있어야 함

    public float CurrentShield => _currentShield;

    public void AddShield(float amount)
    {
        _currentShield += amount;
        OnShieldChanged?.Invoke(_currentShield);
    }

    private float TakeDamageShield(float damage)
    {
        if (_currentShield <= 0) return damage;

        _currentShield -= damage;
        if (_currentShield <= 0)
        {
            float remaining = Mathf.Abs(_currentShield);
            _currentShield = 0;
            OnShieldChanged?.Invoke(_currentShield);
            return remaining;
        }

        OnShieldChanged?.Invoke(_currentShield);
        return 0f;
    }

    public override void TakeDamage(float damage)
    {
        float remainingDamage = TakeDamageShield(damage);
        if (remainingDamage > 0f)
            base.TakeDamage(remainingDamage);
    }

    public void ResetShield()
    {
        _currentShield = 0;
        OnShieldChanged?.Invoke(_currentShield);
    }
    
    public void SetHealth(float current, float max)
    {
        maxHealth = max;
        currentHealth = current;
        InvokeHealthChanged();
    }
    
}