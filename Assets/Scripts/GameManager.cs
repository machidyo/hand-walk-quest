using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private VPSLocalization vpsLocalization;
    [SerializeField] private Player player;

    [SerializeField] private GameObject startBanana;
    [SerializeField] private GameObject goalApple;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private GameObject bagPrefab;
    [SerializeField] private GameObject jewelPrefab;
    [SerializeField] private List<GameObject> itemPoints;

    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI statusText;
    
    private CompositeDisposable disposables = new();
    
    private int point = 0;
    private int bigCount = 0;
    
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
                    bigCount++;
                }
                if (item.Contains("Bag"))
                {
                    point += 3;
                    bigCount++;
                }
                if (item.Contains("Jewel"))
                {
                    point += 5;
                    bigCount++;
                }
                // todo: 得点追加表示
                pointText.text = $"Point: {point}";
                if (point >= 5)
                {
                    goalApple.SetActive(true);
                }

                if (bigCount >= 3)
                {
                    ShowCoins();
                    bigCount = 0;
                }
            }).AddTo(disposables);

        player.Apple
            .Subscribe(async _ =>
            {
                BGM.Instance.FadeOutSound(0.5f).Forget();
                goalApple.SetActive(false);
                HideCoins();
                
                SoundManager.Instance.PlaySound(SoundManager.SoundNames.Win);
                pointText.text = $"You got {point}";
                ShowMessage("You WIN!!!").Forget();

                point = 0;
                await UniTask.Delay(3000);
                startBanana.SetActive(true);
            }).AddTo(disposables);
    }

    private void OnDestroy()
    {
        disposables.Dispose();
    }

    private void ShowCoins()
    {
        foreach (var coin in itemPoints)
        {
            GameObject go = null;
            if (coin.name.Contains("Coin"))
            {
                go = coinPrefab;
            }
            if (coin.name.Contains("Ring"))
            {
                go = ringPrefab;
            }
            if (coin.name.Contains("Bag"))
            {
                go = bagPrefab;
            }
            if (coin.name.Contains("Jewel"))
            {
                go = jewelPrefab;
            }
            Instantiate(go, coin.transform);
        }
    }

    private void HideCoins()
    {
        var items = itemPoints
            .Select(ip => ip.GetComponentInChildren<SphereCollider>())
            .Where(sc => sc != null)
            .Select(sc => sc.gameObject);
        foreach (var item in items)
        {
            Destroy(item);
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
