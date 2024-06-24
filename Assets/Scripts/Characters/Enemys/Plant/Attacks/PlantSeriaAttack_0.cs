using System;
using Unity.VisualScripting;
using UnityEngine;



//дописати
public class PlantSeriaAttack_0 : Abstruct_Attack
{


    public PlantSeriaAttack_0(Weapon[] weapon) :
        base(coolDown: 1.2f,
        damage: 1,
        animationName: "Plant_Attack_1",
        weapon: weapon)
    {

    }

    protected override string[] WeaponNames => new string[] {
        "Tentacles"
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

