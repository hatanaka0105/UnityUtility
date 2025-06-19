using UnityEngine;

public class DebugOnEnableLog : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.LogError(gameObject.name + "がON");
    }
}