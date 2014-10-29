using UnityEngine;

namespace Assets.Scripts.Menu
{
    [RequireComponent(typeof(GUITexture))]
    public class StartButton : ButtonBase
    {
        protected override void OnButtonClicked()
        {
            Application.LoadLevel("ProceduralGenerationTest");
        }
    }
}
