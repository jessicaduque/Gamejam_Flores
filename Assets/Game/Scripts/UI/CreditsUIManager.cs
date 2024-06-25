using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private Button b_exitCredits;

    private MenuUIManager _menuUIManager => MenuUIManager.I;

    private void Start()
    {
        b_exitCredits.onClick.AddListener(ControlCreditsPanel);
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

    public void ControlCreditsPanel()
    {
        ButtonsActivationControl(false);
        _menuUIManager.ControlCreditsPanel(false);
    }
    public void ControlCreditsPanel(InputAction.CallbackContext obj)
    {
        ControlCreditsPanel();
    }
    private void ButtonsActivationControl(bool state)
    {
        b_exitCredits.enabled = state;
    }
}
