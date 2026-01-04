using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private VPSLocalization vpsLocalization;
    [SerializeField] private Player player;

    [SerializeField] private GameObject startBanana;
    [SerializeField] private GameObject goalApple;
    [SerializeField] private List<GameObject> coins;

    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI statusText;
    
    private CompositeDisposable disposables = new();
    
    private int point = 0;
    
    void Start()
    {
        startBanana.SetActive(false);
        goalApple.SetActive(false);
        HideCoins();

        vpsLocalization.IsTracked
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                startBanana.SetActive(true);
                ShowMessage("Get Banana").Forget();
            }).AddTo(disposables);

        player.Banana
            .Subscribe(_ =>
            {
                BGM.Instance.PlaySound(BGM.BGMTypes.Playing);
                startBanana.SetActive(false);
                ShowMessage($"GAME START{Environment.NewLine}Get Coins").Forget();
                ShowCoins();
            }).AddTo(disposables);

        player.Item
            .Subscribe(item =>
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundNames.Coin);
                if (item.Contains("Coin"))
                {
                    point += 1;
                }
                if (item.Contains("Ring"))
                {
                    point += 2;
                }
                if (item.Contains("Bag"))
                {
                    point += 3;
                }
                if (item.Contains("Jewel"))
                {
                    point += 5;
                }
                // todo: 得点追加表示
                pointText.text = $"Point: {point}";
                if (point >= 5)
                {
                    goalApple.SetActive(true);
                }
            }).AddTo(disposables);

        player.Apple
            .Subscribe(_ =>
            {
                SoundManager.Instance.PlaySound(SoundManager.SoundNames.Win);
                ShowMessage("You WIN!!!").Forget();
                goalApple.SetActive(false);
            });
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }

    private void ShowCoins()
    {
        foreach (var coin in coins)
        {
            coin.SetActive(true);
        }
    }

    private void HideCoins()
    {
        foreach (var coin in coins)
        {
            coin.SetActive(false);
        }
    }

    private async UniTask ShowMessage(string message, int duration = 3000)
    {
        Debug.Log($"ShowMessage: {message}");
        statusText.gameObject.SetActive(true);
        statusText.text = message;
        await UniTask.Delay(duration);
        statusText.text = string.Empty;
        statusText.gameObject.SetActive(false);
    }
}
