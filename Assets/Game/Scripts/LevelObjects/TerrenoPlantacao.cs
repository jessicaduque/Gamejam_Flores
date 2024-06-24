using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrenoPlantacao : MonoBehaviour, IInteractable
{
    public string interactionPrompt => throw new System.NotImplementedException();

    private bool _hasPlant;
    private bool _isWatered;
    private bool _plantReady;
    private bool _plantRotten;

    private Player _player => Player.I;

    private void ResetTerrain()
    {
        _hasPlant = false;
        _isWatered = false;
        _plantReady = false;
        _plantRotten = false;
    }

    #region Interaction
    public bool CanInteract()
    { 
        if (_player.GetIsHoldingItem())
        {
            var holdable = _player._itemHeld.GetComponent<IHoldable>();
            if (holdable != null)
            {
                if(holdable.holdableTypeName == "regador" && _hasPlant && !_isWatered)
                {
                    return true;
                }

                if(holdable.holdableTypeName == "semente" && !_hasPlant)
                {
                    return true;
                }

                return false;
            }
            else
            {
                Debug.Log("Item held by player isn't holdable. (No IHoldable Script)");
                return false;
            }
        }
        else
        {
            if (_plantReady)
            {
                return true;
            }

            return false;
        }
    }

    public void InteractControl(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
