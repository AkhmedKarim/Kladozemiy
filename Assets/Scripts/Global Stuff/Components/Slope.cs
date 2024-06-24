using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Slope : MonoBehaviour
{
    List<ICanTilt> tiltableObjts = new List<ICanTilt>();

    private void Update()
    {
        //bedolaga - обʼєкт який зараз тільтує, тобто в зоні трігеру Slop
        foreach (var bedolaga in tiltableObjts)
        {
            if (bedolaga.rotation >= 50)
            {
                bedolaga.SetRotation(50);
            }
            else if (bedolaga.rotation <= -50)
            {
                bedolaga.SetRotation(-50);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ICanTilt tiltable))
        {
            Debug.Log("enter");
            tiltable.freezeRotation = false;
            tiltableObjts.Add(tiltable);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.TryGetComponent(out ICanTilt tiltable))
        {
            Debug.Log("exit");
            tiltableObjts.Remove(tiltable);

            tiltable.SetRotation(0);
            tiltable.freezeRotation = true;
        }
    }

    IEnumerator Stabilizate(ICanTilt tiltable)
    {
        float t = 0;
        float startRotation = tiltable.rotation;

        while (t < 1)
        {
            tiltable.SetRotation(Mathf.Lerp(tiltable.rotation, 0, t));
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tiltable.SetRotation(0);
        tiltable.freezeRotation = true;
    }

}

public interface ICanTilt
{
    bool freezeRotation { get; set; }
    void SetRotation(float angle);
    float rotation { get;}
    
}


