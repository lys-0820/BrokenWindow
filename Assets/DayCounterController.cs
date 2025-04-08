using TMPro;
using UnityEngine;

public class DayCounterController : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    private int currentDay = 0;

    void Start()
    {
        UpdateDayText();
        ClockController.OnDayPassed += HandleDayPassed;
    }

    void OnDestroy()
    {
        ClockController.OnDayPassed -= HandleDayPassed;
    }

    private void HandleDayPassed()
    {
        currentDay++;
        UpdateDayText();
    }

    private void UpdateDayText()
    {
        dayText.text = currentDay.ToString();
    }
}
