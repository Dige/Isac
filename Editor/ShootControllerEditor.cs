using Assets.Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ShootControllerBase), true)]
    public class ShootControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var character = (ShootControllerBase)target;
            EditorGUILayout.LabelField("Min Shoot Pitch:", character.MinShootPitch.ToString());
            EditorGUILayout.LabelField("Max Shoot Pitch:", character.MaxShootPitch.ToString());
            EditorGUILayout.MinMaxSlider(new GUIContent("Damaged Shoot Pitch Range"), ref character.MinShootPitch, ref character.MaxShootPitch, -3.0f, 3.0f);
            EditorGUILayout.LabelField("Min Bullet Collide Pitch:", character.MinBulletCollidePitch.ToString());
            EditorGUILayout.LabelField("Max Bullet Collide Pitch:", character.MaxBulletCollidePitch.ToString());
            EditorGUILayout.MinMaxSlider(new GUIContent("Bullet Collide Pitch Range"), ref character.MinBulletCollidePitch, ref character.MaxBulletCollidePitch, -3.0f, 3.0f);

            if(GUI.changed)
                EditorUtility.SetDirty(character);
        }
    }
}