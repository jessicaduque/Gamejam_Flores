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
        if (!_player.GetIsHoldingItem())
        {
            _player.ControlRegador(true);
            _thisRegador.SetActive(false);
        }
        else
        {
            _player.ControlRegador(false);
            _thisRegador.SetActive(true);
        }
    }
    #endregion
}
