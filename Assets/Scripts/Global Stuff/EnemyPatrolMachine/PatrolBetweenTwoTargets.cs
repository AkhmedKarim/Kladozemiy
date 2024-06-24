using System;
using UnityEngine;

public class PatrolBetweenTwoTargets : PatrolBetweenTargetsCreator
{
    public PatrolBetweenTwoTargets(Transform carrier, Transform[] targets) : base(carrier)
    {

        for (int i = 0; i < 2; i++)
        {
            targetsList.Add(targets[i]);
        }
    }

    public override bool IsBetweenTargets(Transform obj)
    {
        //тут явно можна написати це краще, не створюючи targetsX та targetsY
        float[] targetsX = new float[targetsList.Count];
        {
            for (int i = 0; i < targetsX.Length; i++)
                targetsX[i] = targetsList[i].position.x;
        }
        float[] targetsY = new float[targetsList.Count];
        {
            for (int i = 0; i < targetsX.Length; i++)
                targetsY[i] = targetsList[i].position.y;
        }
        //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-


        Vector2 lowLimit = new Vector2(
            x: Mathf.Min(targetsX),
            y: Mathf.Min(targetsY)
            );
        Vector2 highLimit = new Vector2(
            x: Mathf.Max(targetsX),
            y: Mathf.Max(targetsY)
            );

        float offsetUp = 10;
        float offsetDown = 1.5f;


        if (!IsInRange(obj.position.y, lowLimit.y - offsetDown, highLimit.y + offsetUp))
            return false;

        else if (IsInRange(obj.position.x, lowLimit.x, highLimit.x))
            return true;

        return false;

        
        bool IsInRange(float value, float min, float max)
        {
            return (value > min && value < max);
        }
    }
}

