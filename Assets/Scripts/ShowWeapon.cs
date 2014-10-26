using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShowWeapon : MonoBehaviour
    {

        private Player _playerCharacter;

        public void Start()
        {
            _playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        }

        public void OnGUI()
        {
            if (_playerCharacter == null)
            {
                return;
            }

            var bulletSprite =
                _playerCharacter.GetComponent<ShootControllerBase>()
                    .BulletPrefab.GetComponent<SpriteRenderer>()
                    .sprite;
            var iconWidth = Screen.width/14;
            var iconHeight = Screen.height/13;

            GUI.DrawTextureWithTexCoords(new Rect(Screen.width / 2.15f, Screen.height/11.0f, iconWidth, iconHeight),
                bulletSprite.texture, new Rect(0,0, 0.25f, 1.0f));
        }
    }
}