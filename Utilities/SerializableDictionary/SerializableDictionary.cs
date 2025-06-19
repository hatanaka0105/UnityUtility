using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCustomExtension
{
	/// <summary>
	/// シリアライズ可能な Dictionary
	/// </summary>
	[Serializable]
	public abstract class SerializableDictionary<TKey, TValue, TPair> : IEnumerable<TPair>
		where TPair : SerializableKeyValuePair<TKey, TValue>, new()
	{
		[SerializeField] private List<TPair> _list = new List<TPair>();

		private Dictionary<TKey, TValue> _table;

		private Func<TKey, TValue> _getValMethod = null;

		/// <summary>
		/// 指定されたキーに紐付く値を返します
		/// </summary>
		public TValue this[ TKey key ]
		{
			get
			{
				var pair = _list.Find( c => c.Key.Equals( key ) );
				if(pair != null)
				{
					if(pair.Value == null && _getValMethod != null)
                    {
						var tempVal = _getValMethod.Invoke(pair.Key);
						if (tempVal != null)
						{
							pair.SetValue(pair.Key, pair.Value);
						}
                    }
					return pair.Value;
				}
				return default(TValue);
			}
		}

		public void SetFindTargetAndCacheMethod(Func<TKey, TValue> getMethod)
        {
			_getValMethod = getMethod;
		}

		/// <summary>
		/// Dictionary を返します
		/// </summary>
		public Dictionary<TKey, TValue> Table => _table ?? ( _table = ListToDictionary( _list ) );

		/// <summary>
		/// m_list を Dictionary に変換して返します
		/// </summary>
		private static Dictionary<TKey, TValue> ListToDictionary( IList<TPair> list )
		{
			var dict = new Dictionary<TKey, TValue>();

			foreach ( var n in list )
			{
				dict.Add( n.Key, n.Value );
			}

			return dict;
		}

		/// <summary>
		/// コレクションを反復処理する列挙子を返します
		/// </summary>
		IEnumerator<TPair> IEnumerable<TPair>.GetEnumerator()
		{
			foreach ( var n in _list )
			{
				yield return n;
			}
		}

		/// <summary>
		/// コレクションを反復処理する列挙子を返します
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach ( var n in _list )
			{
				yield return n;
			}
		}
	}
}