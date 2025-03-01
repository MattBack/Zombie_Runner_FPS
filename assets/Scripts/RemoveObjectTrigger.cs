using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObjectTrigger : MonoBehaviour
{
    // get collider and OnTriggerEnter Destroy(gameObject)
    [SerializeField] GameObject RemoveObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(RemoveObject);
        }
    }

}
