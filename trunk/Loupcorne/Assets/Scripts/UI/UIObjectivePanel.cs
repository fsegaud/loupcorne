using UnityEngine;
using System.Collections;

public class UIObjectivePanel : UIPanel
{
    private int maxPeons;
    private int maxGuards;

    protected override void Draw()
    {
        base.Draw();

        GUI.BeginGroup(new Rect(Screen.width - 115f, 0f, 115f, 94f), string.Empty);
        {
            // Background.
            GUI.Box(new Rect(0f, 0f, 115f, 94f), string.Empty, "background");

            // Remaining peons.
            GUI.Label(new Rect(8f + 10, 8f + 4, 50f, 28f), string.Format("{0}<color=#808080>/{1}</color>", UnitsManager.Instance.peons.Count, this.maxPeons), "peons");
            GUI.Label(new Rect(64f + 12, 8f + 4, 28f, 28f), string.Empty, "peon_icon");
            
            // Remaining guards.
            GUI.Label(new Rect(8f + 10, 44f + 4, 50f, 28f), string.Format("{0}<color=#808080>/{1}</color>", UnitsManager.Instance.guards.Count, this.maxGuards), "guards");
            GUI.Label(new Rect(64f + 12, 44f + 4, 28f, 28f), string.Empty, "guard_icon");
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
}
