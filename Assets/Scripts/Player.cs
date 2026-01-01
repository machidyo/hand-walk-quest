using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int point = 0;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        // todo: 取得パーティクル
        SoundManager.Instance.PlaySound(SoundManager.SoundNames.Coin);
        point++;
        Destroy(other.gameObject);
        Debug.Log($"Get Coin, Point: {point}");
    }
}
