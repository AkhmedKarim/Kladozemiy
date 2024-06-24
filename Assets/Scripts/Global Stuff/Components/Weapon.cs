using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public delegate void OnHandTriggerAction(Collider2D collision);
    OnHandTriggerAction action;

    public void SetOnTriggerEnterAction(OnHandTriggerAction action)
    {
        this.action = action;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         action(collision);
    }

}

