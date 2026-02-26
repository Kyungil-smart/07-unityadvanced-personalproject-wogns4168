using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private float maxHealth;
    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(_currentHealth, 0f);
        if (_currentHealth <= 0) Die();
    }
    
    public virtual void Heal(float heal)
    {
        _currentHealth += heal;
        _currentHealth = Mathf.Min(maxHealth, _currentHealth);
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
