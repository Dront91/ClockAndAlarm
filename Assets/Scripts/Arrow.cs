using UnityEngine;
using UnityEngine.EventSystems;

public class Arrow : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform _arrow;
    [SerializeField] private ArrowClock _arrowClock;
    [SerializeField] private Clock _clock;
    public void OnDrag(PointerEventData eventData)
    {
        if (_clock.IsAlarmSetNow == false) return;

        Vector3 globalMosPos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, eventData.position, eventData.pressEventCamera, out globalMosPos))
        {
            _arrow.up = -(_arrow.position - globalMosPos).normalized;
            _arrowClock.SetAlarmTimeFromArrow(_arrow);
        }
    }
}
