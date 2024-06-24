using UnityEngine;
public class PlantSerialAttack_AttacksGetter : AttackGetter
{
    public int attackCounter = 0;

    float lastAttackTime = 0, currentAttackTime = 0;

    public override Abstruct_Attack GetAttack()
    {
        currentAttackTime = Time.time;
        float delay = currentAttackTime - lastAttackTime;

        if (delay > 1.4f || attackCounter > 2)
            attackCounter = 0;

        lastAttackTime = Time.time;


        switch (attackCounter++)
        {
            default:
            case 0:
                return new PlantSeriaAttack_0(weapons);
            case 1:
                return new PlantSeriaAttack_1(weapons);
            case 2:
                return new PlantSeriaAttack_2(weapons);
        }

        
    }
}

