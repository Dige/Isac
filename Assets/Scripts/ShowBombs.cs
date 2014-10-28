using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShowBombs : MonoBehaviour
    {
        private PlayerShootController _player;

        public void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShootController>();
        }

        public void OnGUI()
        {
            if (_player == null || _player.BombPrefab == null)
            {
                return;
            }

            var iconTexture = _player.BombPrefab.GetComponent<SpriteRenderer>().sprite.texture;
            var iconWidth = Screen.width / 17;
            var iconHeight = Screen.height / 17;

            GUI.DrawTexture(new Rect(Screen.width - Screen.width / 1.5f, Screen.height / 11.0f, iconWidth, iconHeight), iconTexture, ScaleMode.ScaleToFit);
            GUI.Label(new Rect(Screen.width - Screen.width / 1.5f + iconWidth, Screen.height / 11.0f + iconHeight/4.0f, iconWidth, iconHeight), "x" + _player.NumberofBombs);
        }
    }
}
