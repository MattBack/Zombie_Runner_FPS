using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    [System.Serializable]
    public struct Objective
    {
        public string name;
        public Transform location;
        public string message;
        public Animator animator; // Animator component for the objective
        public string animationTrigger; // Animation trigger name
        public Animator rightDoorAnimator;
    }

    public Transform player;
    public Transform arrowPointer;
    public Text objectiveText;
    public Objective[] objectives;

    private int currentObjectiveIndex = 0;

    private void Start()
    {
        if (arrowPointer == null)
        {
            Debug.LogError("ArrowPointer is not assigned in the Inspector.");
        }

        // set the initial objective
        SetObjective(currentObjectiveIndex);
    }

    private void Update()
    {
        // Check if the player has reached the current objective
        if (objectives.Length > 0 && currentObjectiveIndex < objectives.Length &&
            Vector3.Distance(player.position, objectives[currentObjectiveIndex].location.position) < 1.0f)
        {
            // Complete the current objective
            CompleteObjective(currentObjectiveIndex);

            // Move to the next objective
            currentObjectiveIndex++;

            // Check if all objectives are completed
            if (currentObjectiveIndex < objectives.Length)
            {
                SetObjective(currentObjectiveIndex);
            }
            else
            {
                Debug.Log("All objectives completed!");
            }
        }
    }

    private void SetObjective(int index)
    {
        if (objectives != null && index >= 0 && index < objectives.Length)
        {
            objectiveText.text = objectives[index].message;

            if (arrowPointer != null)
            {
                arrowPointer.LookAt(objectives[index].location);
            }
            else
            {
                Debug.LogError("ArrowPointer is not assigned in the Inspector.");
            }
        }
        else
        {
            // Handle the case when objectives are null or the index is out of bounds
            objectiveText.text = "No Objectives";
        }
    }

    public Transform GetObjectiveTarget()
    {
        if (currentObjectiveIndex < objectives.Length)
        {
            return objectives[currentObjectiveIndex].location;
        }

        return null;
    }

    private void CompleteObjective(int index)
    {
        Debug.Log("Completed objective: " + objectives[index].name);

        // Trigger the specific animation for the objective if it has an animator and a trigger
        if (objectives[index].animator != null && !string.IsNullOrEmpty(objectives[index].animationTrigger))
        {
            objectives[index].animator.SetTrigger(objectives[index].animationTrigger);
            objectives[index].rightDoorAnimator.SetTrigger(objectives[index].animationTrigger);
        }
    }
}
