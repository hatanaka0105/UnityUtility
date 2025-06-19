using System;
using UnityEngine;

namespace UnityCustomExtension
{
	/// <summary>
	/// シリアライズ可能な KeyValuePair
	/// </summary>
	[Serializable]
	public abstract class SerializableKeyValuePair<TKey, TValue>
	{
		[SerializeField] private TKey   _key   = default;
		[SerializeField] private TValue _value = default;

		/// <summary>
		/// キーを返します
		/// </summary>
		public TKey Key => _key;

		/// <summary>
		/// 値を返します
		/// </summary>
		public TValue Value => _value;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		protected SerializableKeyValuePair()
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		protected SerializableKeyValuePair( TKey key, TValue value )
		{
			_key   = key;
			_value = value;
		}

		public void SetValue(TKey key, TValue val)
        {
			if(_key.ToString() == key.ToString())
            {
				_value = val;
            }
        }
	}
}