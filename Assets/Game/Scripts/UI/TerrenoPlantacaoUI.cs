using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrenoPlantacaoUI : MonoBehaviour
{
    // Espera do cliente
    [SerializeField] private Image _waitImage;
    [SerializeField] private Image _regarImage;

    [SerializeField] private Color _corVerde;
    [SerializeField] private Color _corAmarelo;
    [SerializeField] private Color _corVermelho;

    public void ControlRegarImage(bool state) 
    {
        _regarImage.gameObject.SetActive(state);
    }

    #region Plant wait
    public void TurnOnWait()
    {
        _waitImage.gameObject.SetActive(true);
        _waitImage.fillAmount = 0;
    }

    public void UpdateWaitSpriteNormal(float fillAmount)
    {
        _waitImage.fillAmount = fillAmount;
    }

    public void TurnOffWait()
    {
        _waitImage.color = _corVerde;
        _waitImage.gameObject.SetActive(false);
    }

    public void UpdateWaitSpriteColored(float fillAmount)
    {
        _waitImage.fillAmount = fillAmount;

        if(fillAmount < 0.33f)
        {
            _waitImage.color = _corVerde;
        }
        else if(fillAmount < 0.66f)
        {
            _waitImage.color = _corAmarelo;
        }
        else
        {
            _waitImage.color = _corVermelho;
        }
    }


    #endregion
}
