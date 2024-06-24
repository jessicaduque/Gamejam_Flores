using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BancadaRegador : MonoBehaviour, IInteractable
{
    public string interactionPrompt => throw new System.NotImplementedException();
    // Regador da bancada
    [SerializeField] private GameObject _thisRegador;
    private Player _player => Player.I;
    #region Interaction
    public bool CanInteract()
    {
        if (!_player.GetIsHoldingItem())
            return true;
        else if (_player._itemHeld.GetComponent<IHoldable>().holdableTypeName == "regador")
            return true;
        return false;
    }

    public void InteractControl(Interactor interactor)
    {
        // Player pega regador
        if (!_player.GetIsHoldingItem())
        {
            Debug.Log("Player picked up regador.");
            _player.ControlRegador(true);
            _thisRegador.SetActive(false);
        }
        else // Player solta regador na bancada
        {
            Debug.Log("Player put down regador.");
            _player.ControlRegador(false);
            _thisRegador.SetActive(true);
        }
    }
    #endregion
}
