using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    public string interactionPrompt => throw new System.NotImplementedException();
    private Player _player => Player.I;
    #region Interaction
    public bool CanInteract()
    {
        if (_player.GetIsHoldingItem())
        {
            if(_player._itemHeld.GetComponent<IHoldable>().holdableTypeName != "regador")
            {
                return true;
            }
            return false;
        }
        return false;
    }

    public void InteractControl(Interactor interactor)
    {
        var item = _player._itemHeld.GetComponent<IHoldable>();
        Debug.Log("Player throwing away " + _player._itemHeld.name + ".");
        if (item.holdableTypeName == "semente")
        {
            _player.ControlSemente(false);
        }
        else
        {
            _player.ControlOrgao(false);
        }
    }
    #endregion
}
