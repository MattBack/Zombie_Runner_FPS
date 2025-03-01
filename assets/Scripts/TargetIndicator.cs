using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 3f;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("taret is empty");
        }
        else {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotateSpeed * Time.deltaTime);
        }
        
    }
}
