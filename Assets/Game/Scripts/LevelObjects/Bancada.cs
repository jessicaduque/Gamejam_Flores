using System.Collections;
using UnityEngine;

public class Bancada : MonoBehaviour, IInteractable
{
    public string interactionPrompt => throw new System.NotImplementedException();
    // Times to wait before creating a new client
    [SerializeField] float _startSceneClientSpawnTimeMin = 3;
    [SerializeField] float _startSceneClientSpawnTimeMax = 7;
    // Point to instantiate client
    [SerializeField] private Transform _clientTransformPoint;
    // Info about current client at bancada
    private GameObject _currentClient;
    private Client _currentClientScript;
    private Player _player => Player.I;
    private SOManager _soManager => SOManager.I;
    private GameController _gameController => GameController.I;

    private void Start()
    {
        StartCoroutine(CreateClientWait(_startSceneClientSpawnTimeMin, _startSceneClientSpawnTimeMax));
    }

    #region Client control
    public void AddClient()
    {
        _currentClient = Instantiate(_soManager.RandomizeClientPrefab(), _clientTransformPoint.position, Quaternion.identity);
        _currentClientScript = _currentClient.GetComponent<Client>();
    }
    public void RemoveClient()
    {
        _currentClient = null;
        Destroy(_currentClient);
        _currentClientScript = null;
        _currentClient = null;

        _gameController.SetNewAmountClients();

        if (_gameController.GetClientsStillLeft())
             StartCoroutine(CreateClientWait());
    }

    private IEnumerator CreateClientWait(float clientNormalSpawnTimeMin = 2, float clientNormalSpawnTimeMax = 7)
    {
        yield return new WaitForSeconds(Random.Range(clientNormalSpawnTimeMin, clientNormalSpawnTimeMax));
        AddClient();
    }
    #endregion

    #region Interaction
    public bool CanInteract()
    {
        if (_player.GetIsHoldingItem() && _currentClient != null)
        {
            if(_player._itemHeld.GetComponent<IHoldable>().holdableTypeName == "orgao")
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void InteractControl(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
