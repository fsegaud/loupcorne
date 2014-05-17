using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIEndGamePanel : UIPanel
{
    private enum State
    {
        Good = 0,
        Evil,
        Loading
    }

    [SerializeField]
    private State initialState;

    private State state;

    protected override void Bind()
    {
        base.Bind();

        this.state = this.initialState;
    }

    protected override void Draw()
    {
        base.Draw();

        switch (this.state)
        {
            case State.Good:
            case State.Evil:
                this.Draw_Waiting();
                break;

            case State.Loading:
                this.Draw_Loading();
                break;
        }
    }

    private void Draw_Waiting()
    {
        GUI.Label(new Rect(0f, Screen.height * .5f, Screen.width, Screen.height * .5f), this.state == State.Good ? Content.GoodText.ToUpper() : Content.EvilText.ToUpper(), "endtext");

        if (GUI.Button(new Rect(Screen.width - 160f, Screen.height - 60f, 150f, 50f), "Continue", "continue"))
        {
            this.state = State.Loading;
            Application.LoadLevel("MainMenu");
        }
    }

    private void Draw_Loading()
    {
        GUI.Label(new Rect(Screen.width - 180f, Screen.height - 40f, 180f, 32f), "Loading...", "loading");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            switch (this.initialState)
            {
                case State.Good:
                    Application.LoadLevel("EndingBad");
                    break;

                case State.Evil:
                    Application.LoadLevel("EndingGood");
                    break;
            }
        }
    }

    private abstract class Content
    {
        public static readonly string GoodText = "THE USURPER KING FLED THE CATLE\nTHE SURVIVAL OF MOST OF YOUR VILLAGERS MADE YOU A <color=#FFC000>LOVED KING</color>";
        public static readonly string EvilText = "THE USURPER KING FLED THE CATLE\nTHE KILLING OF MOST OF YOUR VILLAGERS MADE YOU A <color=#FF0000>HATRED KING</color>";
    }
}
