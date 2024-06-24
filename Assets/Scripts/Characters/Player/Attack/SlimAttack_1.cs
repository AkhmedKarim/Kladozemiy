using System;
using Unity.VisualScripting;
using UnityEngine;

public class SlimAttack_1 : Abstruct_Attack
{
    public SlimAttack_1(Weapon[] weapons) :
        base(coolDown: 1f,
        damage: 1,
        animationName: "Attack_1",
        weapon: weapons)
    {
    }

    protected override string[] WeaponNames => new string[] {
        "LeftHand"
    };

    protected override void ActionsToWeapons(Collider2D collider)
    {
        IDamageable obj;
        if (collider.TryGetComponent(out obj))
        {
            obj.GetHurt(Damage);
        }
    }
}

