using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIGameOverPanel : UIPanel
{
    private enum State
    {
        Waiting = 0,
        Loading
    }

    private State state;

    protected override void Draw()
    {
        base.Draw();

        switch(this.state)
        {
            case State.Waiting:
                this.Draw_Waiting();
                break;

            case State.Loading:
                this.Draw_Loading();
                break;
        }
    }

    private void Draw_Waiting()
    {
        GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), "GAME OVER", "gameover");

        if(GUI.Button(new Rect(Screen.width - 160f, Screen.height - 60f, 150f, 50f), "Continue", "continue"))
        {
            this.state = State.Loading;
            Application.LoadLevel("MainMenu");
        }
    }

    private void Draw_Loading()
    {
        GUI.Label(new Rect(Screen.width - 180f, Screen.height - 40f, 180f, 32f), "Loading...", "loading");
    }
}
