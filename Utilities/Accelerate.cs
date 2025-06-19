using System;
using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// 通常はRigidBodyでAddForceするところを自分で書く
    /// </summary>
    public class Accelerate
    {
        private Transform _transform;

        private Vector3 _initialVelocity;

        // 現在速度
        private Vector3 _velocity;

        private Vector3 _prevPosition;

        public Accelerate(Transform transform)
        {
            _transform = transform;
        }

        /// <summary>
        /// 初速を入れる
        /// </summary>
        /// <param name="velocity"></param>
        public void InitializeVelocity(Vector3 velocity)
        {
            _initialVelocity = velocity;
            _prevPosition = _transform.position + (velocity * Time.deltaTime);
        }

        public void Update()
        {
            var position = _transform.position;
            var velocity = (position - _prevPosition) / Time.deltaTime;

            velocity += Physics.gravity * Time.deltaTime;

            _prevPosition = position;
            position += velocity * Time.deltaTime;

            _transform.position = position;
        }

        public void Reset()
        {
            _velocity = Vector3.zero;
        }

        public Vector3 GetVelocity()
        {
            return _velocity;
        }
    }
}