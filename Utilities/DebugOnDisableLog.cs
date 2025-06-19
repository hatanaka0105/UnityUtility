using UnityEngine;

public class DebugOnDisableLog : MonoBehaviour
{
    private void OnDisable()
    {
        Debug.LogError(gameObject.name + "がOFF");
    }
}