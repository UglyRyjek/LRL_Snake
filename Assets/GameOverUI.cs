using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Transform _whole;
    [SerializeField] private Button _restartButton;

    void Start()
    {
        _restartButton.onClick.RemoveAllListeners();
        _restartButton.onClick.AddListener(RestartScene);
    }

    public void LoadGameOver()
    {
        _whole.gameObject.SetActive(true);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
