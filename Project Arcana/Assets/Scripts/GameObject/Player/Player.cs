using System;
using UnityEngine;

public class Player : MonoBehaviour, ITargetable
{
    PlayerHealth _playerHealth;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerHealth.Init();
    }

    public void TakeDamage(float damage) => _playerHealth.TakeDamage(damage);
    public void Heal(float heal) => _playerHealth.Heal(heal);
    public void Die() => _playerHealth.Die();
    
    public void OnHoverEnter()
    {
    }

    public void OnHoverExit()
    {
    }

    public void OnSelect()
    {
    }
}
