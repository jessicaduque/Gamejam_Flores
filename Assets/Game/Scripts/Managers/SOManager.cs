using UnityEngine;
using Utils.Singleton;

public class SOManager : Singleton<SOManager>
{
    [SerializeField] private GameObject[] _possibleClientPrefabs;
    private GameObject _lastClientPrefab;

    [SerializeField] private OrgaoSO[] _possibleOrganSOs;

    private new void Awake() { }

    #region Randomization
    public GameObject RandomizeClientPrefab()
    {
        GameObject client = _possibleClientPrefabs[Random.Range(0, _possibleClientPrefabs.Length)];
        while(client == _lastClientPrefab)
        {
            client = _possibleClientPrefabs[Random.Range(0, _possibleClientPrefabs.Length)];
        }
        _lastClientPrefab = client;
        return client;
    }

    public OrgaoSO RandomizeOrganSO()
    {
        return _possibleOrganSOs[Random.Range(0, _possibleOrganSOs.Length)];
    }
    #endregion

}
