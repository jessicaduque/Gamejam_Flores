using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    [SerializeField] private Button b_playAgain;
    [SerializeField] private Button b_exit;

    private LevelUIManager _levelUIManager => LevelUIManager.I;
    private void Start()
    {
        b_exit.onClick.AddListener(ControlEndPanel);
        b_playAgain.onClick.AddListener(PlayAgain);
    }

    private void OnEnable()
    {
        ButtonsActivationControl(false);
        StartCoroutine(EnableInputCooldowns(Helpers.blackFadeTime));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator EnableInputCooldowns(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        ButtonsActivationControl(true);
    }

    public void ControlEndPanel()
    {
        ButtonsActivationControl(false);
        _levelUIManager.ControlEndPanel(false);
    }
    public void PlayAgain()
    {
        ButtonsActivationControl(false);
        BlackScreenController.I.FadeOutScene("Main");
    }
    private void ButtonsActivationControl(bool state)
    {
        b_exit.enabled = state;
    }
}
