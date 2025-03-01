using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointAction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger entered");
        if (other.CompareTag("Player")) {

            Debug.Log("This should be destroyed");

            gameObject.SetActive(false);
        }
    }
}
