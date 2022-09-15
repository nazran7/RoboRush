using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    //singleton pattern
    #region singleton
    public static PlayerStatus singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    //health fields
    [SerializeField] public int maxHealth;
    [SerializeField] private float fallCheckBorder;
    [SerializeField] private int[] healthLevels;
    [SerializeField] private int healthLevel;
    //body sprites
    [Header("Cosmetic effects")]
    [SerializeField] private SpriteRenderer bodySprite;
    [SerializeField] private Sprite[] bodySprites;
    //current health
    private int health;
    //player dead true/false
    private bool isPlayerDead = false;
    //player death event
    public delegate void PlayerStatusEvent();
    public PlayerStatusEvent OnPlayerDeath;

    private void Start()
    {
        HealthLevelInitialize();
    }
    //health level initialize
    private void HealthLevelInitialize()
    {
        //load health level from save system
        healthLevel = SaveSystem.LoadData(SaveSystem.Type.healthLevel);
        maxHealth = healthLevels[healthLevel];
        health = maxHealth;
        bodySprite.sprite = bodySprites[healthLevel];
    }
    //take damage event
    public delegate void OnTakeDamageEvent(int currentHealth);
    public OnTakeDamageEvent OnTakeDamage;
    //take damage method 
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // if lose all health - player die
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
    //player fall check method
    private void PlayerFallCheck()
    {
        //if player croos fall check border - player die
        if (transform.position.y < fallCheckBorder && !isPlayerDead)
        {
            isPlayerDead = true;
            OnPlayerDeath?.Invoke();
        }
    }


    //player collision check with projectile
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<DroneProjectile>() != null)
        {
            TakeDamage(collision.gameObject.GetComponent<DroneProjectile>().Damage);
        }
    }
}
