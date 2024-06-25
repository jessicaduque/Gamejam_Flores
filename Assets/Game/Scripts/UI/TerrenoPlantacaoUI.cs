using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrenoPlantacaoUI : MonoBehaviour
{
    // Espera do cliente
    [SerializeField] private Image _waitImage;
    [SerializeField] private Image _regarImage;

    public void ControlRegarImage(bool state) 
    {
        _regarImage.gameObject.SetActive(state);
    }

    #region Plant wait
    public void TurnOnWait()
    {
        _waitImage.gameObject.SetActive(true);
        _waitImage.fillAmount = 1;
    }

    public void UpdateWaitSpriteNormal(float fillAmount)
    {
        _waitImage.fillAmount = fillAmount;
    }

    public void TurnOffWait()
    {
        _waitImage.gameObject.SetActive(false);
    }

    public void UpdateWaitSpriteColored(float fillAmount)
    {
        _waitImage.fillAmount = fillAmount;
    }


    #endregion
}
