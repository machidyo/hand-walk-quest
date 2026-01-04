using System;
using UnityEngine;
using R3;

public class Player : MonoBehaviour
{
    public Subject<Unit> Banana { get; private set; } = new();
    public Subject<Unit> Apple { get; private set; } = new();
    public Subject<String> Item { get; private set; } = new();
    
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Item"))
        {
            // todo: 取得パーティクル
            Destroy(other.gameObject);
            Item.OnNext(other.gameObject.name);
        }
        if (other.gameObject.name.Contains("Banana"))
        {
            Banana.OnNext(Unit.Default);
        }
        if (other.gameObject.name.Contains("Apple"))
        {
            Apple.OnNext(Unit.Default);
        }
    }
}
