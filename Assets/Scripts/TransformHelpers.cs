using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public static class TransformHelpers
    {
        public static void FlipX(GameObject gameObject)
        {
            Vector3 theScale = gameObject.transform.localScale;
            theScale.x *= -1;
            gameObject.transform.localScale = theScale;
        }

        public static void FlipY(GameObject gameObject)
        {
            Vector3 theScale = gameObject.transform.localScale;
            theScale.y *= -1;
            gameObject.transform.localScale = theScale;
        }
    }
}
