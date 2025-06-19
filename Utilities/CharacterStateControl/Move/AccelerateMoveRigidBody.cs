using Photon.Pun;
using System;
using UnityEngine;

namespace UnityCustomExtension
{
    public class AccelerateMoveRigidBody : ICharacterMoveStrategy
    {
        private Rigidbody _rigidbody;
        private float _power;
        private float _minSpeed;
        private float _maxSpeed;
        private Vector3 _beforeDir;
        private float _drag;

        private Transform _stepRay;             //段差を昇る為のレイを飛ばす位置
        private float _stepDistance = 0.5f;     //レイを飛ばす距離
        private float _stepOffset = 0.3f;       //昇れる段差
        private float _slopeDistance = 1f;      //昇れる段差の位置から飛ばすレイの距離

        private Vector3 _addtionalEnertiaVelocity = Vector3.zero; //動く床の上にいるときの参照ベクトル
        private float _dotRefEnertia = 0f; //動く床の移動方向と自分の移動方向の内積 
        private Vector3 _correctEnertiaVelocity = Vector3.zero;

        private Vector3 _platformPos;
        private Vector3 _platformVelocity;
        private bool _isReservedAdjustPlatform;
        private bool _isSlip = false;
        private Func<int> _playerNumGetter;
        private ForceMode _forceMode;
        private ReferenceWrapper<bool> _isOnGround;
        private bool _isAttenuationSpeed = false;
        private float _multiplier = 1.0f;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="rigidbody">本体のRigidbody</param>
        /// <param name="power">移動量にかける係数</param>
        public AccelerateMoveRigidBody(
            Rigidbody rigidbody, float power, float maxSpeed
            , float minSpeed, Transform stepRay, float stepDistance
            , float stepOffset, float slopeDistance, float drag
            , ref ReferenceWrapper<bool> isOnGround
            , ForceMode forceMode = default
            , Func<int> playerNum = null, bool isSlip = false)
        {
            _rigidbody = rigidbody;
            _power = power;
            _maxSpeed = maxSpeed;
            _minSpeed = minSpeed;
            _stepRay = stepRay;
            _stepDistance = stepDistance;
            _stepOffset = stepOffset;
            _slopeDistance = slopeDistance;
            _isSlip = isSlip;
            _drag = drag;
            _isOnGround = isOnGround;
            _playerNumGetter = playerNum;
            _forceMode = forceMode;
        }

        public void Move(Vector3 dir)
        {
            _rigidbody.drag = _drag;

            if(_isReservedAdjustPlatform && dir.magnitude < 0.1f && _platformVelocity.magnitude > 0.0f)
            {
                if(_rigidbody.velocity.magnitude < _platformVelocity.magnitude)
                {
                    _rigidbody.position += _platformPos;
                }
                _isReservedAdjustPlatform = false;
                _platformPos = Vector3.zero;
                _platformVelocity = Vector3.zero;
            }

            if (dir.magnitude <= 0.05f && !_isSlip)
            {
                var velocity = _rigidbody.velocity;
                velocity.x /= 1.1f;
                velocity.z /= 1.1f;
                _rigidbody.velocity = velocity;
                return;
            }

            //if (dir.magnitude == 0f)
            //{
            //    return;
            //}
            var beforeVelocity = _rigidbody.velocity;
            var moveVec = dir * _power;

            //if (!_isSlip)
            {
                //向きが大きく変わる場合は前の移動方向を打ち消して急ターンする
                //if (Vector3.Dot(_rigidbody.velocity.normalized, dir) < 0.8f)
                {
                    //_rigidbody.velocity *= 0.8f;
                }
            }

            moveVec = Step(dir, moveVec);

            _correctEnertiaVelocity = Vector3.zero;
            if (_addtionalEnertiaVelocity.magnitude != 0f)
            {
                //動く床と自分の移動向きを調べる
                //Unityの物理慣性は少し弱いので、板と同じ方向に移動する時に強めにする 1～3の間
                _dotRefEnertia = Vector3.Dot(_addtionalEnertiaVelocity.normalized, _rigidbody.velocity.normalized);
                _correctEnertiaVelocity = _addtionalEnertiaVelocity * Time.fixedDeltaTime * (_dotRefEnertia + 2f); //同じ向きなら少しプラス、逆方向ならマイナスに0～2倍で補正
            }
            var force = moveVec + _correctEnertiaVelocity;
            //if(_playerNumGetter != null)
            //{
            //    force *= (0.25f * _playerNumGetter());
            //}
            _rigidbody.AddForce(force * _multiplier, _forceMode);
#if UNITY_EDITOR
            //Debug.Log("addForce:" + force + " current:" + _rigidbody.velocity.magnitude);
#endif
            if (_isAttenuationSpeed)
            {
                //Debug.Log("減速 now:" + _rigidbody.velocity + " atte:" + (_rigidbody.velocity / 1.04f));

                //_rigidbody.velocity /= 1.04f;
            }
            //オンライン氷床運び向け、失われた速度を無理やり引き戻す
            if (_isSlip && _drag == 0f)
            {
                var before = beforeVelocity;
                before.y = 0f;
                var current = _rigidbody.velocity;
                current.y = 0f;
                if(before.magnitude < current.magnitude)
                {
                    var diff = (before.magnitude - current.magnitude) * 30f;
                    _rigidbody.AddForce(_rigidbody.velocity * diff, ForceMode.VelocityChange);
                }
            }
            //else
            //{
            //    //if(_rigidbody.velocity.magnitude >= _maxSpeed)
            //    //{
            //    //    var velocity = (dir * _maxSpeed) + _correctEnertiaVelocity;
            //    //    velocity.y = _rigidbody.velocity.y;
            //    //    _rigidbody.velocity = velocity;
            //    //}
            //    //else
            //    {
            //        _rigidbody.AddForce(force, _forceMode);

            //    }
            //}
            //動く床の上にいるとき、床の速度より自分の最高速度が遅い場合は補正する
            if (_addtionalEnertiaVelocity.magnitude > _maxSpeed)
            {
                var newVelocity = _rigidbody.velocity;
                if (_rigidbody.velocity.x > _maxSpeed * _multiplier * ((_dotRefEnertia + 2f) * 1.25f))
                {
                    newVelocity.x = moveVec.x * _maxSpeed * _multiplier * (_dotRefEnertia + 2f) * 1.25f;
                }
                if (_rigidbody.velocity.z > _maxSpeed * _multiplier * ((_dotRefEnertia + 2f) * 1.25f))
                {
                    newVelocity.z = moveVec.z * _maxSpeed * _multiplier * (_dotRefEnertia + 2f) * 1.25f;
                }
                _rigidbody.velocity = newVelocity;
            }
            else
            {
                var newVelocity = _rigidbody.velocity;
                if(_rigidbody.velocity.x > _maxSpeed * _multiplier)
                {
                    newVelocity.x = _maxSpeed * _multiplier;
                }
                else if (_rigidbody.velocity.x < -_maxSpeed * _multiplier)
                {
                    newVelocity.x = -_maxSpeed * _multiplier;
                }
                if(_rigidbody.velocity.z > _maxSpeed * _multiplier)
                {
                    newVelocity.z = _maxSpeed * _multiplier;
                }
                else if(_rigidbody.velocity.z < -_maxSpeed * _multiplier)
                {
                    newVelocity.z = -_maxSpeed * _multiplier;
                }
                _rigidbody.velocity = newVelocity;
            }

#if UNITY_EDITOR
            //Debug.Log("移動:" + dir);
#endif
            if (dir.magnitude == 0f && _rigidbody.velocity.magnitude < 0.1f && !_isSlip)
            {
                _rigidbody.SetXZVelocityZero();
            }

            if (dir.magnitude <= 0f && _rigidbody.velocity.magnitude > 0f
                && _isSlip && _rigidbody.drag == 0f && _beforeDir.magnitude > 0f)
            {
                _rigidbody.AddForce(_beforeDir, ForceMode.Acceleration);
                return;
            }

            _beforeDir = dir;
        }

        private Vector3 Step(Vector3 input, Vector3 vector)
        {
            var trans = _rigidbody.transform;
#if UNITY_EDITOR

            //昇れる段差を表示
            Debug.DrawLine(_stepRay.position + trans.right * 0.6f,
                _stepRay.position + trans.right * 0.5f + input * _stepDistance,
                Color.green);
            Debug.DrawLine(_stepRay.position - trans.right * 0.6f,
                _stepRay.position - trans.right * 0.5f + input * _stepDistance,
                Color.green);
#endif
            //方向キーが押されている時
            if (input.magnitude > 0f)
            {
                Vector3 stepRayPositionDown = _stepRay.position - trans.up * 0.05f;  // 最初のレイ開始位置
                Vector3 stepRayPositionRight = _stepRay.position + trans.right * 0.6f;  // 最初のレイ開始位置
                Vector3 stepRayPositionLeft = _stepRay.position - trans.right * 0.6f;  // 最初のレイ開始位置

                if (StepRay(ref vector, _stepRay.position, trans, input))
                {
                    return vector;
                }
                if (StepRay(ref vector, stepRayPositionDown, trans, input))
                {
                    return vector;
                }
                if (StepRay(ref vector, stepRayPositionRight, trans, input))
                {
                    return vector;
                }
                if (StepRay(ref vector, stepRayPositionLeft, trans, input))
                {
                    return vector;
                }
            }

            return vector;
        }

        /// <summary>
        /// 斜面を登る処理
        /// </summary>
        /// <returns></returns>
        private bool StepRay(ref Vector3 vector, Vector3 stepRayStartPos, Transform trans, Vector3 input)
        {
            RaycastHit hit;
            RaycastHit hit2;
            // 少しずつ上にずらす量（調整可能)
            float stepIncrement = 0.01f;
            float maxHeight = 1.0f;  // 最大試行高さ（調整可能）
            int maxIterations = Mathf.CeilToInt(maxHeight / stepIncrement);  // 最大試行回数

            // 初回のLinecast
            if (Physics.Linecast(stepRayStartPos, stepRayStartPos + input * _stepDistance, out hit
                , LayerMask.GetMask("Default", "Slope", "Ground", "Button"), QueryTriggerInteraction.Ignore))
            {
                if(!_isOnGround.Value)
                {
                    _isAttenuationSpeed = true;
                }
                else
                {
                    _isAttenuationSpeed = false;
                }

                //斜面の場合
                float angleThreshold = 5f;  // 判定に使う角度（例えば30度以上の傾きがあれば斜面と見なす）
                float angle = Vector3.Angle(hit.normal, Vector3.up);  // 法線と上方向ベクトルの角度
                                                                      //斜面の場合
                if (angle < 90 - angleThreshold)
                {
                    float normalizedAngle = Mathf.Clamp((angle - angleThreshold) / (90 - angleThreshold), 0f, 1f);
                    float limit = Mathf.Lerp(0.2f, 10f, normalizedAngle);
                    //超人的な速度にならないよう調整
                    if (_rigidbody.velocity.y > limit)
                    {
                        return false;
                    }
#if UNITY_EDITOR
                    Debug.Log("stepPos:" + trans.position + new Vector3(0f, _stepOffset, 0f)
                        + " end:" + (trans.position + new Vector3(0f, _stepOffset, 0f) + trans.forward * _slopeDistance));
#endif
                    // 進行方向の地面の角度が指定以下、または昇れる段差より下だった場合の移動処理
                    if (!Physics.Linecast(trans.position + new Vector3(0f, _stepOffset, 0f),
                            trans.position + new Vector3(0f, _stepOffset, 0f) + input * _slopeDistance))
                    {
#if UNITY_EDITOR
                        Debug.Log("斜面UP:" + 0.2f + " 対象:" + hit.transform?.name);
#endif
                        vector += new Vector3(0f, _power, 0f);
                        return true;
                    }
                }
                //階段のような段差の場合
                else
                {
                    //一回前にレイ飛ばしただけだと巨大な壁などの判定が不安なので、もう一回身長の高さでレイを飛ばしてみる
                    var topRayStartPos = stepRayStartPos + Vector3.up * 1.5f;
                    if (!Physics.Linecast(topRayStartPos, topRayStartPos + input * _stepDistance, out hit2
                            , LayerMask.GetMask("Default", "Slope", "Ground", "Button"), QueryTriggerInteraction.Ignore))
                        {
                        // 当たった位置の少し奥に進めた位置
                        Vector3 offsetPos = hit.point + trans.forward * Time.deltaTime + new Vector3(0f, _stepOffset, 0f);

                        // 真下にレイを飛ばす
                        RaycastHit downHit;
                        //if (Physics.Raycast(offsetPos, Vector3.down, out downHit, _stepOffset, LayerMask.GetMask("Default", "Slope", "Ground", "Button")))
                        if (Physics.BoxCast(offsetPos, Vector3.one * 0.2f, Vector3.down, out downHit, Quaternion.identity, _stepOffset
                            , LayerMask.GetMask("Default", "Slope", "Ground", "Button"), QueryTriggerInteraction.Ignore))
                        {
                            if (downHit.distance <= 0.0f)
                            {
                                return false;
                            }
                            // 真下のレイが当たった位置とtransform.positionの高さの差分を計算
                            float heightDiff = downHit.point.y - hit.point.y;

                            if (heightDiff > 0f)
                            {
                                // Rigidbodyのy座標を段差の高さ分ずらす
                                Vector3 newPosition = trans.position;
                                //上に上げるだけだと落ちるので少し向き方向に動かす
                                newPosition += vector * Time.deltaTime * 1.5f;
                                // 高さ分だけずらす
                                newPosition.y += heightDiff;
                                // Rigidbodyの位置を更新
                                _rigidbody.position = newPosition;
                                //段差にぶつかって速度が落ちる場合があるので速度を補填
                                //vector *= 1.1f;
#if UNITY_EDITOR
                                Debug.Log("段差 UP:" + heightDiff
                                    + " vec:" + vector
                                    + " 対象:" + hit.transform?.name
                                    + " downHitY:" + downHit.point.y
                                    + " hitPointY:" + hit.point.y
                                    + " downHitDistance:" + downHit.distance);
#endif
                                return true;
                            }
                        }
                    }
                }
            }

            _isAttenuationSpeed = false;
            return false;
        }

        public Vector3 CurrentSpeed()
        {
            if (_rigidbody == null)
            {
                return Vector3.zero;
            }
            return _rigidbody.velocity;
        }

        public Vector3 GetPosition()
        {
            return _rigidbody.position;
        }

        public float GetPower()
        {
            return _power;
        }

        public void SetEnertia(Vector3 velocity)
        {
            _addtionalEnertiaVelocity = velocity;
        }

        public void SetAdjustPos(Vector3 addedPos)
        {
#if UNITY_EDITOR
            Debug.Log("位置調整:" + addedPos);
#endif
            if (_beforeDir.magnitude <= 0.1f)
            {
                _rigidbody.position += addedPos;
            }
        }

        public void SetReserveForAdjustPos(Vector3 platformPos, Vector3 platformVelocity)
        {
            _isReservedAdjustPlatform = true;
            _platformPos = platformPos;
            _platformVelocity = platformVelocity;
        }

        public void Boost(float multiply)
        {
            _multiplier = multiply;
        }

        public void ResetBoost()
        {
            _multiplier = 1f;
        }

        public float GetBoostMultiplier()
        {
            return _multiplier;
        }
    }
}