using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Inspector에서 연결할 UI 요소들
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalDistanceText;

    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    // Player 스크립트에서 호출될 함수
    public void HandleGameOver()
    {
        Debug.Log("UI Manager: 게임 오버 처리 시작. 시간 정지.");

        // 1. 월드 시간 정지 (가장 먼저 실행하여 화면 멈춤)
        Time.timeScale = 0f;

        // 2. 게임 오버 UI 활성화
        ShowGameOverUI();
    }

    private void ShowGameOverUI()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (finalDistanceText != null)
            {
                finalDistanceText.text = "GAME OVER!";
            }
        }
    }

    // 재시작 버튼의 OnClick 이벤트에 연결할 함수
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}