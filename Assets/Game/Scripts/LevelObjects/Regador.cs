using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regador : MonoBehaviour, IHoldable
{
    public string holdableTypeName => "regador";

    private Player _player => Player.I;

    #region Holdable
    public bool CanHold()
    {
        if (!_player.GetIsHoldingItem())
            return true;
        return false;
    }
    #endregion
}
