using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityCustomExtension;

public class PUNTopDownMovementController : MonoBehaviourPunCallbacks, IMoveSpeed
{
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _minSpeed;
    [SerializeField]
    private float _acceleration;

    private Vector3 _currentDir;

    private ICharacterMoveInputStrategy _input;
    private ICharacterMoveStrategy _move;
    private ICharacterRotateStrategy _rotate;

    private bool _cantControl = false;

    private void Start()
    {
        var rigidbody = GetComponent<Rigidbody>();
        _input = new MoveWASDInputStrategy();
        //_move = new AccelerateMoveRigidBody(rigidbody, _maxSpeed, _minSpeed, _acceleration);
        _rotate = new RotateRigidBodyStrategy(transform, rigidbody);
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (_cantControl)
            {
                _currentDir = Vector3.zero;
                _move.Move(_currentDir);
                return;
            }

            _currentDir = _input.InputMovement();
            _move.Move(_currentDir);

            if (_currentDir.magnitude > 0.0f)
            {
                _rotate.SetRotateStart(_currentDir);
            }

            _rotate.UpdateRotate();
        }
    }

    public float GetSpeed()
    {
        return _move.CurrentSpeed().magnitude;
    }

    public Vector3 GetPos()
    {
        return transform.position;
    }

    public void Clear()
    {
        _cantControl = true;
        GetComponent<RandomDanceAnimationController>().RandomDancePlay();
    }

}
