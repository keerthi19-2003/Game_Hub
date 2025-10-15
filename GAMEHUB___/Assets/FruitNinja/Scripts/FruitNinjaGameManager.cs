using System;
using System.Collections;
using System.Collections.Generic;
using Menu;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FruitNinja.Scripts {
    public class FruitNinjaGameManager : MonoBehaviour {

        public static readonly List<GameObject> spawnedSceneObjects = new();
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image fadeImage;
        [SerializeField] private TextMeshProUGUI timerText; // Use TextMeshProUGUI for timer display
        [SerializeField] private TextMeshProUGUI livesText; // Text to display lives
        [SerializeField] private float initialTime = 60f; // Initial time in seconds

        private int _score;
        private Blade _blade;
        private Spawner _spawner;
        private float _currentTime; // Tracks the current time
        private bool _isGameRunning = false;
        private int _missedFruits = 0; // Count of missed fruits
        private const int MAX_MISSED_FRUITS = 3; // Maximum allowed missed fruits

        private void Awake() {
            _blade = FindObjectOfType<Blade>();
            _spawner = FindObjectOfType<Spawner>();
        }

        private void Start() {
            NewGame();
        }

        private void OnEnable() {
            restartButton.onClick.AddListener(RestartGame);
            quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable() {
            restartButton.onClick.RemoveListener(RestartGame);
            quitButton.onClick.RemoveListener(QuitGame);
        }

        private void RestartGame() {
            SceneManager.LoadScene((int)Scenes.FruitNinja);
        }

         private void QuitGame() {
            SceneManager.LoadScene((int)Scenes.FinalMenu);
        }

        private void NewGame() {
            Time.timeScale = 1f;
            ClearScene();
            _blade.enabled = true;
            _spawner.enabled = true;
            _score = 0;
            scoreText.text = _score.ToString();
            _currentTime = initialTime;
            UpdateTimerText();
            _isGameRunning = true;
            _missedFruits = 0; // Reset missed fruits count
            UpdateLivesText();
            StartCoroutine(Countdown());
        }

        private void ClearScene() {
            foreach (var sceneObject in spawnedSceneObjects) {
                Destroy(sceneObject.gameObject);
            }
            spawnedSceneObjects.Clear();
        }

        public void IncreaseScore(int amount) {
            _score += amount;
            scoreText.text = _score.ToString();
        }

        public void AddTime(float seconds) {
            _currentTime += seconds;
            UpdateTimerText();
        }

        public void Explode() {
            _blade.enabled = false;
            _spawner.enabled = false;
            StartCoroutine(ExplosionEffect());
            gameOverPanel.SetActive(true);
        }

        public void MissedFruit() {
            _missedFruits++;
            UpdateLivesText();
            if (_missedFruits >= MAX_MISSED_FRUITS) {
                GameOver();
            }
        }

        private IEnumerator ExplosionEffect() {
            yield return LerpOverTime(Color.clear, Color.white, 0.5f);
            yield return new WaitForSecondsRealtime(1f);
            NewGame();
            yield return LerpOverTime(Color.white, Color.clear, 0.5f);
        }

        private void GameOver() {
            _isGameRunning = false;
            _blade.enabled = false;
            _spawner.enabled = false;
            StartCoroutine(ExplosionEffect());
            gameOverPanel.SetActive(true);
        }

        private void UpdateTimerText() {
            int minutes = (int)Mathf.Floor(_currentTime / 60);
            int seconds = (int)_currentTime % 60;
            timerText.text = string.Format("{0}:{1}{2}", minutes, seconds > 9 ? "" : "0", seconds);
        }

        private void UpdateLivesText() {
            livesText.text = $"Lives: {MAX_MISSED_FRUITS - _missedFruits}";
        }

        private IEnumerator Countdown() {
            while (_isGameRunning && _currentTime > 0) {
                _currentTime -= Time.deltaTime;
                UpdateTimerText();
                yield return null;
            }

            if (_isGameRunning) {
                GameOver();
            }
        }

        private IEnumerator LerpOverTime(Color startColor, Color endColor, float duration) {
            var elapsed = 0f;
            while (elapsed < duration) {
                var t = Mathf.Clamp01(elapsed / duration);
                fadeImage.color = Color.Lerp(startColor, endColor, t);

                Time.timeScale = 1f - t;
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
        }
    }
}
