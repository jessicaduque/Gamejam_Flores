using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orgao : MonoBehaviour
{
    private OrgaoSO _organSO;
    [SerializeField] Transform _signTransform;

    #region Set

    public void SetOrganSO(OrgaoSO so)
    {
        _organSO = so;
    }

    #endregion
}
