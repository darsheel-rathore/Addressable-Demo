using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.Services.RemoteConfig;

public class Coin : MonoBehaviour
{
    private int coinValue = 20;
    private const string _PLAYER = "Player";

    // Initial Rotation
    private Vector3 newRotation = new Vector3(0f, 360f, 0f);
    private float tweenDuration = 2f;
    private int infiniteLoop = -1;

    // OnTrigger Rotation
    private float onTriggerAnimDuration = 0.2f;

    private void Awake() => DOTween.Init();

    // Event
    public Action<int> scoreUpdateEvent; 

    void Start()
    {
        //InitialCoinRotation_Loop();

        // Subscribing
        scoreUpdateEvent += PersistantManager.instance.CalculateCurrentScore;
        scoreUpdateEvent += CustomCanvas.instance.UpdateScoreText;
    }

    private void InitialCoinRotation_Loop()
    {
        transform.DORotate(
            newRotation, tweenDuration, RotateMode.LocalAxisAdd).
            SetLoops(infiniteLoop, LoopType.Restart).
            SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == _PLAYER)
        {
            var coinValue = RemoteConfigService.Instance.appConfig.GetInt("COIN", this.coinValue);

            // Raise Event
            scoreUpdateEvent(coinValue);

            Vector3 target = transform.position + new Vector3(0f, 1.5f, 0);
            transform.DOMove(target, onTriggerAnimDuration).SetEase(Ease.Linear).onComplete += () =>
            {

                transform.DOScale(Vector3.zero, onTriggerAnimDuration).SetEase(Ease.InBounce).onComplete += () =>
                {
                    gameObject.SetActive(false);
                };
            };
        }
    }
}
