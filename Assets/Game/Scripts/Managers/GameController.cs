using Utils.Singleton;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private int _minClients = 5;
    [SerializeField] private int _maxClients = 7;
    private int _clientsLeft;

    private new void Awake() 
    {
        _clientsLeft = Random.Range(_minClients, _maxClients + 1);
    }

    #region Set

    public void SetNewAmountClients(int clients=-1)
    {
        _clientsLeft += clients;
    }

    #endregion

    #region Get

    public bool GetClientsStillLeft()
    {
        return _clientsLeft > 0;
    }

    #endregion
}
