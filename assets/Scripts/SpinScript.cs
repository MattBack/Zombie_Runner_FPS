using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    public float spinSpeed = 10f;

     void Update()
    {
        transform.Rotate(0f, yAngle: spinSpeed * Time.deltaTime, 0f, Space.Self);
    }

}
