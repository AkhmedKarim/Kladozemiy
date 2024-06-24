using System;
using System.Collections.Generic;

public abstract class AttackGetter
{
    protected Weapon[] weapons;

    //protected Dictionary<Type, Abstruct_Attack> attacksDict;

    //public void SetAttacksDict(Dictionary<Type, Abstruct_Attack> attacksDict)
    //{
    //    this.attacksDict = attacksDict;
    //}

    public void SetWeapons(Weapon[] weapons)
    {
        this.weapons = weapons;
    }

    public abstract Abstruct_Attack GetAttack();
}

