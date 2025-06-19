using UnityEngine;
using UnityEngine.EventSystems;

public class DragMoveUI : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        // ドラッグ中は位置を更新する
        var pos = new Vector3(eventData.position.x - (Screen.width / 2),
                                eventData.position.y - (Screen.height / 2),
                                transform.position.z);
        transform.localPosition = pos;
    }
}
