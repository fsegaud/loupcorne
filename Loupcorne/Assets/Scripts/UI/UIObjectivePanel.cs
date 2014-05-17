using UnityEngine;
using System.Collections;

public class UIObjectivePanel : UIPanel
{
    private int maxPeons;
    private int maxGuards;

    public float x1, y1, x2, y2;

	private string timeValue = "00:00";

    protected override void Draw()
    {
        base.Draw();

        GUI.BeginGroup(new Rect(Screen.width - 209, 0f, 209f, 97f), string.Empty);
        {
            // Background.
            GUI.Box(new Rect(0f, 0f, 209f, 97f), string.Empty, "background");

            // Remaining peons.
            GUI.Label(new Rect(8f + 40f, 8f + 4f, 50f, 28f), string.Format("{0}<color=#808080>/{1}</color>", UnitsManager.Instance.peons.Count, this.maxPeons), "peons");
            GUI.Label(new Rect(64f + -40f, 8f + 4f, 28f, 28f), string.Empty, "peon_icon");
            
            // Remaining guards.
            GUI.Label(new Rect(8f + 128f, 8f + 4, 50f, 28f), string.Format("{0}<color=#808080>/{1}</color>", UnitsManager.Instance.guards.Count, this.maxGuards), "guards");
            GUI.Label(new Rect(64f + 48f, 8f + 4, 28f, 28f), string.Empty, "guard_icon");

            // Timer
			GUI.Label(new Rect(0f, 64f, 209f, 0f), timeValue, "timer");
        }
        GUI.EndGroup();
    }

    protected override void Bind()
    {
 	     base.Bind();

        GameManager.Instance.OnGameReady += this.GameManager_OnGameReady;
    }

    protected override void Unbind()
    {
 	    base.Unbind();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameReady -= this.GameManager_OnGameReady;
        }
    }

    private void GameManager_OnGameReady(GameManager sender)
    {
        this.maxPeons = UnitsManager.Instance.peons.Count;
        this.maxGuards = UnitsManager.Instance.guards.Count;
    }

	public void SetTimeValue(string timeValue)
	{
		this.timeValue = timeValue;
	}
}
