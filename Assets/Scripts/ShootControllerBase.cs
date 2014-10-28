using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class ShootControllerBase : MonoBehaviour
    {
        [SerializeField]
        private float _bulletSpeed = 0.5f;
        public float BulletSpeed
        {
            get { return _bulletSpeed; }
            set { _bulletSpeed = value; }
        }

		[SerializeField]
		private float _shootSpeed = 0.3f;
		public float ShootingSpeed
		{
			get { return _shootSpeed; }
			set { _shootSpeed = value; }
		}

        [SerializeField]
        private Rigidbody2D _bulletPrefab;
        public Rigidbody2D BulletPrefab
        {
            get { return _bulletPrefab; }
            set { _bulletPrefab = value; }
        }

        [SerializeField]
        private List<AudioSource> _shootClips = new List<AudioSource>(1);
        public List<AudioSource> ShootClips
        {
            get { return _shootClips; }
            set { _shootClips = value; }
        }

        [HideInInspector]
        public float MinShootPitch = -3.0f;
        [HideInInspector]
        public float MaxShootPitch = 3.0f;

        [SerializeField]
        private List<AudioSource> _bulletCollideClips = new List<AudioSource>(1);
        public List<AudioSource> BulletCollideClips
        {
            get { return _bulletCollideClips; }
            set { _bulletCollideClips = value; }
        }
        [HideInInspector]
        public float MinBulletCollidePitch = -3.0f;
        [HideInInspector]
        public float MaxBulletCollidePitch = 3.0f;

		public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }
    }

    [CustomEditor(typeof(ShootControllerBase), true)]
    public class ShootControllerEditor : Editor
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
