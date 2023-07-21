using UnityEngine.UI;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] private Clock _clock;
    [SerializeField]
    private Text
        _hourText,
        _minuteText,
        _secondText;

    private void Update()
    {
            _hourText.text = ((int)_clock.CurrentTime.hour).ToString("D2");
            _minuteText.text = ((int)_clock.CurrentTime.minute).ToString("D2");
            _secondText.text = ((int)_clock.CurrentTime.second).ToString("D2");
    }
}
