using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //animator component of player
    [SerializeField] private Animator animator;
    // player mover component
    private PlayerMover pm;

    private void Start()
    {
        //get player mover component
        pm = FindObjectOfType<PlayerMover>();
        //events subscribe for shooting and repair
        PlayerShooter.singleton.OnPlayerShoot += PlayerShootAnimation;
        BrokenMachine.OnRepair += PlayerRepairAnimation;
    }
    private void OnDisable()
    {
        PlayerShooter.singleton.OnPlayerShoot -= PlayerShootAnimation;
        BrokenMachine.OnRepair -= PlayerRepairAnimation;
    }
    private void Update()
    {
        //animation control with state integer
        animator.SetInteger("State", ((int)pm.PlayerState));
    }
    //attack animation play
    private void PlayerShootAnimation()
    {
        animator.Play("Attack");
    }
    //repair animation play
    private void PlayerRepairAnimation(float time)
    {
        animator.Play("Repair");
    }
}
