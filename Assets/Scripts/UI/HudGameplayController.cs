using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HudGameplayController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField]
    private GameObject _pausePanel = null;
    [SerializeField]
    private GameObject _topPanel = null;
    [SerializeField]
    private GameObject _startLevelPanel = null;
    [SerializeField]
    private GameObject _endLevelPanel = null;
    [SerializeField]
    private GameObject _gameplayPanel = null;
    [Header("Counters")]
    [SerializeField]
    private TextMeshProUGUI _startLevelCountdown = null;
    [Header("EndGame Info")]
    [SerializeField]
    private Animator _endLevelAnimator = null;
    [SerializeField]
    private CanvasGroup _endLevelGroup = null;
    [SerializeField]
    private Animator _startLevelAnimator = null;
    [Header("Feedback")]
    [SerializeField]
    private TextMeshProUGUI _endTitleText = null;

    private ScenarioController _sceneController;

    public void Initialize(ScenarioController controller)
    {
        _sceneController = controller;
        OnStartLevel();
    }

    private void OnStartLevel()
    {
        _topPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _endLevelPanel.SetActive(false);
        _endLevelGroup.alpha = 0.0f;
        _startLevelPanel.SetActive(true);
        StartCoroutine(StartLevelCountdown());
    }

    private IEnumerator StartLevelCountdown()
    {
        _startLevelCountdown.text = "3";
        _startLevelAnimator.SetTrigger("OnLevelStart");
        yield return new WaitForSeconds(1.0f);
        _startLevelCountdown.text = "2";
        _startLevelAnimator.SetTrigger("OnLevelStart");
        yield return new WaitForSeconds(1.0f);
        _startLevelCountdown.text = "1";
        _startLevelAnimator.SetTrigger("OnLevelStart");
        yield return new WaitForSeconds(1.0f);
        _startLevelCountdown.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        _startLevelPanel.SetActive(false);
        _topPanel.SetActive(true);
        _sceneController.StartLevel();
        yield return null;
    }

    public void OnPause()
    {
        _sceneController.TogglePause(true);
        _pausePanel.SetActive(true);
    }

    public void OnUnpause()
    {
        _sceneController.TogglePause(false);
        _pausePanel.SetActive(false);
    }

    public void OnQuitGame()
    {
        _sceneController.OnQuit();
    }

    private void OnFinishLevel()
    {
        _topPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _endLevelPanel.SetActive(true);

        _endLevelAnimator.SetTrigger("OnLevelEnd");
    }

    public void OnReplay()
    {
        // no scene reset
        _sceneController.InitializeScene();
    }

    public void AddToGameplayUI(Transform t)
    {
        t.SetParent(_gameplayPanel.transform);
        t.localScale = Vector3.one;
    }

    public void OnTeamWon(UnitTeam team)
    {
        _endTitleText.text = team == UnitTeam.Team1 ? "TEAM 1 WON!" : "TEAM 2 WON!";
        OnFinishLevel();
    }
}