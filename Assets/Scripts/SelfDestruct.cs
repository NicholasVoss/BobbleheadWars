using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destructTime = 3.0f;

    public void Initiate()
    {
        //destroy alien's dead body after lying on the ground for a certain time
        Invoke("selfDestruct", destructTime);
    }

    private void selfDestruct()
    {
        Destroy(gameObject);
    }
}
