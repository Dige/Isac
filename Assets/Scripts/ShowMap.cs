﻿using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class ShowMap : MonoBehaviour 
{
    [SerializeField]
    private Texture2D _roomTexture;
    public Texture2D RoomTexture
    {
        get { return _roomTexture; }
        set { _roomTexture = value; }
    }

    [SerializeField]
    private Texture2D _unvisitedRoomTexture;
    public Texture2D UnvisitedRoomTexture
    {
        get { return _unvisitedRoomTexture; }
        set { _unvisitedRoomTexture = value; }
    }


    [SerializeField]
    private Texture2D _playerInRoomTexture;
    public Texture2D PlayerInRoomTexture
    {
        get { return _playerInRoomTexture; }
        set { _playerInRoomTexture = value; }
    }

    [SerializeField]
    private Texture2D _treasureRoomIcon;
    public Texture2D TreasureRoomIcon
    {
        get { return _treasureRoomIcon; }
        set { _treasureRoomIcon = value; }
    }

    [SerializeField]
    private Texture2D _bossRoomIcon;
    public Texture2D BossRoomIcon
    {
        get { return _bossRoomIcon; }
        set { _bossRoomIcon = value; }
    }

    private FloorGenerator _floorGenerator;

	public void Awake()
	{
        _floorGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<FloorGenerator>();
	}

    public void OnGUI()
    {
        if (_floorGenerator == null)
        {
            return;
        }

        var roomWidth = Screen.width/30;
        var roomHeight = Screen.height/30;

        var leftPadding = Screen.width/8;
        var topPadding = Screen.height/7;

        var iconWidth = Screen.width / 19;
        var iconHeight = Screen.height / 19;

        foreach (var room in _floorGenerator.Grid.Rooms.Where(r => r.IsVisibleOnMap))
        {
            var roomTexture = room.PlayerHasVisited ? RoomTexture : UnvisitedRoomTexture;
            GUI.DrawTexture(
                new Rect(leftPadding + room.X*roomWidth, topPadding + -room.Y*roomHeight, roomWidth*2, roomHeight*2),
                room.PlayerIsInRoom ? PlayerInRoomTexture : roomTexture, ScaleMode.ScaleToFit);

            if (room.RoomType == RoomType.BossRoom)
            {
                GUI.DrawTexture(
                    new Rect(leftPadding + room.X*roomWidth + roomWidth/2, topPadding + -room.Y*roomHeight, iconHeight, iconWidth),
                    BossRoomIcon, ScaleMode.ScaleToFit);
            }
            else if (room.RoomType == RoomType.TreasureRoom)
            {
                GUI.DrawTexture(
                    new Rect(leftPadding + room.X*roomWidth + roomWidth/2, topPadding + -room.Y*roomHeight, iconHeight, iconWidth),
                    TreasureRoomIcon, ScaleMode.ScaleToFit);
            }
        }
    }
}
