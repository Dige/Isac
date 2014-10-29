using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Menu
{
    [RequireComponent(typeof(GUITexture))]
    public abstract class ButtonBase : MonoBehaviour
    {
        public Texture Texture;
        public Texture HoverTexture;

        public AudioSource ClickClip;

        public void OnMouseEnter()
        {
            guiTexture.texture = HoverTexture;
        }

        public void OnMouseExit()
        {
            guiTexture.texture = Texture;
        }

        public void OnMouseUp()
        {
            if (ClickClip != null)
            {
                ClickClip.Play();
            }
            OnButtonClicked();
        }

        protected abstract void OnButtonClicked();
    }
}
