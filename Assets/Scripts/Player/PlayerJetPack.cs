using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJetPack : MonoBehaviour
{
    [SerializeField] private KeyCode jetPackKey;
    [SerializeField] private float jetPackForce;
    [SerializeField] private int fuelMax;
    [SerializeField] private int fuelSpend;
    [SerializeField] private int fuelRecharge;

    [Header("Cosmetic effects")]
    [SerializeField] private GameObject fuelBar;
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform particlesPoint;

    [SerializeField] private JetPackLevel[] levels;

    private Rigidbody2D rb;
    private int fuel;
    private bool isCanFly = true;
    private void Start()
    {
        fuel = fuelMax;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(JetPackFly());
        StartCoroutine(FuelRecharge());
    }

    private IEnumerator JetPackFly()
    {
        while (true)
        {

            if (Input.GetKeyDown(jetPackKey) && isCanFly )
            {

                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
                while (Input.GetKey(jetPackKey) && isCanFly )
                {
                    FuelChange(-fuelSpend);
                    //rb.AddForce(new Vector2(0, jetPackForce));
                    rb.velocity = new Vector2(rb.velocity.x, jetPackForce);
                    Destroy(Instantiate(particles, particlesPoint), 0.5f);
                    yield return new WaitForFixedUpdate();
                }
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
public class JetPackLevel
{
    [SerializeField] private float fuelForce;
    [SerializeField] private int fuelSpending;
    [SerializeField] private int fuelMax;


    public JetPackLevel(float force, int spending, int maxFuel)
    {
        fuelForce = force;
        fuelSpending = spending;
        fuelMax = maxFuel;
    }
}
