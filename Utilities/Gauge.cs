using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace UnityCustomExtension.Gauge
{
    /// <summary>
    /// 内部的に増減して最大値、最小値がある変数
    /// </summary>
    public class Gauge
    {
        private int _min;
        private int _max;
        private float _interval;
        private LevelList _level;
        private Tween _tween;

        public int CurrentVal { get; private set; }

        private bool _isLock;

        public Gauge(List<Level> levels = null, float interval = 0f, int max = 100, int min = 0)
        {
            _interval = interval;
            _level = new LevelList(levels);
            _max = max;
            _min = min;
        }

        public void Add(int add, bool forceAdd = false)
        {
            if(_isLock && !forceAdd)
            {
                return;
            }
#if UNITY_EDITOR
            //Debug.LogError("蓄積値増加→" + CurrentVal);
#endif
            //増加
            CurrentVal += add;

            //最大、最小調整
            if (CurrentVal > _max)
            {
                CurrentVal = _max;
            }
            else if (CurrentVal < _min)
            {
                CurrentVal = _min;
            }

            //設定した段階を越えているかチェック
            _level.LevelCheck(CurrentVal);

            //インターバルが設定されている場合は指定秒過ぎるまで増加できない
            if(_interval > 0.0f)
            {
                _isLock = true;
                _tween = DOVirtual.DelayedCall(_interval, Unlock);
            }
        }

        public void Set(int val, bool forceSet = false)
        {
            if (_isLock && !forceSet)
            {
                return;
            }
#if UNITY_EDITOR
            //Debug.LogError("蓄積値増加→" + CurrentVal);
#endif
            //増加
            CurrentVal = val;

            //最大、最小調整
            if (CurrentVal > _max)
            {
                CurrentVal = _max;
            }
            else if (CurrentVal < _min)
            {
                CurrentVal = _min;
            }

            //設定した段階を越えているかチェック
            _level.LevelCheck(CurrentVal);

            //インターバルが設定されている場合は指定秒過ぎるまで増加できない
            if (_interval > 0.0f)
            {
                _isLock = true;
                _tween = DOVirtual.DelayedCall(_interval, Unlock);
            }
        }

        private void Unlock()
        {
            _isLock = false;
        }

        public void OnDisable()
        {
            // Tween破棄
            if (DOTween.instance != null)
            {
                _tween?.Kill();
            }
        }

        /// <summary>
        /// ゲージにおける段階とコールバックの設定
        /// </summary>
        public class LevelList
        {
            private List<Level> _levelList;
            private int _currentIndex;
            public LevelList(List<Level> levelList)
            {
                _levelList = levelList;
                _currentIndex = 0;
            }

            public void LevelCheck(int val)
            {
                if (_currentIndex > 0 && _levelList.Count >= _currentIndex)
                {
                    if (val < _levelList[_currentIndex - 1]._threshold)
                    {
                        DownLevel();
                    }
                }
                if (_levelList.Count <= _currentIndex)
                {
                    return;
                }
                if (val > _levelList[_currentIndex]._threshold)
                {
                    Next();
                }
            }

            public void Next()
            {
                _levelList[_currentIndex]._onLevelStep?.Invoke();
                _currentIndex++;
            }

            public void DownLevel()
            {
                _currentIndex--;
            }
        }

        public class Level
        {
            public Level(int threshold, System.Action onLevelStep)
            {
                _threshold = threshold;
                _onLevelStep = onLevelStep;
            }
            public int _threshold;
            public System.Action _onLevelStep;
        }
    }
}