using System;
using UnityEngine;

public class SkeletonsAttack_2 : Abstruct_Attack
{
    public SkeletonsAttack_2(Weapon[] weapon) :
        base(coolDown: 1.2f,
        damage: 2,
        animationName: "Attack_2",
        weapon: weapon)
    {
    }

    protected override string[] WeaponNames => new string[] {
        "Sword"
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

