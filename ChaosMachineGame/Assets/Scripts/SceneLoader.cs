 using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject loadingCanvas; 
    public Slider progressBar;      
    public TextMeshProUGUI progressText; 

    [Header("Settings")]
    [Tooltip("Tempo mínimo em segundos que a tela de carregamento ficará visível, mesmo se o carregamento for rápido.")]
    [Range(0.1f, 5f)]
    public float minimumLoadingTime = 1.0f;

    [Header("Transition Animation Settings")]
    [Tooltip("Tempo que leva para a tela de carregamento subir e desaparecer.")]
    public float fadeOutAnimationDuration = 0.5f;
    [Tooltip("Distância que a tela de carregamento se moverá para cima ao desaparecer.")]
    public float slideUpDistance = 500f; 

    private RectTransform loadingCanvasRect; 
    private string _sceneToLoad; 
    private AsyncOperation _loadingOperation; 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (loadingCanvas != null)
            {
                loadingCanvas.SetActive(false);
                loadingCanvasRect = loadingCanvas.GetComponent<RectTransform>();
                if (loadingCanvasRect != null)
                {
                    loadingCanvasRect.anchoredPosition = Vector2.zero;
                }
            }
        }
    }
    [ContextMenu("Teste")]
    public void LoadTest()
    {
        LoadScene("gui");
    }
    public void LoadScene(string sceneName)
    {
        _sceneToLoad = sceneName;
        if (loadingCanvas != null)
        {
            if (loadingCanvasRect != null)
            {
                loadingCanvasRect.anchoredPosition = Vector2.zero;
            }
            loadingCanvas.SetActive(true);
        }
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        _loadingOperation = SceneManager.LoadSceneAsync(_sceneToLoad);
        _loadingOperation.allowSceneActivation = false;

        float timer = 0f;
        if (progressBar != null) progressBar.value = 0;
        if (progressText != null) progressText.text = "LOADING... 0%";

        while (!_loadingOperation.isDone || timer < minimumLoadingTime)
        {
            timer += Time.deltaTime;

            float realProgress = Mathf.Clamp01(_loadingOperation.progress / 0.9f);

            float combinedProgress = Mathf.Clamp01(Mathf.Max(realProgress, timer / minimumLoadingTime));

            if (realProgress >= 1f && timer < minimumLoadingTime)
            {
                combinedProgress = Mathf.Lerp(realProgress, 0.99f, (minimumLoadingTime - timer) / minimumLoadingTime);
            }
            else if (realProgress >= 1f && timer >= minimumLoadingTime)
            {
                combinedProgress = 1f;
            }

            if (progressBar != null)
            {
                progressBar.value = combinedProgress;
            }
            if (progressText != null)
            {
                progressText.text = $"LOADING... {(combinedProgress * 100):F0}%";
            }
            if (_loadingOperation.progress >= 0.9f && timer >= minimumLoadingTime)
            {
                _loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        if (loadingCanvas != null && loadingCanvasRect != null)
        {
            Vector2 startPos = loadingCanvasRect.anchoredPosition;
            Vector2 endPos = startPos + new Vector2(0, slideUpDistance);

            float animationTimer = 0f;

            while (animationTimer < fadeOutAnimationDuration)
            {
                animationTimer += Time.deltaTime;
                float t = animationTimer / fadeOutAnimationDuration;
                loadingCanvasRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }

            loadingCanvasRect.anchoredPosition = endPos;
            loadingCanvas.SetActive(false);
            loadingCanvasRect.anchoredPosition = Vector2.zero; 
        }
        else if (loadingCanvas != null)
        {
            loadingCanvas.SetActive(false);
        }
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}