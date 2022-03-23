using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerMover pm;
    private bool isPlayerDead = false;

    private void Start()
    {
        pm = FindObjectOfType<PlayerMover>();
        PlayerShooter.singleton.OnPlayerShoot += PlayerShootAnimation;
    }
    private void OnDisable()
    {
        PlayerShooter.singleton.OnPlayerShoot -= PlayerShootAnimation;
    }
    private void Update()
    {
        if (!isPlayerDead)
            animator.SetInteger("State", ((int)pm.PlayerState));
    }

    private void OnPlayerDeadAnimation()
    {
        isPlayerDead = true;
        animator.Play("PlayerDeath");
    }
    private void PlayerShootAnimation()
    {
        animator.Play("Attack");
    }
}
