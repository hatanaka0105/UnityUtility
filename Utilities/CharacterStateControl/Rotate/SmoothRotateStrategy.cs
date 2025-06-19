using UnityEngine;
using DG.Tweening;

namespace UnityCustomExtension
{
    public class SmoothRotateStrategy : ICharacterRotateStrategy
    {
        private Transform _trans;
        private Vector3 _target;
        private float _time;
        private bool _isDuringRotate = false;
        private Tween _tween;

        public SmoothRotateStrategy(Transform trans, float time)
        {
            _trans = trans;
            _time = time;
        }

        public void SetRotateStart(Vector3 target)
        {
            _target = target;
            _tween = _trans.DORotate(_target, _time);
        }

        public void UpdateRotate()
        {
        }
    }
}