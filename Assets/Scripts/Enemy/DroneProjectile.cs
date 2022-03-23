using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneProjectile : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    public int Damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Destroy(Instantiate(particles, transform.position, Quaternion.identity), 1f);

    }
}
