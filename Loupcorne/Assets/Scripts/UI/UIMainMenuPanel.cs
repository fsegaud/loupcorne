using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UIMainMenuPanel : UIPanel
{
    public enum State
    {
        Main,
        Manual,
        Credits,
        Loading
    }

    private State state;

    [SerializeField]
    private Texture loadingTexture;

    protected override void Draw()
    {
        base.Draw();

        switch (this.state)
        {
            case State.Main:
                this.Draw_Main();
                break;

            case State.Credits:
                this.Draw_Credits();
                break;

            case State.Manual:
                this.Draw_Manual();
                break;

            case State.Loading:
                this.Draw_Loading();
                break;
        }
    }

    private void Draw_Main()
    {
        base.Draw();

        GUI.BeginGroup(new Rect(32f, Screen.height * .5f - 185f, 300f, 370f), string.Empty);
        {
            GUI.Box(new Rect(0f, 0f, 300f, 370f), string.Empty);

            if (GUI.Button(new Rect(10f, 10f, 280f, 80f), "NEW GAME"))
            {
                this.state = State.Loading;
                Application.LoadLevel("3C");
            }

            if (GUI.Button(new Rect(10f, 100f, 280f, 80f), "MANUAL"))
            {
                this.state = State.Manual;
            }

            if (GUI.Button(new Rect(10f, 190f, 280f, 80f), "CREDITS"))
            {
                this.state = State.Credits;
            }

            if (GUI.Button(new Rect(10f, 280f, 280f, 80f), "QUIT TO DESKTOP"))
            {
                Application.Quit();
            }
        }
        GUI.EndGroup();
    }

    private void Draw_Credits()
    {
        base.Draw();

        GUI.BeginGroup(new Rect(32f, Screen.height * .5f - 185f, 300f, 370f), string.Empty);
        {
            GUI.Box(new Rect(0f, 0f, 300f, 370f), string.Empty);

            GUI.Label(new Rect(10f, 5f, 280f, 270f), Content.Credits, "credits");

            if (GUI.Button(new Rect(10f, 280f, 280f, 80f), "BACK"))
            {
                this.state = State.Main;
            }
        }
        GUI.EndGroup();
    }

    private void Draw_Manual()
    {
        base.Draw();

        GUI.BeginGroup(new Rect(32f, Screen.height * .5f - 185f, 300f, 370f), string.Empty);
        {
            GUI.Box(new Rect(0f, 0f, 300f, 370f), string.Empty);

            if (GUI.Button(new Rect(10f, 280f, 280f, 80f), "BACK"))
            {
                this.state = State.Main;
            }
        }
        GUI.EndGroup();
    }

    private void Draw_Loading()
    {
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), this.loadingTexture);

        GUI.Label(new Rect(Screen.width - 180f, Screen.height - 40f, 180f, 32f), "Loading...", "loading");
    }

    private class Content
    {
        public static readonly string Credits = string.Empty
            + "<color=#FFC000>Game Design</color>\nKevin Nguyen Kim Tuoï\n\n"
            + "<color=#FFC000>Programming</color>\nPierre Maury\nFrançois Ségaud\n\n"
            + "<color=#FFC000>Art</color>\nBaptiste Doux\nOlivier Leroy\n\n"
            + "<color=#FFC000>Music & Sound Design</color>\nMarc-Antoine Archier\n\n"
            + "<color=#FFC000>Resources</color>\ncgtextures.com\ngame-icons.net\nhttp://dafont.com";
    }
}
