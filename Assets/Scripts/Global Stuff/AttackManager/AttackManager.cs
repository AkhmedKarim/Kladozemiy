using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

public class AttackManager
{
    ICanAttack currier;
    private Animator _Animator;//init in constructor

    public bool IsSomeAttackPlaying { get => someAttackIsPlaying; }

    public Abstruct_Attack currentAttack;

    public AttackManager(ICanAttack attacker)
    {
        currier = attacker;
        _Animator = attacker.GetAnimator;
    }

    public void Attacking(AttackGetter attackGetter)
    {
        attackGetter.SetWeapons(currier.GetAllWeapon());
        currentAttack = attackGetter.GetAttack();

        currier.StartCoroutine(WaitForCooldownEnd());
        currier.StartCoroutine(WaitForAttackEnd());
    }

    public void HideAllWeapon()
    {
        Weapon[] weapons = currier.GetAllWeapon();
        foreach (var wepon in weapons)
        {
            wepon.gameObject.SetActive(false);
        }
    }

    bool someAttackIsPlaying = false;
    IEnumerator WaitForCooldownEnd()
    {
        someAttackIsPlaying = true;
        _Animator.Play(currentAttack);

        yield return new WaitForSeconds(currentAttack.CoolDown);
        someAttackIsPlaying = false;

    }
    IEnumerator WaitForAttackEnd()
    {
        float delay = _Animator.GetCurrentAnimatorClipInfo(0).Length;

        currentAttack.SetActiveWeapon(true);

        yield return new WaitForSeconds(delay);

        currentAttack.SetActiveWeapon(false);
    }
}