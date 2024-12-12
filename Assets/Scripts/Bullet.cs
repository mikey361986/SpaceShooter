using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float velocity;
    [SerializeField]
    private Vector3 direction;

    
    private ObjectPool<Bullet> myPool;

    public ObjectPool<Bullet> MyPool { get => myPool; set => myPool = value; }

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
            //Debug.LogWarning("Entered ReleaseBullet....");
            //problem was here!!!! :)
            timer = 0;
        }

    }
}
