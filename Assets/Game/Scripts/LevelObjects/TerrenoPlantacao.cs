using System.Collections;
using UnityEngine;

public class TerrenoPlantacao : MonoBehaviour, IInteractable
{
    public string interactionPrompt => throw new System.NotImplementedException();
    // UI
    private TerrenoPlantacaoUI _uiPlantacao;

    // Controle booleanos
    private bool _hasPlant;
    private bool _isWatered;
    private bool _plantReady;
    private bool _plantRotten;

    // Órgão plantado
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
        _thisRenderer = this.GetComponent<Renderer>();
        _uiPlantacao = this.GetComponent<TerrenoPlantacaoUI>();
    }

    #region Plant terrain
    private void PlantTerrain(OrgaoSO organ)
    {
        _hasPlant = true;
        _plantedOrganSO = organ;
        _uiPlantacao.ControlRegarImage(true);
        _currentSign = Instantiate(_plantedOrganSO.organSignPrefab, _signTransform.position, Quaternion.identity);
    }
    #endregion
    #region Water terrain
    private void WaterTerrain()
    {
        _isWatered = true;
        _uiPlantacao.ControlRegarImage(false);
        StartCoroutine(PlantTime());
    }
    #endregion
    #region Time counts for plantations
    private IEnumerator PlantTime()
    {
        _uiPlantacao.TurnOnWait();
        float time = 0;
        while(time < (!_plantReady ? _plantedOrganSO.organTimeGrow : _plantedOrganSO.organTimeRot))
        {
            time += Time.deltaTime;
            if (!_plantReady)
                _uiPlantacao.UpdateWaitSpriteNormal(time / _plantedOrganSO.organTimeGrow);
            else
                _uiPlantacao.UpdateWaitSpriteColored(time / _plantedOrganSO.organTimeRot);
            yield return null;
        }

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
        _uiPlantacao.TurnOffWait();
        // Controlar a placa
        Destroy(_currentSign);
        _currentSign = null;
        // Spawnar o órgão
        GameObject orgao = Instantiate((!_plantRotten ? _plantedOrganSO.organNormalPrefab : _plantedOrganSO.organRottenPrefab), transform.position, Quaternion.identity);
        orgao.GetComponent<Orgao>().SetOrganDetails(_plantedOrganSO, _plantRotten);
        _player.ControlOrgao(true, orgao);
        // Resetar tudo do terreno
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
        if(_player.GetIsHoldingItem())
        {
            var holdable = _player._itemHeld.GetComponent<IHoldable>();
            // Se tiver regador e a semente plantada sem água, regue a planta
            if (holdable.holdableTypeName == "regador" && _hasPlant && !_isWatered)
            {
                Debug.Log("Terrain watered by player.");
                _audioManager.PlaySfx("watering");
                WaterTerrain();
                return;
            }
            // Se tiver segurando uma semente e a terra não tenha planta
            if (holdable.holdableTypeName == "semente" && !_hasPlant)
            {
                PlantTerrain(_player._itemHeld.GetComponent<Semente>().GetOrganSO());
                _audioManager.PlaySfx("planting");
                _player.ControlSemente(false);
                return;
            }
        }

        // Neste último caso a planta está pronta e o usuário de mãos vazias para pegá-lo
        _audioManager.PlaySfx("reaping");
        CollectOrgan();
    }
    

    #endregion
}
