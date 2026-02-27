using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public bool isDead => currentHealth <= 0f;
    
    public event Action<float, float> OnHealthChanged;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public virtual void TakeDamage(float damage)
    {
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
}
