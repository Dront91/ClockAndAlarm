using UnityEngine.UI;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] private Button _alarmSetButton;
    [SerializeField] private Text _buttonText;
    [SerializeField] private GameObject _inputFields;
    [SerializeField] private Clock _clock;
    [SerializeField] private ArrowClock _arrowClock;
    [SerializeField] private GameObject _popUpWindow;
    [SerializeField] private InputField _inputFieldHour;
    public InputField InputFieldHour => _inputFieldHour;
    [SerializeField] private InputField _inputFieldMinute;
    public InputField InputFieldMinute => _inputFieldMinute;
    [SerializeField] private InputField _inputFieldSecond;
    public InputField InputFieldSecond => _inputFieldSecond;
    private Clock.WebTime1 _alarmTime;
    private bool _isAlarmActive = false;
    private float _alarmDelay = 2f;
    private float _timer;

    private void Start()
    {
        _alarmTime = new Clock.WebTime1();
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _alarmDelay) { _timer = _alarmDelay; }
        CheckAlarm();
    }
    public void AlarmSetting()
    {
        _clock.SwitchAlarmSetting();
        _arrowClock.CheckCurrentTimeAMOrPM();
        _inputFields.SetActive(_clock.IsAlarmSetNow);
        if (_clock.IsAlarmSetNow == true)
        {
            _inputFieldHour.text = ((int)_clock.CurrentTime.hour).ToString("D2");
            _inputFieldMinute.text = ((int)_clock.CurrentTime.minute).ToString("D2");
            _inputFieldSecond.text = ((int)_clock.CurrentTime.second).ToString("D2");
            _alarmSetButton.GetComponent<Image>().color = Color.red;
            _buttonText.text = "Confirm";
        }
        else
        {
            SaveAlarmTime();
            _alarmSetButton.GetComponent<Image>().color = Color.green;
            _buttonText.text = "Set Alarm";
        }
    }
    public void CheckFieldInput(InputField inputField)
    {
        int.TryParse(inputField.text, out int value);
        if (inputField == _inputFieldHour)
        {
            if (value >= 23)
            {
                inputField.text = "23";
            }
            if(value < 0)
            {
                inputField.text = "0";
            }
        }
        if(inputField == _inputFieldMinute || inputField == _inputFieldSecond)
        {
            if(value >= 60)
            {
                inputField.text = "59";
            }
            if(value < 0)
            {
                inputField.text = "0";
            }
        }
    }
    private void SaveAlarmTime()
    {
        if(int.TryParse(_inputFieldHour.text, out int hour) && int.TryParse(_inputFieldMinute.text, out int minute) && int.TryParse(_inputFieldSecond.text, out int second))
        {
            _alarmTime.hour = hour;
            _alarmTime.minute = minute;
            _alarmTime.second = second;
            _isAlarmActive = true;
            Debug.LogFormat("Alarm set on [{0}, {1}, {2}]", _alarmTime.hour, _alarmTime.minute, _alarmTime.second);
        }
        else
        {
            _isAlarmActive = false;
        }
    }
    private void CheckAlarm()
    {
        if (_isAlarmActive == true)
        {
            if ((int)_alarmTime.hour == (int)_clock.CurrentTime.hour && (int)_alarmTime.minute == (int)_clock.CurrentTime.minute && (int)_alarmTime.second == (int)_clock.CurrentTime.second)
            {
                StartAlarm();
            }
        }
    }
    private void StartAlarm()
    {
        if (_timer == _alarmDelay)
        {
            _popUpWindow.SetActive(true);
            _timer = 0;
        }
    }

}
