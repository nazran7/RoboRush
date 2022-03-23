using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    [SerializeField] private KeyCode boostKey;
    [SerializeField] private float boostForce;
    [SerializeField] private int fuelMax;
    [SerializeField] private int fuelSpend;
    [SerializeField] private int fuelRecharge;

    [SerializeField] private GameObject fuelBar;
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform particlesPoint;
    [SerializeField] private Transform flyDirection;

    [SerializeField] private BoostLevel[] levels;


    private Rigidbody2D rb;
    private int fuel;
    private bool isFuelEnd = false;
    private bool isCanFly = true;
    private void Start()
    {
        fuel = fuelMax;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(KeyPressDelay());
        StartCoroutine(FuelRecharge());
    }

    private IEnumerator KeyPressDelay()
    {
        while (true)
        {

            //if (Input.GetKeyDown(jetPackKey) && isCanFly && !PlayerMover.singleton.IsGrounded)
            //{

            //    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            //}

            if (Input.GetKey(boostKey) && isCanFly)
            {
                FuelChange(-fuelSpend);

                Vector2 dir = new Vector2(transform.position.x - flyDirection.position.x,
                    transform.position.y - flyDirection.position.y);
                rb.AddForce(-dir * boostForce);
                Destroy(Instantiate(particles, particlesPoint), 0.5f);
                yield return new WaitForFixedUpdate();
            }
            else
                yield return null;
        }
    }

    private IEnumerator FuelRecharge()
    {
        while (true)
        {
            FuelChange(fuelRecharge);
            yield return new WaitForFixedUpdate();
        }
    }

    private void FuelChange(int count)
    {
        fuel += count;

        fuelBar.SetActive(true);
        if (fuel <= 0)
        {
            isCanFly = false;
        }
        else if (fuel > fuelMax / 5)
        {
            isCanFly = true;
        }
        if (fuel >= fuelMax)
        {
            fuel = fuelMax;
            fuelBar.SetActive(false);
        }

        float fuelScaleY = ((float)fuel) / ((float)fuelMax);

        fuelBar.transform.localScale = new Vector3(fuelBar.transform.localScale.x, fuelScaleY,
            fuelBar.transform.localScale.z);
    }
}
[System.Serializable]
public class BoostLevel
{
    [SerializeField] private float fuelForce;
    [SerializeField] private int fuelSpending;
    [SerializeField] private int fuelMax;
}
