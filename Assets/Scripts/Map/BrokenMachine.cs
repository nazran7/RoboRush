using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMachine : MonoBehaviour
{
    [SerializeField] private bool _isBroken;
    [SerializeField] private int _coinsForRepair;
    [SerializeField] private float _timeToRepair;
    [SerializeField] private Sprite _brokenSprite;
    [SerializeField] private Sprite _repairedSprite;
    [SerializeField] private KeyCode _repairKey;
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private GameObject _popUp;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (_isBroken)
        {
            sr.sprite = _brokenSprite;
        }
        else
            sr.sprite = _repairedSprite;
    }
    public delegate void CoinAddEvent(int count);
    public static CoinAddEvent OnCoinAdd;
    public delegate void RepairTimeEvent(float time);
    public static RepairTimeEvent OnRepair;

    public IEnumerator Repair()
    {
        if (_isBroken)
        {
            _isBroken = false;
            OnRepair?.Invoke(_timeToRepair);
            StartCoroutine(BarAnimation());
            yield return new WaitForSeconds(_timeToRepair);
        }
        sr.sprite = _repairedSprite;
        OnCoinAdd?.Invoke(_coinsForRepair);
    }
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isBroken && collision.gameObject.GetComponent<PlayerStatus>() != null)
        {
            _popUp.SetActive(true);
            if (Input.GetKey(_repairKey))
                StartCoroutine(Repair());
        }
        else
            _popUp.SetActive(false);
    }
}
