using UnityEngine;
using System.Collections;

public class UITimerPanel : UIPanel
{
    private string timeValue = "00:00";

    protected override void Draw()
    {
        base.Draw();

        GUI.BeginGroup(new Rect(Screen.width * .5f - 50f, 0f, 100f, 32f), string.Empty);
        {
            // Background.
            GUI.Box(new Rect(0f, 0f, 100f, 32f), string.Empty, "background");

            // Timer.
            GUI.Label(new Rect(0f, 0f, 100f, 32f), timeValue, "timer");
        }
        GUI.EndGroup();
    }

    public void SetTimeValue(string timeValue)
    {
        this.timeValue = timeValue;
    }
}
