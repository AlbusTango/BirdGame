using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI numbercoinText;

    [Header("Score Manager")]
    public int score = 0;
    private float displayedScore = 0f;
    public float interval = 0.1f;
    public int pointsPerTick = 1;
    public float smoothSpeed = 5f;
    public TMP_Text scoreText;
    public GameObject gameOverScreen;

    [Header("Lives")]
    public int maxLives = 3;
    private int currentLives;
    [SerializeField] private Image[] heartImages;

    public Button pauseButton;

    private CoinManager coinmanager;

    private int highscore;
    private int coin;
    private bool isGameOver = false;

    void Start()
    {
        coinmanager = FindFirstObjectByType<CoinManager>();
        StartCoroutine(AddScoreOverTime());
    }

    void Update()
    {
        // Smoothly score animation
        displayedScore = Mathf.Lerp(displayedScore, score, Time.deltaTime * smoothSpeed);
        scoreText.text = score.ToString();

        highscoreText.text = $"Highscore: {highscore}";

        CoinAdd();
        numbercoinText.text = coin.ToString();

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoseLife();
        }
    }

    IEnumerator AddScoreOverTime()
    {
        while (!isGameOver)
        {
            score += pointsPerTick;
            CheckHighScore();

            yield return new WaitForSeconds(interval);
        }
    }

    public void restartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        pauseButton.interactable = true;
    }

    void Awake()
    {
        Time.timeScale = 1f;

        highscore = PlayerPrefs.GetInt("Highscore", 0);

        if (highscoreText != null)
            highscoreText.text = $"Highscore: {highscore}";

        currentLives = maxLives;
        UpdateHearts();
    }

    void CheckHighScore()
    {
        if (score > highscore)
        {
            highscore = score;

            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save();
        }

    }

    void CoinAdd()
    {
        if (coinmanager != null)
        {
            coin = coinmanager.coinCount;
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        FindFirstObjectByType<GravityController_Touch>()?.DisableControls();

        if (coinmanager != null)
            coinmanager.SaveCoins();

        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);

        pauseButton.interactable = false;

    }


    public void LoseLife()
    {
        if (currentLives <= 0) return;

        currentLives--;
        UpdateHearts();

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < currentLives;
        }
    }
}
