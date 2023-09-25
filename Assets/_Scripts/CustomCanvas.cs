using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.UI;

public class CustomCanvas : MonoBehaviour
{
    public static CustomCanvas instance;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Button startGameBtn;
    [SerializeField] private Button nextLevelLoadBtn;
    [SerializeField] private Button prevLevelBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button quitBtn;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI sliderPercentage;
    [SerializeField] private TextMeshProUGUI scoreText;

    private AsyncOperationHandle<SceneInstance> handle;
    private bool isLoading = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (isLoading)
        {
            slider.value = Mathf.Lerp(slider.value, handle.PercentComplete * 100f, Time.deltaTime * 5f);
            sliderPercentage.text = slider.value.ToString("F2") + "%";
        }
    }

    #region Canvas Buttons
    public void BTN_StartGame()
    {
        // Enable loading panel
        loadingPanel.gameObject.SetActive(true);
        // Close all the buttons
        CloseAllBtn();

        isLoading = true;

        // Load Level 1
        handle = PersistantManager.instance.LoadLevel( 
                    PersistantManager.instance.firstLevel,
                    onSuccess => {

                        Debug.Log(onSuccess);

                        loadingPanel.gameObject.SetActive(false);
                        nextLevelLoadBtn.gameObject.SetActive(true);
                        mainMenuBtn.gameObject.SetActive(true);
                        titleText.gameObject.SetActive(false);
                        quitBtn.gameObject.SetActive(true);
                        scoreText.gameObject.SetActive(true);
                        
                        isLoading = false;
                    }, 
                    onFailure => {
                        isLoading = false;
                        Debug.Log(onFailure);
                    });
    }

    public void BTN_NextLevel()
    {
        loadingPanel.gameObject.SetActive(true);
        CloseAllBtn();

        isLoading = true;

        handle = PersistantManager.instance.LoadLevel( 
                    PersistantManager.instance.secondLevel,
                    onSuccess => { 
                        Debug.Log(onSuccess);

                        loadingPanel.gameObject.SetActive(false);
                        prevLevelBtn.gameObject.SetActive(true);
                        mainMenuBtn.gameObject.SetActive(true);
                        quitBtn.gameObject.SetActive(true);
                        isLoading = false;
                    }, 
                    onFailure => { 
                        isLoading = false;
                        Debug.Log(onFailure); 
                    });
    }

    public void BTN_PrevLevel() => BTN_StartGame();

    public void BTN_MainMenu()
    {
        loadingPanel.gameObject.SetActive(true);
        CloseAllBtn();
        isLoading = true;

        handle = PersistantManager.instance.LoadLevel(
            PersistantManager.instance.mainMenu, 
            onSuccess => {
                loadingPanel.gameObject.SetActive(false);
                startGameBtn.gameObject.SetActive(true);
                titleText.gameObject.SetActive(true);
                quitBtn.gameObject.SetActive(true);
                // Disable the score ui and reset the score
                scoreText.gameObject.SetActive(false);
                PersistantManager.instance.score = 0;
                UpdateScoreText(value: 0);

                isLoading = false;
                Debug.Log(onSuccess);
            }, 
            onFailure => {
                isLoading = false;
                Debug.Log(onFailure);
            });
    }

    public void BTN_ApplicationQuit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion

    private void CloseAllBtn()
    {
        startGameBtn.gameObject.SetActive(false);
        nextLevelLoadBtn.gameObject.SetActive(false);
        prevLevelBtn.gameObject.SetActive(false);
        mainMenuBtn.gameObject.SetActive(false);
        quitBtn.gameObject.SetActive(false);
    }
    
    public void UpdateScoreText(int value)
    {
        var textPattern = $"Score: {PersistantManager.instance.score}";
        scoreText.text = textPattern;
    }
}
