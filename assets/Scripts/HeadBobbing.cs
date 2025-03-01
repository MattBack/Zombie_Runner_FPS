using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : MonoBehaviour
{

    public GameObject cam;

    public void StartBobbing()
    {
        cam.GetComponent<Animator>().Play("HeadBobbing");
    }

    public void StopBobbing()
    {
        cam.GetComponent<Animator>().Play("NewState");
    }

}
