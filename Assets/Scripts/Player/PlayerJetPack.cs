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
    [SerializeField] private SpriteRenderer[] legs;
    [SerializeField] private Sprite[] legsSprites;

    [SerializeField] private JetPackLevel[] levels;
    [SerializeField] private int jetPackLevel;

    public bool isFlyInput { get; set; }

    private Rigidbody2D rb;
    private int fuel;
    private bool isCanFly = true;
    private void Start()
    {
        JetPackLevelInitialize();
        fuel = fuelMax;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(FuelRecharge());
    }

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
    public delegate void JetPackFlyEvent();
    public static JetPackFlyEvent OnJetPackFly;

    private void FixedUpdate()
    {
        JetPackFly();
    }
    private void JetPackFly()
    {
        if (isFlyInput && isCanFly)
        {

            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
            
                OnJetPackFly?.Invoke();
                FuelChange(-fuelSpend);
                //rb.AddForce(new Vector2(0, jetPackForce));
                rb.velocity = new Vector2(rb.velocity.x, jetPackForce);
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
public class JetPackLevel
{
    [SerializeField] public float Force;
    [SerializeField] public int Spending;
    [SerializeField] public int MaxFuel;
    [SerializeField] public int Recharge;
}
