using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    //enemy fields
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

    //bool for check player on trigger zone
    private bool isPlayerOnZone = false;
    //moving coroutine
    private IEnumerator movingCoroutine;
    //shooting loop coroutine
    private IEnumerator shootingCoroutine;
    //start moving position
    private Vector3 startPos;
    //end position
    private Vector3 endPos;
    private void Start()
    {
        startPos = transform.position;
        endPos = transform.position + movingPoint;
        StartMoving();
    }
    #region moving
    //start moving coroutine
    private void StartMoving()
    {
        movingCoroutine = MoveTo();
        if (shootingCoroutine != null)
            StopCoroutine(shootingCoroutine);
        StartCoroutine(movingCoroutine);
    }
    //moving coroutine
    private IEnumerator MoveTo()
    {
        while (true)
        {
            //moving to end position
            Rotation(endPos.x);
            while (transform.position != endPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, speed / 100);
                yield return new WaitForFixedUpdate();
            }
            //moving to start position
            Rotation(startPos.x);
            while (transform.position != startPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, speed / 100);
                yield return new WaitForFixedUpdate();
            }

        }
    }
    //turn in the direction of moving
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

    //gizmos to draw way of moving
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + movingPoint);
    }
    #endregion
    //finding player on trigger zone and start shooting
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

    //shooting loop coroutine
    private IEnumerator ShootingLoop(GameObject target)
    {
        while (isPlayerOnZone)
        {
            //look on player
            Rotation(target.transform.position.x);
            yield return new WaitForSeconds(shootingRate);
            //finding the shooting direction
            Vector2 shootingDir = new Vector2(target.transform.position.x - transform.position.x
                , target.transform.position.y - transform.position.y).normalized;
            //spawn projectile
            GameObject projectileGO = Instantiate(projectile,
                shootingPoint.transform.position, Quaternion.identity);
            //spawn shooting particles
            Destroy(Instantiate(shootingParticles,
                shootingPoint.transform.position, Quaternion.identity), 1f);
            //add force to projectile
            projectileGO.GetComponent<Rigidbody2D>().AddForce(shootingDir * projectileSpeed);
            projectileGO.GetComponent<DroneProjectile>().Damage = projectileDamege;
            //destroy projectile after 2 sec
            Destroy(projectileGO, 2f);
        }
    }
    //stop moving coroutine and start shooting coroutine
    private void StartShooting(GameObject target)
    {
        shootingCoroutine = ShootingLoop(target);
        if (movingCoroutine != null)
            StopCoroutine(movingCoroutine);
        StartCoroutine(shootingCoroutine);
    }
    //event on enemy kill
    public delegate void EnemyDestroyEvent();
    public static EnemyDestroyEvent OnEnemyDestroy;
    //take damage method
    public void TakeDamage(int damage)
    {
        health -= damage;
        //if health <= 0 - enemy died
        if (health <= 0)
        {
            health = 0;
            OnEnemyDestroy?.Invoke();
            Destroy(Instantiate(explosionParticles, transform), 3f);
            Destroy(gameObject);
        }
        else
        {
            //damage indication
            StartCoroutine(DamageIndication());
        }
    }
    //color switch on take damage
    private IEnumerator DamageIndication()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = new Color(255, 0, 0, 255);

        yield return new WaitForSeconds(0.3f);

        sr.color = new Color(255, 255, 255, 255);
    }
}
