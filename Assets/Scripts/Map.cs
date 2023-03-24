using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField] GameObject[] _fieldPrefabs;
    [SerializeField] PlayerMovement _playerPrefab;
    [SerializeField] Button _hackButton;
    [SerializeField] Code _code;
    [SerializeField] FinalScreen _finalScreen;

    private PlayerMovement _player;
    private IField[,] _mapMatrix;
    private IField _currentField;
    private int _hackCount = 0;
    private int _hackSuccesful = 0;
    private int _hackFailed = 0;
    public IField[,] MapMatrix { get; }

    private void Awake()
    {
        GenerateMap();
        InstaniatePlayer();
        SetGenericSubscribes();
        FindAvailable();
        _code.OnHacked += () => ((HackField)_currentField).HackStatus = HackStatuses.Succesful;
        _code.OnHacked += () => _hackSuccesful++;
        _code.OnHacked += TryEnd;
        _code.OnHackFailed += () => ((HackField)_currentField).HackStatus = HackStatuses.Failed;
        _code.OnHackFailed += () => _hackFailed++;
        _code.OnHackFailed += TryEnd;
    }

    private void TryEnd()
    {
        if(_hackFailed + _hackSuccesful == _hackCount)
        {
            _finalScreen.gameObject.SetActive(true);
            if (_hackSuccesful/(float)_hackCount >= 0.5f)
            {
                _finalScreen.SetText("Вы победили!");
            }
            else
            {
                _finalScreen.SetText("Вы проиграли!");
            } 
        }
    }

    private void SetGenericSubscribes()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                _mapMatrix[i, j].OnClicked += _player.Move;
                _player.OnPlayerMove += _mapMatrix[i, j].Reset;
            }
        }
        _player.OnPlayerMove += FindAvailable;
    }

    private void GenerateMap()
    {
        _mapMatrix = new IField[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                int k = Random.Range(0, 100) < 8 ? 1:0;
                _mapMatrix[i, j] = Instantiate(_fieldPrefabs[k],
                    position: new Vector3(i, 0, j), new Quaternion(0, 0, 0, 0), transform).GetComponent<IField>();
                _mapMatrix[i, j].MapPosition = new Vector2(i, j);

                if (_mapMatrix[i,j] is HackField hackField)
                {
                    hackField.OnSetCurrent += ShowHackButton;
                    hackField.OnDisableCurrent += HideHackButton;
                    hackField.OnHacked += HideHackButton; 
                    _hackCount++;
                }
            }
        }
    }

    private void InstaniatePlayer()
    {
        int x = Random.Range(0, 10);
        int y = Random.Range(0, 10);
        Vector3 position = _mapMatrix[x, y].transform.position + Vector3.up;

        _currentField = _mapMatrix[x, y];
        _currentField.IsCurrent = true;
        _player = Instantiate(_playerPrefab, position, new Quaternion(0, 0, 0, 0));
        _player.MapPosition = new Vector2(x, y);
    }

    private void FindAvailable()
    {
        int x = (int)_player.MapPosition.x;
        int y = (int)_player.MapPosition.y;
        if (x > 0)
        {
            _mapMatrix[x - 1, y].IsAvailable = true;
        }
        if (y > 0)
        {
            _mapMatrix[x, y - 1].IsAvailable = true;
        }
        if (x < 9)
        {
            _mapMatrix[x + 1, y].IsAvailable = true;
        }
        if (y < 9)
        {
            _mapMatrix[x, y + 1].IsAvailable = true;
        }
        _currentField = _mapMatrix[x, y];
        _currentField.IsCurrent = true;
    }

    public void ShowHackButton(IField field)
    {
        if(field is HackField hackField && hackField.HackStatus == HackStatuses.Locked)
        {
            _hackButton.gameObject.SetActive(true);
        }
    }

    public void HideHackButton(IField field)
    {
        _hackButton.gameObject.SetActive(false);
    }
}
