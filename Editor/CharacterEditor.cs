using Assets.Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CharacterBase), true)]
    public class CharacterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var character = (CharacterBase)target;
            EditorGUILayout.LabelField("Min Damaged Pitch:", character.MinDamagedPitch.ToString());
            EditorGUILayout.LabelField("Max Damaged Pitch:", character.MaxDamagedPitch.ToString());
            EditorGUILayout.MinMaxSlider(new GUIContent("Damaged Pitch Range"), ref character.MinDamagedPitch, ref character.MaxDamagedPitch, -3.0f, 3.0f);
            EditorGUILayout.LabelField("Min Die Pitch:", character.MinDiePitch.ToString());
            EditorGUILayout.LabelField("Max Die Pitch:", character.MaxDiePitch.ToString());
            EditorGUILayout.MinMaxSlider(new GUIContent("Die Pitch Range"), ref character.MinDiePitch, ref character.MaxDiePitch, -3.0f, 3.0f);

            if (GUI.changed)
                EditorUtility.SetDirty(character);
        }
    }
}