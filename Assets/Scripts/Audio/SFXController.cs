using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    [SerializeField] private AudioClip _coinTakeSound;
    [SerializeField] private AudioClip _zapSound;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _jetPackSound;

    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        PlayerItemPicker.singleton.OnCoinTake += CoinTakeSoundPlay;
        Drone.OnEnemyDestroy += ExplosionSoundPlay;
        PlayerShooter.singleton.OnPlayerShoot += ZapSoundPlay;
        PlayerJetPack.OnJetPackFly += JetPackSoundPlay;
        PlayerBoost.OnBoostFly += JetPackSoundPlay;
    }
    private void OnDestroy()
    {
        PlayerItemPicker.singleton.OnCoinTake -= CoinTakeSoundPlay;
        Drone.OnEnemyDestroy -= ExplosionSoundPlay;
        PlayerShooter.singleton.OnPlayerShoot -= ZapSoundPlay;
        PlayerJetPack.OnJetPackFly -= JetPackSoundPlay;
        PlayerBoost.OnBoostFly -= JetPackSoundPlay;
    }
    private void CoinTakeSoundPlay(int forDelegate)
    {
        if (_coinTakeSound != null)
        {
            _audioSource.clip = _coinTakeSound;
            _audioSource.Play();
        }
    }
    private void ExplosionSoundPlay()
    {
        if (_explosionSound != null)
        {
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
        }
    }
    private void ZapSoundPlay()
    {
        if (_zapSound != null)
        {
            _audioSource.clip = _zapSound;
            _audioSource.Play();
        }
    }
    private void JetPackSoundPlay()
    {
        if (_jetPackSound != null)
        {
            _audioSource.clip = _jetPackSound;
            _audioSource.Play();
        }
    }



}
