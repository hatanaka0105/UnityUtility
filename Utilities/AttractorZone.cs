using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class AttractorZone : MonoBehaviour
{
    [SerializeField]
    private Transform _attractorPoint; // 引き寄せる中心地点
    [SerializeField]
    private float attractionForce = 10f; // 引力の強さ
    [SerializeField]
    private AnimationCurve _forceByDistance = AnimationCurve.Linear(0, 1, 1, 0); // 距離に応じた力の調整
    [SerializeField]
    private string _targetTag;

    private List<Rigidbody> _affectedBodies = new List<Rigidbody>();

    private void Awake()
    {
        var col = GetComponent<SphereCollider>();
        col.isTrigger = true;
    }

    private void FixedUpdate()
    {
        foreach (var body in _affectedBodies)
        {
            if (body == null) continue;

            Vector3 direction = (_attractorPoint.position - body.position);
            float distance = direction.magnitude;
            Vector3 force = direction.normalized * attractionForce * _forceByDistance.Evaluate(distance);
            body.AddForce(force, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(_targetTag))
        {
            var rb = other.attachedRigidbody;
            if (rb != null && !_affectedBodies.Contains(rb))
            {
                _affectedBodies.Add(rb);
            }
        }
        else
        {
            if (other.CompareTag(_targetTag))
            {
                var rb = other.attachedRigidbody;
                if (rb != null && !_affectedBodies.Contains(rb))
                {
                    _affectedBodies.Add(rb);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (string.IsNullOrEmpty(_targetTag))
        {
            var rb = other.attachedRigidbody;
            if (rb != null)
            {
                _affectedBodies.Remove(rb);
            }
        }
        else
        {
            if (other.CompareTag(_targetTag))
            {
                var rb = other.attachedRigidbody;
                if (rb != null)
                {
                    _affectedBodies.Remove(rb);
                }
            }
        }
    }
}
