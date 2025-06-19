using UnityEngine;
//using EscapeModules;

/// <summary>
/// 画面ドラッグで視点を動かせる機能
/// 脱出ゲーム制作にはあまり使われないので一時機能面を分離した
/// このままだと動かないので動かせるようにするには改良が必要
/// </summary>
public class CameraRotateDragController : MonoBehaviour
{
    public static bool CanDragCamera;
    private const float _speed = 0.1f;
    private Vector3 _rotateVector3;
    private float _timeRotate;
    private bool _isRotate;

    void Awake()
    {
        CanDragCamera = true;

        //EventAction.Drag += RotatePlayer;
    }
    void OnDisable()
    {
        //EventAction.Drag -= RotatePlayer;
    }

    private void Update()
    {
        //RotatePlayer();
    }

    private void RotatePlayer(Vector3 rotateAxis)
    {
        if (!CanDragCamera) return;
        Vector3 angle = Vector3.zero;
        angle += new Vector3(-rotateAxis.y * _speed, rotateAxis.x * _speed, rotateAxis.z);
        _rotateVector3 = CheckAngle(angle);
        _timeRotate = 0;
        _isRotate = true;
    }

    private Vector3 CheckAngle(Vector3 angle)
    {
        if (angle.x > 55 && angle.x < 200) angle.x = 55;
        else if (angle.x < 320 && angle.x > 200) angle.x = 320;
        angle.z = 0;
        return angle;
    }
}