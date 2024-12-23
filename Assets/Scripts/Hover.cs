using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{

    // Field instantiation.
    public float speed = 3f;
    public float height = 0.1f;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate new y-position of the collectible.
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;

        // Set the collectible's new y-position.
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
