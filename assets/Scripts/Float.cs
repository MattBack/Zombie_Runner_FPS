using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // position storage variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();


    void Start()
    {
        // store the starting position of the object
        posOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // to make float
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}
