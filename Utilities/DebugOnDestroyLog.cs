using UnityEngine;

public class DebugOnDestroyLog : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.LogError(gameObject.name + "がDestroy");
    }
}