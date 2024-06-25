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

    // �rg�o plantado
    private OrgaoSO _plantedOrganSO;

    // Transform da placa para representar planta
    [SerializeField] Transform _signTransform;
    [SerializeField] GameObject _currentSign;

    // Diferentes tipos de modelos de terra
    private Renderer _thisRenderer;
    [SerializeField] GameObject _terrainWithSeed, _terrainWithoutSeed;
    [SerializeField] Texture _textureNotWet, _textureWet;
    private AudioManager _audioManager => AudioManager.I;
    private Player _player => Player.I;

    private void Awake()
    {
        _thisRenderer = GetComponent<Renderer>();
    }

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
        yield return new WaitForSeconds((!_plantReady ? _plantedOrganSO.organTimeGrow : _plantedOrganSO.organTimeRot));

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
        if(holdable != null)
        {
            // Se tiver regador e a semente plantada sem �gua, regue a planta
            if (holdable.holdableTypeName == "regador" && _hasPlant && !_isWatered)
            {
                Debug.Log("Terrain watered by player.");
                _audioManager.PlaySfx("watering");
                WaterTerrain();
                return;
            }
            // Se tiver segurando uma semente e a terra n�o tenha planta
            if (holdable.holdableTypeName == "semente" && !_hasPlant)
            {
                PlantTerrain(_player._itemHeld.GetComponent<Semente>().GetOrganSO());
                _audioManager.PlaySfx("planting");
                _player.ControlSemente(false);
                return;
            }
        }

        // Neste �ltimo caso a planta est� pronta e o usu�rio de m�os vazias para peg�-lo
        _audioManager.PlaySfx("reaping");
        CollectOrgan();
    }
    

    #endregion
}
