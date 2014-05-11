using UnityEngine;
using System.Collections;

public class UIPanel : MonoBehaviour
{
    [SerializeField]
    protected GUISkin skin;

    private void OnGUI()
    {
        this.Draw();
    }

    protected virtual void Draw()
    {
        if (this.skin != null)
        {
            GUI.skin = this.skin;
        }
    }

    void OnEnable()
    {
        this.Bind();
    }

    void OnDisable()
    {
        this.Unbind();
    }

    protected virtual void Bind()
    {
    }

    protected virtual void Unbind()
    {
    }
}
