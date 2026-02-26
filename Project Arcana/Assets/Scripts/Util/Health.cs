using UnityEngine;
using UnityEngine.Serialization;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private float maxHealth;
    [SerializeField] 
    private float currentHealth;

    private void Awake()
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
