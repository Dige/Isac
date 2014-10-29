using Assets.Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ItemBase), true)]
    public class ItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var item = (ItemBase)target;
            EditorGUILayout.LabelField("Min Spawn Pitch:", item.MinSpawnPitch.ToString());
            EditorGUILayout.LabelField("Max Spawn Pitch:", item.MaxSpawnPitch.ToString());
            EditorGUILayout.MinMaxSlider(new GUIContent("Spawn Pitch Range"), ref item.MinSpawnPitch, ref item.MaxSpawnPitch, -3.0f, 3.0f);

            if(GUI.changed)
                EditorUtility.SetDirty(item);
        }
    }
}