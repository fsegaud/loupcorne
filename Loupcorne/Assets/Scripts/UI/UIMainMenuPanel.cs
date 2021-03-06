﻿using UnityEngine;
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

    [SerializeField]
    private Texture tutoTexture;

    protected override void Draw()
    {
        base.Draw();

        GUI.Label(new Rect(10f, Screen.height - 30f, 150f, 20f), Version.ToString("<b>Version {0}.{1}</b> <color=#FFC000>{2}</color> <color=#808080>({3})</color>"), "version");

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
            GUI.Box(new Rect(0f, 0f, 300f, 370f), string.Empty, "background");

            if (GUI.Button(new Rect(10f, 10f, 280f, 80f), "NEW GAME", "button"))
            {
                this.state = State.Loading;
                Application.LoadLevel("3C");
            }

            if (GUI.Button(new Rect(10f, 100f, 280f, 80f), "MANUAL", "button"))
            {
                this.state = State.Manual;
            }

            if (GUI.Button(new Rect(10f, 190f, 280f, 80f), "CREDITS", "button"))
            {
                this.state = State.Credits;
            }

            if (GUI.Button(new Rect(10f, 280f, 280f, 80f), "QUIT TO DESKTOP", "button"))
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
            GUI.Box(new Rect(0f, 0f, 300f, 370f), string.Empty, "background");

            GUI.Label(new Rect(10f, 5f, 280f, 270f), Content.Credits, "credits");

            if (GUI.Button(new Rect(10f, 280f, 280f, 80f), "BACK", "button"))
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
            GUI.Box(new Rect(0f, 0f, 300f, 370f), string.Empty, "background");

            GUI.Label(new Rect(10f, 5f, 280f, 270f), Content.Manual, "manual");

            if (GUI.Button(new Rect(10f, 280f, 280f, 80f), "BACK", "button"))
            {
                this.state = State.Main;
            }
        }
        GUI.EndGroup();

        float w = Screen.width - 376;
        float h = w * 9f / 16f;
        float x = 344f;
        float y = (Screen.height - h) * .5f;

        GUI.DrawTexture(new Rect(x, y, w, h), this.tutoTexture);
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
            + "<color=#FFC000>Resources</color>\ncgtextures.com\ngame-icons.net\ndafont.com";

        public static readonly string Manual = string.Empty
            + "You are the fallen <color=#FFC000><b>king</b></color> of this land.\n\n"
            + "Your goal is to reclaim your throne by killing the new <color=#FFC000><b>king's guard</b></color>.\n\n"
            + "To do so you have been granted a sword and some spells. Some are <color=#FFC000><b>good</b></color>, others are <color=#FFC000><b>evil</b></color>, use them wisely.\n\n";
    }
}
