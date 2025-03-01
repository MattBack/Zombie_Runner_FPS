using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinVisuals : MonoBehaviour
{
    [SerializeField] ParticleSystem winVFX;

    void Update()
    {
        GetComponent<WinLevel>().PlayWinVisuals();
    }
}
