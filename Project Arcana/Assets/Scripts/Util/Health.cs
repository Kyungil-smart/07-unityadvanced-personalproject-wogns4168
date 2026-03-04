using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public bool isDead => currentHealth <= 0f;

    public StatusManager StatusManager { get; private set; }

    public event Action<float, float> OnHealthChanged;

    public virtual void Awake()
    {
        if (currentHealth <= 0)
            currentHealth = maxHealth;
    
        StatusManager = new StatusManager(this);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public virtual void TakeDamage(float damage)
    {
        // Break 상태이상이면 받는 피해 증가
        int breakStack = StatusManager.GetStack("Break");
        if (breakStack > 0)
            damage *= BreakStatus.DamageMultiplier;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);
        if (currentHealth <= 0) Die();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
    
    protected void InvokeHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}