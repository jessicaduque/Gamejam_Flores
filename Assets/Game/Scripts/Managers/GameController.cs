using Utils.Singleton;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private int _minClients;
    [SerializeField] private int _maxClients;
    private int _clientsLeft;

    private new void Awake() { }

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
