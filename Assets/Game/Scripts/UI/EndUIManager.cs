using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    [SerializeField] private Button b_playAgain;
    [SerializeField] private Button b_exit;
    [SerializeField] private Image _gradeImage;
    [SerializeField] private Sprite[] _grades;

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
        SetGradeImage();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void SetGradeImage()
    {
        switch (GameController.I._finalScorePercent)
        {
            case < 20:
                _gradeImage.sprite = _grades[4];
                break;
            case < 40:
                _gradeImage.sprite = _grades[3];
                break;
            case < 70:
                _gradeImage.sprite = _grades[2];
                break;
            case < 90:
                _gradeImage.sprite = _grades[1];
                break;
            case >= 90:
                _gradeImage.sprite = _grades[0];
                break;
        }
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

    #region Buttons
    public void PlayAgain()
    {
        ButtonsActivationControl(false);
        BlackScreenController.I.FadeOutScene("Main");
    }
    private void ButtonsActivationControl(bool state)
    {
        b_exit.enabled = state;
    }
    #endregion
}
