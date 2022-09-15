using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    //SFX audio clips
    [SerializeField] private AudioClip _coinTakeSound;
    [SerializeField] private AudioClip _zapSound;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _jetPackSound;

    //Audio Source on this game object
    private AudioSource _audioSource;
    void Start()
    {
        //get audio source component
        _audioSource = GetComponent<AudioSource>();
        //Subscribe on events for audio play
        PlayerItemPicker.singleton.OnCoinTake += CoinTakeSoundPlay;
        Drone.OnEnemyDestroy += ExplosionSoundPlay;
        PlayerShooter.singleton.OnPlayerShoot += ZapSoundPlay;
        PlayerJetPack.OnJetPackFly += JetPackSoundPlay;
        PlayerBoost.OnBoostFly += JetPackSoundPlay;
    }
    private void OnDestroy()
    {
        //Unsubscribe
        PlayerItemPicker.singleton.OnCoinTake -= CoinTakeSoundPlay;
        Drone.OnEnemyDestroy -= ExplosionSoundPlay;
        PlayerShooter.singleton.OnPlayerShoot -= ZapSoundPlay;
        PlayerJetPack.OnJetPackFly -= JetPackSoundPlay;
        PlayerBoost.OnBoostFly -= JetPackSoundPlay;
    }

    //Coin sound play
    private void CoinTakeSoundPlay(int forDelegate)
    {
        if (_coinTakeSound != null)
        {
            _audioSource.clip = _coinTakeSound;
            _audioSource.Play();
        }
    }
    //Explosion sound play
    private void ExplosionSoundPlay()
    {
        if (_explosionSound != null)
        {
            _audioSource.clip = _explosionSound;
            _audioSource.Play();
        }
    }
    //Zap sound play
    private void ZapSoundPlay()
    {
        if (_zapSound != null)
        {
            _audioSource.clip = _zapSound;
            _audioSource.Play();
        }
    }
    //Jet sound play
    private void JetPackSoundPlay()
    {
        if (_jetPackSound != null)
        {
            _audioSource.clip = _jetPackSound;
            _audioSource.Play();
        }
    }



}
