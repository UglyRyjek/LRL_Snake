using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Transform _whole;
    [SerializeField] private Button _restartButton;

    private void Update()
    {
        if(GameSessionService.I.SessionData.State == SessionData.SessionState.GameOver)
        {
            if (_whole.gameObject.activeInHierarchy == false)
            {
                TurnOnGameOverScreen();
            }
        }
    }

    private void TurnOnGameOverScreen()
    {
        _whole.gameObject.SetActive(true);
        _restartButton.onClick.RemoveAllListeners();
        _restartButton.onClick.AddListener(RestartScene);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
