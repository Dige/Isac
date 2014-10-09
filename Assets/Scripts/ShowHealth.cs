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

    private PlayerController _playerCharacter;

    public void Start()
    {
        _playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnGUI()
    {
        for (int i = 0; i < _playerCharacter.Health / 2; i++)
        {
            GUI.DrawTexture(new Rect(Screen.width - Screen.width / 4 + i*40,30,40,40), _healthTexture, ScaleMode.ScaleToFit);    
        }

        if (_playerCharacter.Health % 2 == 1)
        {
            GUI.DrawTexture(new Rect(Screen.width - Screen.width / 4 + _playerCharacter.Health / 2 * 40, 30, 40, 40), _halfHealthTexture, ScaleMode.ScaleToFit);
        }
    }
}
