using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneProjectile : MonoBehaviour
{
    //particles on projectile collision
    [SerializeField] private GameObject particles;
    //damage of projectile
    public int Damage;
    //projectile collision check
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Destroy(Instantiate(particles, transform.position, Quaternion.identity), 1f);
    }
}
