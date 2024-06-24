using UnityEngine;


public class SlimAttacksGetter : AttackGetter
{
    //public override Abstruct_Attack GetAttack()
    //{
    //    if (_lastDirection != _currentDirection)
    //        return attacksDict[typeof(SlimAttack_2)];
    //    else
    //        return attacksDict[typeof(SlimAttack_1)];
    //}
    public override Abstruct_Attack GetAttack()
    {
        if (_lastDirection != _currentDirection)
            return new SlimAttack_2(this.weapons);
        else
            return new SlimAttack_1(this.weapons);
    }

    int _lastDirection,
        _currentDirection;

    public int LastDirection {set => _lastDirection = value; }
    public int CurrentDirection { set => _currentDirection = value; }

}
