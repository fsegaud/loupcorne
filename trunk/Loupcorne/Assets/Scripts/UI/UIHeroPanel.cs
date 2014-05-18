using UnityEngine;
using System.Collections.Generic;

public class UIHeroPanel : UIPanel
{
    private Dictionary<string, Texture> stateTextures = new Dictionary<string, Texture>();

	protected override void Draw()
    {
        base.Draw();

        Player player = UnitsManager.Instance.player;
        float playerHealthRatio = Mathf.Clamp01(player.Health / player.MaxHealth);

        GUI.BeginGroup(new Rect(0f, 0f, 209f, 97f), string.Empty);
        {
            // Background.
            GUI.Box(new Rect(0f, 0f, 209, 97f), string.Empty, "background");

            // Portrait.
            GUI.Label(new Rect(9f, 12f, 64f, 64f), string.Empty, "portrait");

            // Health bar.
            GUI.Label(new Rect(81f, 12f, 112 * playerHealthRatio, 24f), string.Empty, "healthbar");
            GUI.Label(new Rect(81f, 12f, 112, 24f), string.Format("{0}/{1}", Mathf.RoundToInt(player.Health), player.MaxHealth), "healthbar-label");

            // States.
            float x = 81f;
            foreach (string tag in player.GetTags())
            {
                if (tag.StartsWith("SkillEffect"))
                {
                    if (!this.stateTextures.ContainsKey(tag))
                    {
                        this.stateTextures.Add(tag, Resources.Load<Texture>(string.Format(@"UI/{0}", tag)) as Texture);
                    }

                    GUI.Label(new Rect(x, 46f, 32f, 32f), this.stateTextures[tag]);
                    x += 40f;
                }
            }
        }
        GUI.EndGroup();
    }
}
