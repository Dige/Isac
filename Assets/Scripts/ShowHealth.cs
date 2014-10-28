using UnityEngine;

public class ShowHealth : MonoBehaviour {

    [SerializeField]
    private Texture2D _healthTexture;
    public Texture2D HealthTexture
    {
        get { return _healthTexture; }
        set { _healthTexture = value; }
    }

    [SerializeField]
    private Texture2D _halfHealthTexture;
    public Texture2D HalfHealthTexture
    {
        get { return _halfHealthTexture; }
        set { _halfHealthTexture = value; }
    }

    [SerializeField]
    private Texture2D _emptyHealthTexture;
    public Texture2D EmptyHealthTexture
    {
        get { return _emptyHealthTexture; }
        set { _emptyHealthTexture = value; }
    }

    private Player _playerCharacter;

    public void Start()
    {
        _playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void OnGUI()
    {
        if (_playerCharacter == null)
        {
            return;
        }

        var heartWidth = Screen.width/20;
        var heartHeight = Screen.height/20;

        var origLeft = Screen.width - Screen.width / 4;
        var left = Screen.width - Screen.width/4;
        var top = Screen.height/11.0f;

        int i; 
        for (i = 1; i <= _playerCharacter.Health / 2; i++)
        {
            GUI.DrawTexture(new Rect(left, top, heartWidth, heartHeight), _healthTexture, ScaleMode.ScaleToFit);
            left += heartWidth;
            if (i % 4 == 0)
            {
                left = origLeft;
                top += heartHeight;
            }

        }

        if (_playerCharacter.Health % 2 == 1)
        {
            GUI.DrawTexture(new Rect(left, top, heartWidth, heartHeight), _halfHealthTexture, ScaleMode.ScaleToFit);
            left += heartWidth;
            i++;
        }

        for (; i <= _playerCharacter.MaxHealth / 2; i++)
        {
            GUI.DrawTexture(new Rect(left, top, heartWidth, heartHeight), _emptyHealthTexture, ScaleMode.ScaleToFit);
            left += heartWidth;
            if (i > 0 && i % 4 == 0)
            {
                left = origLeft;
                top += heartHeight;
            }
        }
    }
}
