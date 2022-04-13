using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    #region singleton
    public static PlayerShooter singleton { get; private set; }
    private void Awake()
    {
        singleton = this;
    }
    #endregion
    [SerializeField] private KeyCode shootingKey;
    [SerializeField] private GameObject zapPrefab;
    [SerializeField] private float zapDistance;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform shootingDirection;
    [SerializeField] private float fireRate;
    [SerializeField] private int damage;
    [SerializeField] private WeaponLevel[] levels;
    [SerializeField] private int weaponLevel;
    [Header("Cosmetic effects")]
    [SerializeField] private GameObject shootingParticles;
    [SerializeField] private SpriteRenderer rightArmSprite;
    [SerializeField] private Sprite[] armSprites;

    private bool isCanShoot = true;
    private void Start()
    {
        WeaponLevelInitialize();
    }
    private void WeaponLevelInitialize()
    {
        weaponLevel = SaveSystem.LoadData(SaveSystem.Type.weaponLevel);
        rightArmSprite.sprite = armSprites[weaponLevel];
        damage = levels[weaponLevel].Damage;
        fireRate = levels[weaponLevel].FireRate;
    }
    private void Update()
    {
        if (Input.GetKeyDown(shootingKey))
        {
            Shoot();
        }
    }
    public delegate void ShootEvent();
    public ShootEvent OnPlayerShoot;
    public void Shoot()
    {
        if (isCanShoot)
        {
            Destroy(Instantiate(zapPrefab, shootingPoint), 0.5f);

            RaycastHit2D hit = Physics2D.Raycast(shootingPoint.transform.position,
                shootingDirection.transform.position, zapDistance);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<Drone>() != null)
                {
                    hit.collider.gameObject.GetComponent<Drone>().TakeDamage(damage);
                }
            }

            Destroy(Instantiate(shootingParticles, shootingPoint), 1f);
            StartCoroutine(ShootRecharge());
            OnPlayerShoot?.Invoke();
        }
    }
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
    [SerializeField] public float FireRate;
    [SerializeField] public int Damage;
}
