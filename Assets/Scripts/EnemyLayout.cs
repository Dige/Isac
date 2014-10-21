using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemyLayout : MonoBehaviour {


	[SerializeField]
	private List<GameObject> _enemyLayouts = new List<GameObject>();
	public List<GameObject> EnemyLayouts
	{
		get { return _enemyLayouts; }
	}

	void Start () {
	
	}
	
	void Update () {
	
	}
}
