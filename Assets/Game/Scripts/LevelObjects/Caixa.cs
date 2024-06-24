using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caixa : MonoBehaviour, IInteractable
{
    [SerializeField] OrgaoSO _organSO;

    public string interactionPrompt => throw new System.NotImplementedException();
    private Player _player => Player.I;

    #region Interaction
    public bool CanInteract()
    {
        if (!_player.GetIsHoldingItem())
            return true;
        return false;
    }

    public void InteractControl(Interactor interactor)
    {
        Debug.Log("Pegar semente");
    }
    #endregion
}
