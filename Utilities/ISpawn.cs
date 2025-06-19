using UnityEngine;

namespace UnityCustomExtension
{
    /// <summary>
    /// スポーン可能なオブジェクトを動かすための隠蔽
    /// 外部から、もしくはスポーン親から指示を出したいときはこれらを組み合わせる
    /// 移動メソッド等は新しくIMoveMethodを作って定義して注入すること
    /// </summary>
    public interface ISpawn
    {
        void Rotate(Vector3 rotate);
        void SetMoveMethod();
        void StartMove(float speed, float distance);
        void Stop();
        void Death();
    }
}