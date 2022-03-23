using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    #region singleton
    public static PlayerStatus singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    [SerializeField] public int maxHealth;
    [SerializeField] private float fallCheckBorder;

    private int health;

    public delegate void PlayerStatusEvent();
    public PlayerStatusEvent OnPlayerDeath;

    private void Start()
    {
        health = maxHealth;
        OnPlayerDeath += PlayerDead;
    }
    private void OnDisable()
    {
        OnPlayerDeath += PlayerDead;
    }

    public delegate void OnTakeDamageEvent(int currentHealth);
    public OnTakeDamageEvent OnTakeDamage;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            OnPlayerDeath?.Invoke();
        }
        else
        {
            OnTakeDamage?.Invoke(health);
        }
    }

    private void Update()
    {
        PlayerFallCheck();
    }
    private void PlayerFallCheck()
    {
        if (transform.position.y < fallCheckBorder)
            OnPlayerDeath?.Invoke();
    }

    private void PlayerDead()
    {
        print("dead");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DroneProjectile>() != null)
        {
            TakeDamage(collision.gameObject.GetComponent<DroneProjectile>().Damage);
        }
    }
}
