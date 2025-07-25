using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedUIWatcher : MonoBehaviour
{
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Debug.Log("Selected: " + EventSystem.current.currentSelectedGameObject.name);
        }
    }
}
