using System;
using Unity.VisualScripting;
using UnityEngine;

public class SlimAttack_2 : Abstruct_Attack
{


    public SlimAttack_2(Weapon[] weapons) :
        base(coolDown: 1f,
        damage: 1,
        animationName: "Attack_2",
        weapon: weapons)
    {

    }

    protected override string[] WeaponNames => new string[] {
        "LeftHand", "RightHand"
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

