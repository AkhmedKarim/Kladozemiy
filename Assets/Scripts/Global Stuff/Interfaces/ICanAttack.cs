using System;
using UnityEngine;
using System.Collections;
public interface ICanAttack
{
    Weapon[] GetAllWeapon();
    Animator GetAnimator { get; }
    void StartCoroutine(IEnumerator enumerator);
}

