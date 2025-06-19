using UnityEngine;

namespace UnityCustomExtension
{
    public class UIFactory<T> : MonoBehaviour
    {
        [SerializeField]
        private GameObject _baseUI;

        [SerializeField]
        private Transform _target;

        public T CreateUI()
        {
            var obj = Instantiate(_baseUI, _target);
            var ui = obj.GetComponent<T>();
            if (ui != null)
            {
                return ui;
            }
            return default(T);
        }
    }
}