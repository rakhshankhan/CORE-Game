using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotates the collectible around the y-axis.
        transform.Rotate(new Vector3(0, 60, 0) * Time.deltaTime);
    }
}
