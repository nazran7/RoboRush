using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    //singleton pattern
    #region singleton
    public static PlayerShooter singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    //shooting fields
    [SerializeField] private GameObject zapPrefab;
    [SerializeField] private float zapDistance;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform shootingDirection;
    [SerializeField] private float fireRate;
    [SerializeField] private int damage;
    //levels of weapon
    [SerializeField] private WeaponLevel[] levels;
    [SerializeField] private int weaponLevel;
    //shooting cosmetic effects
    [Header("Cosmetic effects")]
    [SerializeField] private GameObject shootingParticles;
    [SerializeField] private SpriteRenderer rightArmSprite;
    [SerializeField] private Sprite[] armSprites;
    //is can shoot on/off
    private bool isCanShoot = true;
    private void Start()
    {
        WeaponLevelInitialize();
    }
    //weapon level initialization
    private void WeaponLevelInitialize()
    {
        weaponLevel = SaveSystem.LoadData(SaveSystem.Type.weaponLevel);
        rightArmSprite.sprite = armSprites[weaponLevel];
        damage = levels[weaponLevel].Damage;
        fireRate = levels[weaponLevel].FireRate;
    }
    //player shoot event
    public delegate void ShootEvent();
    public ShootEvent OnPlayerShoot;
    //shoot method
    public void Shoot()
    {
        if (isCanShoot)
        {
            //spawn zap effect and destroy it after 0.5 sec
            Destroy(Instantiate(zapPrefab, shootingPoint), 0.5f);
            //hit check with ray
            RaycastHit2D hit = Physics2D.Raycast(shootingPoint.transform.position,
                shootingDirection.transform.position, zapDistance);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<Drone>() != null)
                {
                    hit.collider.gameObject.GetComponent<Drone>().TakeDamage(damage);
                }
            }
            //spawn particles
            Destroy(Instantiate(shootingParticles, shootingPoint), 1f);
            //recharge shoot
            StartCoroutine(ShootRecharge());
            OnPlayerShoot?.Invoke();
        }
    }
    //shoot recharge coroutine
    private IEnumerator ShootRecharge()
    {
        isCanShoot = false;
        yield return new WaitForSeconds(fireRate);
        isCanShoot = true;
    }
}

[System.Serializable]
public class WeaponLevel
{
    //weapon level fields
    [SerializeField] public float FireRate;
    [SerializeField] public int Damage;
}
