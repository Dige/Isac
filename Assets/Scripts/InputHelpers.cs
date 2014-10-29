using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public static class InputHelpers
    {
        public static bool IsAnyKey(params string[] keys)
        {
            return keys.Any(Input.GetKey);
        }

        public static bool IsAnyKey(params KeyCode[] keys)
        {
            return keys.Any(Input.GetKey);
        }

        public static bool IsAnyKeyDown(params string[] keys)
        {
            return keys.Any(Input.GetKeyDown);
        }

        public static bool IsAnyKeyDown(params KeyCode[] keys)
        {
            return keys.Any(Input.GetKeyDown);
        }
    }
}
