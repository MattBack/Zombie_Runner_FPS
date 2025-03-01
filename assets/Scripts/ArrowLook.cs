using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLook : MonoBehaviour
{
    [SerializeField]
    private Transform m_Target = null;
    public Transform LookAtTarget { get { return m_Target; } }

    [SerializeField]
    private Transform m_Spinner;
    public Transform Spinner { get { return m_Spinner; } }

    [SerializeField]
    private Transform m_Scaler;
    public Transform Scaler { get { return m_Scaler; } }

    public float speed;

    public void SetTarget(Transform target = null)
    {
        m_Target = target;
    }

    private ObjectiveManager objectiveManager;

    void Start()
    {
        this.gameObject.SetActive(true);

        objectiveManager = FindObjectOfType<ObjectiveManager>();
    }

    void Update()
    {
        if (objectiveManager)
        {
            SetTarget(objectiveManager.GetObjectiveTarget());
        }


        if(LookAtTarget)
            transform.LookAt(LookAtTarget);

        if (Spinner)
            Spinner.transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}
