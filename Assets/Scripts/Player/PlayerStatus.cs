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
    [SerializeField] private int[] healthLevels;
    [SerializeField] private int healthLevel;

    [Header("Cosmetic effects")]
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private Sprite[] bodySprites;

    private int health;

    private bool isPlayerDead = false;

    public delegate void PlayerStatusEvent();
    public PlayerStatusEvent OnPlayerDeath;

    private void Start()
    {
        HealthLevelInitialize();
        OnPlayerDeath += PlayerDead;
    }
    private void HealthLevelInitialize()
    {
        healthLevel = SaveSystem.LoadData(SaveSystem.Type.healthLevel);
        maxHealth = healthLevels[healthLevel];
        health = maxHealth;
        bodySprite.sprite = bodySprites[healthLevel];
    }
    private void OnDisable()
    {
        OnPlayerDeath -= PlayerDead;
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
        if (transform.position.y < fallCheckBorder && !isPlayerDead)
        {
            isPlayerDead = true;
            OnPlayerDeath?.Invoke();
        }
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
