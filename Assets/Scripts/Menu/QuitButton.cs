using UnityEngine;

namespace Assets.Scripts.Menu
{
    [RequireComponent(typeof(GUITexture))]
    public class QuitButton : ButtonBase
    {
        protected override void OnButtonClicked()
        {
            Application.Quit();
        }
    }
}
