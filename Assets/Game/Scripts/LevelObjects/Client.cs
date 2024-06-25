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
    private float _waitTime = 25f;

    // Bancada onde cliente está
    private Bancada _bancada;

    // UI
    private ClienteUI _uiCliente;

    private SOManager _soManager => SOManager.I;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void InicialSetup()
    {
        // Randomize amount of organs and which ones are wanted
        _amountOrgansWanted = Random.Range(1, 4);
        _organsSOWanted = new OrgaoSO[_amountOrgansWanted];
        for(int i=0; i< _organsSOWanted.Length; i++)
        {
            _organsSOWanted[i] = _soManager.RandomizeOrganSO();
            _uiCliente.AddItemToOrder(_organsSOWanted[i].organNormalSprite);
            _organsSOLeft.Add(_organsSOWanted[i]);
        }
        // Define wait time
        _waitTime *= _amountOrgansWanted;
        StartCoroutine(ClientWaitTime());
    }
    public void RecieveOrgan(OrgaoSO organ)
    {
        // Adicionar órgão na lista de órgãos já dados
        _organsSOGiven.Add(organ);
        // Retirar órgão da lista de órgãos faltando se existir
        for (int i=0; i<_organsSOLeft.Count; i++)
        {
            if(_organsSOLeft[i] == organ)
            {
                _uiCliente.RemoveItemSprite(organ.organNormalSprite);
                _organsSOLeft.RemoveAt(i);
            }
        }
        // Checar se órgãos faltando está nulo, para finalizar pedido do cliente
        if(_organsSOLeft.Count == 0)
        {
            StopAllCoroutines();
            ClientLeave();
        }
    }

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
        //GameController.I.SetPoints();
        _uiCliente.TurnOffWait();
        _uiCliente.TurnOffOrder();
        _bancada.RemoveClient();
    }

    #region Set Bancada

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
