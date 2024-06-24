using UnityEngine;
public class SkeletonsDefaultAttacksGetter : AttackGetter
{
    public override Abstruct_Attack GetAttack()
    {
       
        int r = Random.Range(0, 2);

        switch (r)
        {
            default:
            case 0:
                return new SkeletonsAttack_1(this.weapons);
            case 1:
                return new SkeletonsAttack_2(this.weapons);
        }


    }
}

