using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semente : MonoBehaviour, IHoldable
{
    private OrgaoSO _organSO;

    public string holdableTypeName => "semente";

    #region Holdable
    public bool CanHold()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Set
    public void SetOrganSO(OrgaoSO so)
    {
        _organSO = so;
    }
    #endregion

    #region Get
    public OrgaoSO GetOrganSO()
    {
        return _organSO;
    }
    #endregion
}
