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

    private bool isPlayerOnZone = false;
    private IEnumerator movingCoroutine;
    private IEnumerator shootingCoroutine;
    private Vector3 startPos;
    private Vector3 endPos;
    private void Start()
    {
        startPos = transform.position;
        endPos = transform.position + movingPoint;
        StartMoving();
    }
    #region moving
    private void StartMoving()
    {
        movingCoroutine = MoveTo();
        if (shootingCoroutine != null)
            StopCoroutine(shootingCoroutine);
        StartCoroutine(movingCoroutine);
    }
    private IEnumerator MoveTo()
    {
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
            isPlayerOnZone = true;
            StartShooting(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStatus>() != null)
        {
            isPlayerOnZone = false;
            StartMoving();
        }
    }
    private IEnumerator ShootingLoop(GameObject target)
    {
        while (isPlayerOnZone)
        {
            Rotation(target.transform.position.x);
            yield return new WaitForSeconds(shootingRate);
            Vector2 shootingDir = new Vector2(target.transform.position.x - transform.position.x
                , target.transform.position.y - transform.position.y).normalized;
            GameObject projectileGO = Instantiate(projectile,
                shootingPoint.transform.position, Quaternion.identity);
            Destroy(Instantiate(shootingParticles,
                shootingPoint.transform.position, Quaternion.identity), 1f);
            projectileGO.GetComponent<Rigidbody2D>().AddForce(shootingDir * projectileSpeed);
            projectileGO.GetComponent<DroneProjectile>().Damage = projectileDamege;
            Destroy(projectileGO, 2f);
        }
    }
    private bool VisibleCheck(GameObject target)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position);
        print(hit.collider.gameObject);
        if (hit.collider.gameObject.GetComponent<PlayerStatus>() != null)
            return true;
        else
            return false;
    }
    private void StartShooting(GameObject target)
    {
        shootingCoroutine = ShootingLoop(target);
        if (movingCoroutine != null)
            StopCoroutine(movingCoroutine);
        StartCoroutine(shootingCoroutine);
    }

    public delegate void EnemyDestroyEvent();
    public static EnemyDestroyEvent OnEnemyDestroy;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            OnEnemyDestroy?.Invoke();
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
