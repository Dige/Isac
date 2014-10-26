using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ItemSpawner : MonoBehaviour 
{

    [SerializeField]
    private List<ItemBase> _itemPrefabs = new List<ItemBase>();
    public List<ItemBase> ItemPrefabs
    {
        get { return _itemPrefabs; }
    }

	public void Awake ()
	{
	    var item = (ItemBase)Instantiate(_itemPrefabs[Random.Range(0, _itemPrefabs.Count)]);
	    item.transform.parent = transform;
	    item.transform.localPosition = Vector3.zero;
	}
	
}
