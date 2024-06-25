using Utils.Singleton;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    // Clientes contagens
    [SerializeField] private int _minClients = 5;
    [SerializeField] private int _maxClients = 7;
    private int _clientsLeft;

    // Info pontuação
    public float _finalScorePercent { get; private set; }
    private float _currentPoints = 0;
    private float _maxPoints = 0;

    private LevelUIManager _levelUIManager => LevelUIManager.I;
    private new void Awake() 
    {
        _clientsLeft = Random.Range(_minClients, _maxClients + 1);
    }

    private void Start()
    {
        _levelUIManager.UpdateClientsLeftText(_clientsLeft);
    }

    #region Points

    public void AddMaxPoints(float points)
    {
        _maxPoints += points;
    }

    public void AddCurrentPoints(float points)
    {
        _currentPoints += points;
    }

    private void CalculateFinalScore()
    {
        _finalScorePercent = (_currentPoints * 100) / _maxPoints;
    }

    #endregion

    #region Set

    public void SetNewAmountClients(int clients=-1)
    {
        _clientsLeft += clients;
        _levelUIManager.UpdateClientsLeftText(_clientsLeft);

        if(_clientsLeft == 0)
        {
            CalculateFinalScore();
            _levelUIManager.ControlEndPanel(true);
        }
    }

    #endregion

    #region Get

    public bool GetClientsStillLeft()
    {
        return _clientsLeft > 1;
    }

    #endregion
}
