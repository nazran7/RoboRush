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
    [SerializeField] private Transform flyDirection;

    [Header("Cosmetic effects")]
    [SerializeField] private GameObject fuelBar;
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform particlesPoint;
    [SerializeField] private SpriteRenderer arm;
    [SerializeField] private Sprite[] armSprites;

    [SerializeField] private BoostLevel[] levels;
    [SerializeField] private int boostLevel;

    public bool isFlyInput { get; set; }
    private Rigidbody2D rb;
    private int fuel;
    private bool isFuelEnd = false;
    private bool isCanFly = true;
    private void Start()
    {
        BoostLevelInitialize();
        fuel = fuelMax;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FuelRecharge());
    }
    private void BoostLevelInitialize()
    {
        boostLevel = SaveSystem.LoadData(SaveSystem.Type.boostLevel);
        arm.sprite = armSprites[boostLevel];
        fuelMax = levels[boostLevel].MaxFuel;
        fuelSpend = levels[boostLevel].Spending;
        boostForce = levels[boostLevel].Force;
        fuelRecharge = levels[boostLevel].Recharge;
    }
    public delegate void BoostFlyEvent();
    public static BoostFlyEvent OnBoostFly;
    private void FixedUpdate()
    {
        BoostFly();
    }
    private void BoostFly()
    {

        if (isCanFly && isFlyInput)
        {
            OnBoostFly?.Invoke();
            FuelChange(-fuelSpend);

            Vector2 dir = new Vector2(transform.position.x - flyDirection.position.x,
                transform.position.y - flyDirection.position.y);
            rb.AddForce(-dir * boostForce);
            Destroy(Instantiate(particles, particlesPoint), 0.5f);
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
    [SerializeField] public float Force;
    [SerializeField] public int Spending;
    [SerializeField] public int MaxFuel;
    [SerializeField] public int Recharge;
}
