using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class Clock : MonoBehaviour
{
    [Serializable]
    public class WebTime1
    {
        public float hour;
        public float minute;
        public float second;
    }
    [Serializable]
    public class WebTime2
    {
        public float hour;
        public float minute;
        public float seconds;
    }
    [SerializeField] private string _url1 = "https://api.api-ninjas.com/v1/worldtime?city=moscow";
    [SerializeField] private string _url2 = "https://timeapi.io/api/Time/current/zone?timeZone=Europe/Moscow";
    private WebTime1 _currentTime;
    public WebTime1 CurrentTime => _currentTime;
    private bool _isClockSet = false;
    private bool _isAlarmSetNow = false;
    public bool IsAlarmSetNow => _isAlarmSetNow;
    private float _timeUpdateDelay = 3600f;
    private float _maxSeconds = 60f;
    private float _maxMinutes = 60f;
    private float _maxHour = 24f;
    private float _timer;
    void Start()
    {
        _currentTime = new WebTime1();
        UpdateClockFromServers();
    }
    private void Update()
    {
        ClockRuntime();
        CheckWebTimeUpdateRequared();
    }
    public void SwitchAlarmSetting() { _isAlarmSetNow = !_isAlarmSetNow; }
    private void CheckWebTimeUpdateRequared()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timeUpdateDelay)
        {
            _timer = 0;
            UpdateClockFromServers();
        }
    }
    private void ClockRuntime()
    {
        if (_isClockSet)
        {
            _currentTime.second += Time.deltaTime;
            if (_currentTime.second >= _maxSeconds)
            {
                _currentTime.second -= _maxSeconds;
                _currentTime.minute += 1;
                if (_currentTime.minute >= _maxMinutes)
                {
                    _currentTime.minute -= _maxMinutes;
                    _currentTime.hour += 1;
                    if (_currentTime.hour >= _maxHour)
                    {
                        _currentTime.hour -= _maxHour;
                    }
                }
            }
        }
    }

    private void UpdateClockFromServers()
    {
         StartCoroutine(LoadTimeFromServers(_url1, _url2));
    }
    private IEnumerator LoadTimeFromServers(string url1, string url2)
    {
        var request1 = UnityWebRequest.Get(url1);
        request1.SetRequestHeader("X-Api-Key", "Ohm9qq4jIwsPovDflCiWBg==seaCCOd3BTDkLJBy");
        var request2 = UnityWebRequest.Get(url2);
        yield return request1.SendWebRequest();
        yield return request2.SendWebRequest();
        if (!(request1.result == UnityWebRequest.Result.ProtocolError) && !(request1.result == UnityWebRequest.Result.ConnectionError))
        {
            if (!(request2.result == UnityWebRequest.Result.ProtocolError) && !(request2.result == UnityWebRequest.Result.ConnectionError))
            {
                var data1 = JsonUtility.FromJson<WebTime1>(request1.downloadHandler.text);
                var data2 = JsonUtility.FromJson<WebTime2>(request2.downloadHandler.text);
                if(!CompareWebTimeAndUpdateCurrentTime(data1, data2))
                {
                    Debug.LogError("The difference between time 1 and time 2 is greater than 1 minute, re-query the servers");
                    _isClockSet = false;
                }
                else
                {
                    _isClockSet = true;
                }
            }
            else
            {
                Debug.LogErrorFormat("Error request [{0}, {1}]", url2, request2.error);
            }
        }
        else
        {
            Debug.LogErrorFormat("Error request [{0}, {1}]", url1, request1.error);
        }
        request1.Dispose();
        request2.Dispose();
    }
    private bool CompareWebTimeAndUpdateCurrentTime(WebTime1 time1, WebTime2 time2)
    {
        if (time1.hour != time2.hour)
        {
            if(Mathf.Abs(time1.minute - time2.minute) == 59.0f)
            {
                if (time1.minute == Mathf.Min(time1.minute, time2.minute))
                {
                    var second = (60.0f - time2.seconds + time1.second) / 2;
                    _currentTime.second = time2.seconds + second;
                    if (_currentTime.second >= 60.0f)
                    {
                        _currentTime.second -= 60.0f;
                        _currentTime.minute = time1.minute;
                        _currentTime.hour = time1.hour;
                    }
                    else
                    {
                        _currentTime.minute = time2.minute;
                        _currentTime.hour = time2.hour;
                    }
                    return true;
                }
                else
                {
                    var second = (60.0f - time1.second + time2.seconds) / 2;
                    _currentTime.second = time1.second + second;
                    if (_currentTime.second >= 60.0f)
                    {
                        _currentTime.second -= 60.0f;
                        _currentTime.minute = time2.minute;
                        _currentTime.hour = time2.hour;
                    }
                    else
                    {
                        _currentTime.minute = time1.minute;
                        _currentTime.hour = time1.hour;
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            _currentTime.hour = time1.hour;
            if (time1.minute != time2.minute)
            {
                if (Mathf.Abs(time1.minute - time2.minute) > 1)
                {
                    return false;
                }
                else
                {
                    if (time1.minute < time2.minute)
                    {
                        var check = (60.0f - time1.second + time2.seconds) / 2;
                        if(check > 30.0f)
                        {
                            return false;
                        }
                        if(time1.second + check >= 60.0f)
                        {
                            _currentTime.second = time1.second + check - 60.0f;
                            _currentTime.minute = time2.minute;
                            return true;
                        }
                        else
                        {
                            _currentTime.second = time1.second + check;
                            _currentTime.minute = time1.minute;
                            return true;
                        }
                        
                    }
                    else
                    {
                        var check = (60.0f - time2.seconds + time1.second) / 2;
                        if (check > 30.0f)
                        {
                            return false;
                        }
                        if (time2.seconds + check >= 60.0f)
                        {
                            _currentTime.second = time2.seconds + check - 60.0f;
                            _currentTime.minute = time1.minute;
                            return true;
                        }
                        else
                        {
                            _currentTime.second = time2.seconds + check;
                            _currentTime.minute = time2.minute;
                            return true;
                        }
                    }
                    
                }
            }
            else
            {
                _currentTime.minute = time1.minute;
                _currentTime.hour = time1.hour;
                if (time1.second == time2.seconds)
                {
                    _currentTime.second = time1.second;
                    return true;
                }
                else
                {
                    _currentTime.second = Mathf.Min(time1.second, time2.seconds) + Mathf.Abs(time1.second - time2.seconds) / 2;
                    return true;
                }
            }
        }
    }
}
