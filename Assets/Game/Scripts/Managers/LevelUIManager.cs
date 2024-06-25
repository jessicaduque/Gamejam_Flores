using UnityEngine;
using Utils.Singleton;
using DG.Tweening;
using TMPro;

public class LevelUIManager : Singleton<LevelUIManager>
{
    //[SerializeField] private GameObject _pausePanel; // Pause panel for control
    [SerializeField] private GameObject _endPanel; // End panel for conttrol
    [SerializeField] private TextMeshProUGUI t_clientsLeft;
    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private AudioManager _audioManager => AudioManager.I;
    private new void Awake()
    { }

    public void UpdateClientsLeftText(int amount)
    {
        t_clientsLeft.text = "Clientes restantes: " + amount.ToString();
    }

    #region Control Panels

    //public void ControlPausePanel(bool state)
    //{
    //    if (state)
    //    {
    //        StopAllCoroutines();
    //        Helpers.LockMouse(false);
    //        ThirdPersonController.I.DisableInputs();
    //        Time.timeScale = 0;
    //        DisableInput();
    //        Helpers.FadeInPanel(_pausePanel);
    //    }
    //    else
    //    {
    //        Helpers.LockMouse(true);
    //        StartCoroutine(EnableInputCooldowns(Helpers.panelFadeTime));
    //        Helpers.FadeOutPanel(_pausePanel);
    //    }
    //}

    //public void ControlPausePanel(InputAction.CallbackContext obj)
    //{
    //    ControlPausePanel(true);
    //}

    public void ControlEndPanel(bool state)
    {
        if (state)
        {
            Helpers.LockMouse(false);
            _blackScreenController.FadePanel(_endPanel, true);
        }
        else
        {
            _audioManager.PlayCrossFade("menumusic");
            _blackScreenController.FadeOutScene("Menu");
        }
    }

    #endregion
}
