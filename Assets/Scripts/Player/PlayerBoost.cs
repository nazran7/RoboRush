using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoost : MonoBehaviour
{
    //boost fields
    [SerializeField] private float boostForce;
    [SerializeField] private int fuelMax;
    [SerializeField] private int fuelSpend;
    [SerializeField] private int fuelRecharge;
    [SerializeField] private Transform flyDirection;
    //cosmetic effects of boost
    [Header("Cosmetic effects")]
    [SerializeField] private GameObject fuelBar;
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform particlesPoint;
    [SerializeField] private SpriteRenderer arm;
    [SerializeField] private Sprite[] armSprites;
    //levels of boost
    [SerializeField] private BoostLevel[] levels;
    [SerializeField] private int boostLevel;
    //check fly input
    public bool isFlyInput { get; set; }
    //rigidbody component
    private Rigidbody2D rb;
    //current fuel level
    private int fuel;
    //can fly on/off
    private bool isCanFly = true;

    private void Start()
    {
        //boost initialize
        BoostLevelInitialize();
        fuel = fuelMax;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FuelRecharge());
    }
    //boost level initialize
    private void BoostLevelInitialize()
    {
        boostLevel = SaveSystem.LoadData(SaveSystem.Type.boostLevel);
        arm.sprite = armSprites[boostLevel];
        fuelMax = levels[boostLevel].MaxFuel;
        fuelSpend = levels[boostLevel].Spending;
        boostForce = levels[boostLevel].Force;
        fuelRecharge = levels[boostLevel].Recharge;
    }
    //boost fly event
    public delegate void BoostFlyEvent();
    public static BoostFlyEvent OnBoostFly;
    private void FixedUpdate()
    {
        BoostFly();
    }
    //boost fly method
    private void BoostFly()
    {
        //if can fly bool is true and fly input pressed - boost use
        if (isCanFly && isFlyInput)
        {
            OnBoostFly?.Invoke();
            //fuel spend
            FuelChange(-fuelSpend);
            //add horizontal force
            Vector2 dir = new Vector2(transform.position.x - flyDirection.position.x,
                transform.position.y - flyDirection.position.y);
            rb.AddForce(-dir * boostForce);
            //spawn particles
            Destroy(Instantiate(particles, particlesPoint), 0.5f);
        }
    }
    //fuel recharge coroutine
    private IEnumerator FuelRecharge()
    {
        while (true)
        {
            FuelChange(fuelRecharge);
            yield return new WaitForFixedUpdate();
        }
    }
    //fuel count change method
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
    //boost level fields
    [SerializeField] public float Force;
    [SerializeField] public int Spending;
    [SerializeField] public int MaxFuel;
    [SerializeField] public int Recharge;
}
