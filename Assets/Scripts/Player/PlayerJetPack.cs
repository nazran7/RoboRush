using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJetPack : MonoBehaviour
{
    //jet fields
    [SerializeField] private float jetPackForce;
    [SerializeField] private int fuelMax;
    [SerializeField] private int fuelSpend;
    [SerializeField] private int fuelRecharge;

    //cosmetic effects of jet
    [Header("Cosmetic effects")]
    [SerializeField] private GameObject fuelBar;
    [SerializeField] private GameObject particles;
    [SerializeField] private Transform particlesPoint;
    [SerializeField] private SpriteRenderer[] legs;
    [SerializeField] private Sprite[] legsSprites;
    //levels of jet
    [SerializeField] private JetPackLevel[] levels;
    [SerializeField] private int jetPackLevel;
    // jet input check
    public bool isFlyInput { get; set; }
    //rigidbody of player
    private Rigidbody2D rb;
    //current count of fuel
    private int fuel;
    //can fly onn/off
    private bool isCanFly = true;
    private void Start()
    {
        //jet initialization
        JetPackLevelInitialize();
        fuel = fuelMax;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FuelRecharge());
    }
    //jet level load
    private void JetPackLevelInitialize()
    {
        jetPackLevel = SaveSystem.LoadData(SaveSystem.Type.jetPackLevel);
        foreach (SpriteRenderer sr in legs)
        {
            sr.sprite = legsSprites[jetPackLevel];
        }
        fuelMax = levels[jetPackLevel].MaxFuel;
        fuelSpend = levels[jetPackLevel].Spending;
        jetPackForce = levels[jetPackLevel].Force;
        fuelRecharge = levels[jetPackLevel].Recharge;
    }
    //jet fly event
    public delegate void JetPackFlyEvent();
    public static JetPackFlyEvent OnJetPackFly;

    private void FixedUpdate()
    {
        JetPackFly();
    }
    //jet fly method
    private void JetPackFly()
    {
        //if can  fly = true and input pressed - jetpack use
        if (isFlyInput && isCanFly)
        {
            //velosity update
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            
                OnJetPackFly?.Invoke();
            //fuel spend
                FuelChange(-fuelSpend);
                rb.velocity = new Vector2(rb.velocity.x, jetPackForce);
            //spawn particles
                Destroy(Instantiate(particles, particlesPoint), 0.5f);
        }
    }
    //fuel recharge method
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
        //fuel bar ui update
        float fuelScaleY = ((float)fuel) / ((float)fuelMax);

        fuelBar.transform.localScale = new Vector3(fuelBar.transform.localScale.x, fuelScaleY,
            fuelBar.transform.localScale.z);
    }
}
[System.Serializable]
public class JetPackLevel
{
    //jet levels fields
    [SerializeField] public float Force;
    [SerializeField] public int Spending;
    [SerializeField] public int MaxFuel;
    [SerializeField] public int Recharge;
}
