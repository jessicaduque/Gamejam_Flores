using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    // Sobre o pedido
    private int _amountOrgansWanted;
    private OrgaoSO[] _organsSOWanted;
    private List<OrgaoSO> _organsSOLeft = new List<OrgaoSO>();
    private List<OrgaoSO> _organsSOGiven = new List<OrgaoSO>();

    // Sobre o tempo
    private float _waitTime = 35f;

    // Bancada onde cliente está
    private Bancada _bancada;

    // UI
    private ClienteUI _uiCliente;

    private SOManager _soManager => SOManager.I;
    private GameController _gameController => GameController.I;
    private AudioManager _audioManager => AudioManager.I;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void InicialSetup()
    {
        // Randomize amount of organs and which ones are wanted
        _amountOrgansWanted = Random.Range(1, 3);
        // Make there be higher chances of player getting 1
        if (_amountOrgansWanted == 2 && Random.Range(1, 4) == 2)
            _amountOrgansWanted = 1;
        _organsSOWanted = new OrgaoSO[_amountOrgansWanted];
        for(int i=0; i< _organsSOWanted.Length; i++)
        {
            _gameController.AddMaxPoints(5);
            _organsSOWanted[i] = _soManager.RandomizeOrganSO();
            _uiCliente.AddItemToOrder(_organsSOWanted[i].organNormalSprite);
            _organsSOLeft.Add(_organsSOWanted[i]);
        }
        // Define wait time
        _waitTime *= _amountOrgansWanted;
        StartCoroutine(ClientWaitTime());
    }
    public void RecieveOrgan(OrgaoSO organ, bool isRotten)
    {
        // Adicionar órgão na lista de órgãos já dados
        _organsSOGiven.Add(organ);

        bool needsOrgan = false;
        // Retirar órgão da lista de órgãos faltando se existir
        for (int i=0; i<_organsSOLeft.Count; i++)
        {
            if(_organsSOLeft[i] == organ && !needsOrgan)
            {
                _uiCliente.RemoveItemSprite(organ.organNormalSprite);
                _organsSOLeft.RemoveAt(i);
                needsOrgan = true;
            }
        }

        if (needsOrgan)
        {
            _audioManager.PlaySfx("correctitem");
            _gameController.AddCurrentPoints((isRotten ? 2 : 5));
        }
        else
        {
            _audioManager.PlaySfx("wrongitem");
            _gameController.AddCurrentPoints((isRotten ? -3 : -1));
        }

        // Checar se órgãos faltando está nulo, para finalizar pedido do cliente
        if (_organsSOLeft.Count == 0)
        {
            ClientLeave();
        }
    }

    #region Client Control
    private IEnumerator ClientWaitTime()
    {
        float time = 0;
        while(time < _waitTime)
        {
            _uiCliente.UpdateWaitSprite(time / _waitTime);
            time += Time.deltaTime;
            yield return null;
        }

        ClientLeave();
    }

    private void ClientLeave()
    {
        StopAllCoroutines();
        _uiCliente.TurnOffWait();
        _uiCliente.TurnOffOrder();
        _bancada.RemoveClient();
    }

    #endregion

    #region Set 

    public void SetBancadaUI(Bancada bancada, ClienteUI ui)
    {
        _bancada = bancada;
        _uiCliente = ui;
        _uiCliente.TurnOnWait();
        _uiCliente.TurnOnOrder();
        InicialSetup();
    }

    #endregion
}
