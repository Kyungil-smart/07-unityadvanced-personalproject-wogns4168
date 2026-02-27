using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public bool isDead => currentHealth <= 0f;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);
        if (currentHealth <= 0) Die();
    }
    
    public void Heal(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
