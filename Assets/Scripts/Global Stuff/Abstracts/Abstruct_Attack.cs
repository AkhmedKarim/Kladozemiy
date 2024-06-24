using UnityEngine;
using System.Collections.Generic;
public abstract class Abstruct_Attack
{
    Weapon[] weapons;

    float coolDown;
    int damage;
    string animationName;

    protected abstract string[] WeaponNames { get; }

    public static implicit operator string(Abstruct_Attack instanse)
    {
        return instanse.animationName;
    }

    protected Abstruct_Attack(float coolDown, int damage, string animationName, params Weapon[] weapon)
    {
        this.coolDown = coolDown;
        this.damage = damage;
        this.animationName = animationName;

        weapons = GetOnly(from:weapon, WeaponNames);//weapon;

        SetActionToWeapons();
    }

    public float CoolDown
    {
        get
        {
            return coolDown;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    public void SetActiveWeapon(bool active)
    {
        foreach (var weapon in weapons)
            weapon.gameObject.SetActive(active);
    }

    private  void SetActionToWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.SetOnTriggerEnterAction(ActionsToWeapons);
        }
    }

    protected abstract void ActionsToWeapons(Collider2D collider);

    protected Weapon[] GetOnly(Weapon[] from, params string[] weaponNames)
    {
        List<Weapon> _weapons = new List<Weapon>();

        foreach (string neededName in weaponNames)
        {
            foreach (Weapon weapon in from)
            {
                if (neededName == weapon.name)
                {
                    _weapons.Add(weapon);
                }
            }
        }

        if (_weapons.Count != weaponNames.Length)
        {
            Debug.LogError("Can't Correct Found Weapon Name");
            return null;
        }

        return _weapons.ToArray();
    }
}