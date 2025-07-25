using UnityEngine;
[CreateAssetMenu(fileName = "MouselookSettings", menuName = "Settings/Mouse Look")]
public class MouseLookSettings : ScriptableObject
{
    public float XSensitivity = 0.3f;
    public float YSensitivity = 0.3f;
    public float zoomedXSensitivity = 0.15f;
    public float zoomedYSensitivity = 0.15f;
    public bool smooth = true;
    public float smoothTime = 7f;
    public float controllerLookSensitivity = 2.0f;
}
