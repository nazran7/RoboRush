using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    [SerializeField] public int damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Drone>() != null)
        {
            collision.gameObject.GetComponent<Drone>().TakeDamage(damage);
        }
        Destroy(gameObject);
        Destroy(Instantiate(particles, transform.position, Quaternion.identity), 1f);

    }
}
