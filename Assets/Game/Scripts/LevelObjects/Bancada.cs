using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bancada : IInteractable
{
    public string interactionPrompt => throw new System.NotImplementedException();

    private Client[] _clients = new Client[2];
    private Player _player => Player.I;

    #region Client control
    public bool AddClient(Client client)
    {
        if (!HasClients())
        {
            return false;
        }

        if (_clients[0] == null)
        {
            _clients[0] = client;
            return true;
        }
        else
        {
            _clients[1] = client;
            return true;
        }
        
    }
    private void RemoveClient(Client client)
    {
        if (_clients[0] == client)
        {
            _clients[0] = null;
        }
        else if (_clients[1] == client)
        {
            _clients[1] = null;
        }
    }
    private bool HasClients()
    {
        return _clients[0] != null || _clients[0] != null;
    }
    #endregion

    #region Interaction
    public bool CanInteract()
    {
        if (_player.GetIsHoldingItem() && HasClients())
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
