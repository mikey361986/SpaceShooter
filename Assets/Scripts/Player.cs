using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float velocity;
    [SerializeField]
    private Bullet bulletPrefab;
    [SerializeField]
    private Transform spawnPoint1;
    [SerializeField]
    private Transform spawnPoint2;
    [SerializeField]
    private float shootRatio;
    private float timer=0f;

    private int energy=100;
    [SerializeField]
    private Slider energyValue;

    private GameManager gameManager;
    private bool isInvincible = false;
    [SerializeField]
    private GameObject light2D;

    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }


    //Pool
    private ObjectPool<Bullet> objectPool;
    private ObjectPool<Bullet> objectPool2;



    private void Awake()
    {
        objectPool = new ObjectPool<Bullet>(CreateBullet, GetBullet, ReleaseBullet, DestroyBullet);
        objectPool2 = new ObjectPool<Bullet>(CreateBullet2, GetBullet2, ReleaseBullet, DestroyBullet);

    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, spawnPoint1.transform.position, Quaternion.identity);
        bullet.MyPool = objectPool;
        return bullet;
    }
    private Bullet CreateBullet2()
    {
        Bullet bullet = Instantiate(bulletPrefab, spawnPoint2.transform.position, Quaternion.identity);
        bullet.MyPool = objectPool2;
        return bullet;
    }

    private void GetBullet(Bullet obj)
    {
        obj.transform.position = spawnPoint1.transform.position;
        obj.gameObject.SetActive(true);
    }
    private void GetBullet2(Bullet obj)
    {
        obj.transform.position = spawnPoint2.transform.position;
        obj.gameObject.SetActive(true);
    }

    private void ReleaseBullet(Bullet obj)
    {
        obj.gameObject.SetActive(false);
    }
    private void DestroyBullet(Bullet obj)
    {
        Destroy(obj.gameObject);

    }


    /////////////////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {

        Movement();
        LimitMovement();
        Shoot();

    }

    public void Movement()
    {
        //input
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        //Translate through time
        transform.Translate(new Vector3(inputH, inputV).normalized * velocity * Time.deltaTime);

    }


    public void LimitMovement()
    {
        //Bounding player
        float xClamped = Mathf.Clamp(transform.position.x, -8f, 8f);
        float yClamped = Mathf.Clamp(transform.position.y, -4f, 4f);
        transform.position = new Vector3(xClamped, yClamped, 0);

    }

    public void Shoot()
    {
        timer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && timer>=shootRatio)
        {
            objectPool.Get();
            objectPool2.Get();
            /*
            GameObject bullet1= Instantiate(bulletPrefab, spawnPoint1.transform.position, Quaternion.identity);
            GameObject bullet2 = Instantiate(bulletPrefab, spawnPoint2.transform.position, Quaternion.identity);

            Destroy(bullet1, 10f);
            Destroy(bullet2, 10f);
            */

            timer = 0;
            gameManager.PlaySound(0);
        }
    }
    //Power UP
    public IEnumerator Invincible()
    {
        gameManager.PlaySound(4);
        isInvincible = true;
        GetComponent<BoxCollider2D>().enabled = false;
        light2D.SetActive(true);
        yield return new WaitForSeconds(3f);
        light2D.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = true;
        gameManager.MessageTxt.text = "";

    }

    void OnHit()
    {
        energy -= 20;
        

        if (energy <= 0)
        {
            energy = 0;
            gameManager.PlaySound(2);
            Destroy(this.gameObject,0.5f);
            gameManager.ResetLevel();
            
        }

        energyValue.value = energy/100f;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Enemy") || collision.CompareTag("BulletEnemy"))
        {
            OnHit();
            Destroy(collision.gameObject);
            gameManager.GenerateExplotion(transform);

        }

    }
    //Power UP
    public void LifeUp()
    {
        energy += 20;
        energyValue.value =energy;
        gameManager.PlaySound(4);
    }


}
