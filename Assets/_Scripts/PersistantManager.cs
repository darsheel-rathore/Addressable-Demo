using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class PersistantManager : MonoBehaviour
{
    [SerializeField] public AssetReference firstLevel;
    [SerializeField] public AssetReference secondLevel;
    [SerializeField] public AssetReference mainMenu;
    [SerializeField] private AssetReference bgm_audioClip;
    [SerializeField] public int score;

    public static PersistantManager instance;

    // const
    private const string onSuccessMsg = "Scene Loaded Successfully!!!";
    private const string onFailureMsg = "Scene Loading Failed!!!";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        Addressables.InitializeAsync(this);
        
        // Load Audio
        if (gameObject.GetComponent<AudioSource>() == null)
        {
            bgm_audioClip.LoadAssetAsync<AudioClip>().Completed += (obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    var audioSource = gameObject.AddComponent<AudioSource>();
                    audioSource.clip = obj.Result;
                    audioSource.volume = 0.2f;
                    audioSource.loop = true;
                    audioSource.playOnAwake = false;
                    audioSource.Play();
                }
            };
        }
    }

    public void CalculateCurrentScore(int value) => score += value;

    public AsyncOperationHandle<SceneInstance> LoadLevel(AssetReference levelToLoad, Action<string> onSuccess, Action<string>onFailure)
    {
        AsyncOperationHandle<SceneInstance> handle = levelToLoad.LoadSceneAsync();

        handle.Completed += (obj) => {
            if (obj.Status == AsyncOperationStatus.Succeeded)
                onSuccess(onSuccessMsg);
            else
                onFailure(onFailureMsg + " " + obj.OperationException);
        };

        return handle;
    }
}
