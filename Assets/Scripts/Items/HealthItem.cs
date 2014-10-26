﻿using Assets.Scripts;
using UnityEngine;

public class HealthItem : ItemBase {

    [SerializeField]
    private int _healthAddition = 2;
    public int HealthAddition
    {
        get { return _healthAddition; }
        set { _healthAddition = value; }
    }

    public override void UseItem(Player player)
    {
        player.GetComponent<Player>().Health += HealthAddition;
    }
}
