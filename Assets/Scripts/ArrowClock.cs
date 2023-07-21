using System;
using UnityEngine;
public class ArrowClock : MonoBehaviour
{
    private const float
        _secondsInMinute = 60f,
        _oneCircle = 360f,
        _noon = 12f,
        _hoursToDegrees = 360f / 12f,
        _minutesToDegrees = 360f / 60f,
        _secondToDegrees = 360f / 60f,
        _degreesToHour = 12f / 360f,
        _degreesToMinute = 60f / 360f,
        _degreesToSecond = 60f / 360f;
    [SerializeField] private Transform _hourArrow;
    [SerializeField] private Transform _minuteArrow;
    [SerializeField] private Transform _secondArrow;
    [SerializeField] private Clock _clock;
    [SerializeField] private Alarm _alarm;
    private bool _isAM = true;
    private int _previousTime;

    private void Update()
    {
        if (_clock.IsAlarmSetNow == false)
        {
            ArrowClockAnimation();
        }
    }
    public void CheckCurrentTimeAMOrPM()
    {
        if(_clock.CurrentTime.hour < _noon)
        {
            _isAM = true;
        }
        else
        {
            _isAM = false;
        }
        _previousTime = (int)_clock.CurrentTime.hour;
    }
    public void SetAlarmTimeFromArrow(Transform arrow)
    {
        if(arrow == _hourArrow)
        {
            var newTime = (int)Math.Round(-(_hourArrow.transform.rotation.eulerAngles.z - _oneCircle) * _degreesToHour);
            if (!_isAM) 
            { 
                newTime += (int)_noon;
                if (newTime == 24f && _previousTime == 12f || newTime == 12f && _previousTime == 24f) { _isAM = true; return; }
            }
            if(newTime == 0f && _previousTime == 12f || newTime == 12f && _previousTime == 0f) { _isAM = false; return; }
            _alarm.InputFieldHour.text = newTime.ToString("D2");
            _previousTime = newTime;
        }
        if(arrow == _minuteArrow)
        {
            _alarm.InputFieldMinute.text = ((int)(-(_minuteArrow.transform.rotation.eulerAngles.z - _oneCircle) * _degreesToMinute)).ToString("D2");
        }
        if(arrow == _secondArrow)
        {
            _alarm.InputFieldSecond.text = ((int)(-(_secondArrow.transform.rotation.eulerAngles.z - _oneCircle) * _degreesToSecond)).ToString("D2");
        }
    }
    private void ArrowClockAnimation()
    {
        _hourArrow.localRotation = Quaternion.Euler(0f, 0f, (_clock.CurrentTime.hour + _clock.CurrentTime.minute / _secondsInMinute) * -_hoursToDegrees);
        _minuteArrow.localRotation = Quaternion.Euler(0f, 0f, (_clock.CurrentTime.minute + _clock.CurrentTime.second / _secondsInMinute) * -_minutesToDegrees);
        _secondArrow.localRotation = Quaternion.Euler(0f, 0f, _clock.CurrentTime.second * -_secondToDegrees);
    }

}
