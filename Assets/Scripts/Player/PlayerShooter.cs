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
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootingParticles;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform shootingDirection;
    [SerializeField] private float fireRate;

    private bool isCanShoot = true;

    private void Update()
    {
        if (Input.GetKeyDown(shootingKey))
        {
            Shoot();
        }
    }
    public delegate void ShootEvent();
    public ShootEvent OnPlayerShoot;
    private void Shoot()
    {
        if (isCanShoot)
        {
            GameObject projectileGO = Instantiate(projectile, shootingPoint.position, Quaternion.identity);
            Destroy(Instantiate(shootingParticles, shootingPoint), 1f);
            Vector2 dir = new Vector2(shootingDirection.position.x - transform.position.x
                , shootingDirection.position.y - transform.position.y);
            projectileGO.GetComponent<Rigidbody2D>().AddForce(dir * projectileSpeed);
            //temporary
            //temporary
            //temporary
            //temporary
            projectileGO.GetComponent<Projectile>().damage = 1;
            //temporary
            //temporary
            //temporary
            //temporary
            Destroy(projectileGO, 2f);
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
