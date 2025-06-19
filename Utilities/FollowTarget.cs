using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField, Header("シーン上の初期配置で距離を維持する")]
    private bool _autoOffset;

    private Vector3 _offset;

    private void Awake()
    {
        if (_target != null && _autoOffset)
        {
            _offset = _target.position - transform.position;
        }
    }

    public void SetTarget(Transform target, Vector3 offset)
    {
        _target = target;
        _offset = offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position - _offset;
        }
    }
}