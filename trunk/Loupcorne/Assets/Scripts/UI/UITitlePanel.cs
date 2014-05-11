using UnityEngine;
using System.Collections;

public class UITitlePanel : UIPanel
{
    [SerializeField]
    private Texture titleTexture;

    [SerializeField]
    private string nextScene;

    [SerializeField]
    private float timer;

    private AsyncOperation async;

    void Start()
    {
        this.StartCoroutine(this.GoToNextScene());
    }

    private IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(this.timer);

        this.async = Application.LoadLevelAsync(nextScene);
        yield return async;
    }

    protected override void Draw()
    {
        base.Draw();

        GUI.DrawTexture(new Rect(Screen.width * .5f - 512f, Screen.height * .5f - 384f, 1024f, 768f), this.titleTexture);

        if (this.async != null)
        {
            GUI.Label(new Rect(Screen.width - 180f, Screen.height - 40f, 180f, 32f), "Loading...", "progress");
        }
    }
}
