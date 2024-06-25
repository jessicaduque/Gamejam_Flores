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
    private float _waitTime;

    // Bancada onde cliente est�
    private Bancada _bancada;

    private SOManager _soManager => SOManager.I;

    private void OnEnable()
    {
        InicialSetup();
    }

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
            _organsSOLeft.Add(_organsSOWanted[i]);
        }
        // Define wait time
        _waitTime = _amountOrgansWanted * 15;
        StartCoroutine(ClientWaitTime());
    }
    public void RecieveOrgan(OrgaoSO organ)
    {
        // Adicionar �rg�o na lista de �rg�os j� dados
        _organsSOGiven.Add(organ);
        // Retirar �rg�o da lista de �rg�os faltando se existir
        for (int i=0; i<_organsSOLeft.Count; i++)
        {
            if(_organsSOLeft[i] == organ)
            {
                _organsSOLeft.RemoveAt(i);
            }
        }
        // Checar se �rg�os faltando est� nulo, para finalizar pedido do cliente
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
            time += Time.deltaTime;
            yield return null;
        }

        ClientLeave();
    }

    private void ClientLeave()
    {
        //GameController.I.SetPoints();
        _bancada.RemoveClient();
    }

    #region Set Bancada

    public void SetBancada(Bancada bancada)
    {
        _bancada = bancada;
    }

    #endregion
}
