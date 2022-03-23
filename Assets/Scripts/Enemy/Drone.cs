using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 movingPoint;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootingRate;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int projectileDamege;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject shootingParticles;
    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private int health;
    private void Start()
    {
        StartMoving();
    }
    #region moving
    private void StartMoving()
    {
        StopAllCoroutines();
        StartCoroutine(MoveTo());
    }
    private IEnumerator MoveTo()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + movingPoint;
        while (true)
        {
            Rotation(endPos.x);
            while (transform.position != endPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, speed / 100);
                yield return new WaitForFixedUpdate();
            }
            Rotation(startPos.x);
            while (transform.position != startPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, speed / 100);
                yield return new WaitForFixedUpdate();
            }

        }
    }

    private void Rotation(float x)
    {
        if ((transform.position.x - x) > 0)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, -0, 0));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + movingPoint);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStatus>() != null)
        {
            StartShooting(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStatus>() != null)
        {
            StartMoving();
        }
    }
    private IEnumerator ShootingLoop(GameObject target)
    {
        while (true)
        {
            Rotation(target.transform.position.x);
            yield return new WaitForSeconds(shootingRate);
            Vector2 shootingDir = new Vector2(target.transform.position.x - transform.position.x
                , target.transform.position.y - transform.position.y).normalized;
            GameObject projectileGO = Instantiate(projectile, shootingPoint);
            Destroy(Instantiate(shootingParticles, shootingPoint), 1f);
            projectileGO.GetComponent<Rigidbody2D>().AddForce(shootingDir * projectileSpeed);
            projectileGO.GetComponent<DroneProjectile>().Damage = projectileDamege;
            Destroy(projectileGO, 2f);
        }
    }
    private void StartShooting(GameObject target)
    {
        StopAllCoroutines();
        StartCoroutine(ShootingLoop(target));
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Destroy(Instantiate(explosionParticles, transform), 3f);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DamageIndication());
        }
    }
    private IEnumerator DamageIndication()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = new Color(255, 0, 0, 255);

        yield return new WaitForSeconds(0.3f);

        sr.color = new Color(255, 255, 255, 255);
    }
}
