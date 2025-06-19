using UnityEngine;

public class FPSInitializer : MonoBehaviour
{
    [SerializeField]
    private int _targetFPS;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _targetFPS;
    }
}
