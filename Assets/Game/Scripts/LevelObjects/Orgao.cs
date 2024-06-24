using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orgao : MonoBehaviour, IHoldable
{
    private OrgaoSO _organSO;
    private bool _organRotten;

    public string holdableTypeName => "orgao";

    [SerializeField]

    #region Holdable
    public bool CanHold()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Set

    public void SetOrganDetails(OrgaoSO so, bool organRotten)
    {
        _organSO = so;
        _organRotten = organRotten;
    }
    #endregion

    #region Get

    public OrgaoSO GetOrganSO()
    {
        return _organSO;
    }

    #endregion
}
