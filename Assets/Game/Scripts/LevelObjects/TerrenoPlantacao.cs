using System.Collections;
using UnityEngine;

public class TerrenoPlantacao : MonoBehaviour, IInteractable
{
    public string interactionPrompt => throw new System.NotImplementedException();

    // Controle booleanos
    private bool _hasPlant;
    private bool _isWatered;
    private bool _plantReady;
    private bool _plantRotten;

    // Órgão plantado
    private OrgaoSO _plantedOrganSO;

    // Tempo para plantação e apodrecimento do órgão
    private float _plantTime = 4f;

    // Transform da placa para representar planta
    [SerializeField] Transform _signTransform;
    [SerializeField] GameObject _currentSign;

    private Player _player => Player.I;

    #region Plant terrain
    private void PlantTerrain(OrgaoSO organ)
    {
        _hasPlant = true;
        _plantedOrganSO = organ;
        _currentSign = Instantiate(_plantedOrganSO.organSignPrefab, _signTransform.position, Quaternion.identity);
    }
    #endregion
    #region Water terrain
    private void WaterTerrain()
    {
        _isWatered = true;
        StartCoroutine(PlantTime());
    }
    #endregion
    #region Time counts for plantations
    private IEnumerator PlantTime()
    {
        yield return new WaitForSeconds(_plantTime);

        if (!_plantReady)
        {
            _plantReady = true;
            StartCoroutine(PlantTime());
        }
        else
        {
            _plantRotten = true;
        }
    }

    #endregion
    #region End of Plantation
    private void CollectOrgan()
    {
        Destroy(_currentSign);
        _currentSign = null;
        ResetTerrain();
    }
    private void ResetTerrain()
    {
        _hasPlant = false;
        _isWatered = false;
        _plantReady = false;
        _plantRotten = false;
    }
    #endregion
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
        var holdable = _player._itemHeld.GetComponent<IHoldable>();
        // Se tiver regador e a semente plantada sem água, regue a planta
        if (holdable.holdableTypeName == "regador" && _hasPlant && !_isWatered)
        {
            WaterTerrain();
            return;
        }
        // Se tiver segurando uma semente e a terra não tenha planta
        if (holdable.holdableTypeName == "semente" && !_hasPlant)
        {
            PlantTerrain(_player._itemHeld.GetComponent<Orgao>().GetOrganSO());
            return;
        }

        // Neste último caso a planta está pronta e o usuário de mãos vazias para pegá-lo
        CollectOrgan();
    }
    

    #endregion
}
