using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMachine : MonoBehaviour
{
    //broken machine fields
    [SerializeField] private bool _isBroken;
    [SerializeField] private int _coinsForRepair;
    [SerializeField] private float _timeToRepair;
    [SerializeField] private Sprite _brokenSprite;
    [SerializeField] private Sprite _repairedSprite;
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject _popUp;
    //sprite renderer component
    private SpriteRenderer sr;

    private void Start()
    {
        //spite initialization
        sr = GetComponent<SpriteRenderer>();
        if (_isBroken)
        {
            sr.sprite = _brokenSprite;
        }
        else
            sr.sprite = _repairedSprite;
    }
    //coins add for repair event
    public delegate void CoinAddEvent(int count);
    public static CoinAddEvent OnCoinAdd;
    //time taken for repair event
    public delegate void RepairTimeEvent(float time);
    public static RepairTimeEvent OnRepair;

    //repair corountine
    public IEnumerator Repair()
    {
        if (_isBroken)
        {
            _isBroken = false;
            OnRepair?.Invoke(_timeToRepair);
            StartCoroutine(BarAnimation());
            yield return new WaitForSeconds(_timeToRepair);
        }
        //after repair add coind and sprite switch
        sr.sprite = _repairedSprite;
        OnCoinAdd?.Invoke(_coinsForRepair);
    }
    //repair progress bar animation
    private IEnumerator BarAnimation()
    {
        float startRepairTime = Time.time;
        float endRepairTime = startRepairTime + _timeToRepair;
        while (Time.time < endRepairTime)
        {
            float xBarScale = (Time.time - startRepairTime) / _timeToRepair;
            _progressBar.transform.localScale = new Vector3(xBarScale, _progressBar.transform.localScale.y
                , _progressBar.transform.localScale.z);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Destroy(_progressBar);
    }

    //check player on trigger zone for start repair
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isBroken && collision.gameObject.GetComponent<PlayerStatus>() != null)
        {
            //button for repair show
            _popUp.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStatus>() != null)
        {
            //button for repair hide
            _popUp.SetActive(false);
        }
    }
    //start repair coroutine
    public void StartRepair()
    {
        StartCoroutine(Repair());
    }
}
