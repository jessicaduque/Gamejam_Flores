using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils.Singleton;
using DG.Tweening;
using System.Collections;

public class LevelUIManager : Singleton<LevelUIManager>
{
    //[SerializeField] private GameObject _pausePanel; // Pause panel for control
    [SerializeField] private GameObject _endPanel; // End panel for conttrol

    private BlackScreenController _blackScreenController => BlackScreenController.I;
    private AudioManager _audioManager => AudioManager.I;
    private new void Awake()
    { }

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
