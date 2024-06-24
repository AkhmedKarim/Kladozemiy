using System;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonsAttack_1 : Abstruct_Attack
{

    public SkeletonsAttack_1(Weapon[] weapons) :
        base(coolDown: 1.4f,
        damage: 1,
        animationName: "Attack_1",
        weapon: weapons)
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

