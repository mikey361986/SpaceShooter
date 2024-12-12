using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletEnemy : MonoBehaviour
{
    [SerializeField]
    private float velocity;
    [SerializeField]
    private Vector3 direction;

    
    private ObjectPool<BulletEnemy> myPool;

    public ObjectPool<BulletEnemy> MyPool { get => myPool; set => myPool = value; }

    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * velocity * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer > 4f)
        {
            myPool.Release(this);
            timer = 0;
        }

    }
}
