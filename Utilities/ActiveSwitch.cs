using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// ON／OFF状態を切り替えるだけのやつ
    /// </summary>
    public class ActiveSwitch
    {
        private bool _isEnabled;
        private Action _onEnable;
        private Action _onDisable;

        public ActiveSwitch(Action onEnable = null, Action onDisable = null)
        {
            _isEnabled = true;
            _onEnable = onEnable;
            _onDisable = onDisable;
        }

        public ActiveSwitch(bool isEnable, Action onEnable = null, Action onDisable = null)
        {
            _isEnabled = isEnable;
            _onEnable = onEnable;
            _onDisable = onDisable;
        }

        public void Activate()
        {
            if(_isEnabled)
            {
                _isEnabled = false;
                _onDisable?.Invoke();
            }
            else
            {
                _isEnabled = true;
                _onEnable?.Invoke();
            }
        }
    }
}