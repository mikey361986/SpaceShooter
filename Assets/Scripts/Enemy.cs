using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float velocity;
    [SerializeField]
    private BulletEnemy bulletPrefab;
    [SerializeField]
    private Transform spawnPoint;
    private GameManager gameManager;
    [SerializeField]
    private Sprite []spritesEnemy;
    [SerializeField]
    private Vector3 direction;

    private bool isDestroyed = false;

    
    //Pool
    private ObjectPool<BulletEnemy> objectPool;


    private void Awake()
    {
        objectPool = new ObjectPool<BulletEnemy>(CreateBullet, GetBullet, ReleaseBullet, DestroyBullet);

    }

    private BulletEnemy CreateBullet()
    {
        BulletEnemy bullet = Instantiate(bulletPrefab, spawnPoint.transform.position, Quaternion.identity);
        bullet.MyPool = objectPool;

        return bullet;
    }

    private void GetBullet(BulletEnemy obj)
    {
        obj.transform.position = spawnPoint.transform.position;
        obj.gameObject.SetActive(true);

    }

    private void ReleaseBullet(BulletEnemy obj)
    {
        obj.gameObject.SetActive(false);

    }
    private void DestroyBullet(BulletEnemy obj)
    {
        Destroy(obj.gameObject);

    }


    /////////////////////////////////////////////////////////
    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnBullet());
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(direction * velocity * Time.deltaTime);

    }


    public void SetEnemyByLevel(int level)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = spritesEnemy[level-1];
    }

    public IEnumerator SpawnBullet()
    {
        while (true && !isDestroyed) {

            /*
            GameObject bullet= Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Destroy(bullet, 10f);
    */        

            objectPool.Get();


            gameManager.PlaySound(1);
            yield return new WaitForSeconds(2f);

        }

    }

    void OnHit()
    {
        gameManager.GenerateExplotion(transform);
        gameManager.OnEnemyKilled();

        if (gameManager.EnemiesKilled > 15 && !gameManager.PlayerCtr.IsInvincible)
        {
            gameManager.MessageTxt.text = "Eres invencible por 3 segundos!!!!";
            StartCoroutine(gameManager.PlayerCtr.Invincible());
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet"))
        {
            
            StartCoroutine(DestroyEnemy(collision));
            OnHit();


        }

    }

    private IEnumerator DestroyEnemy(Collider2D collision)
    {
        isDestroyed = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        //if (collision != null) Destroy(collision.gameObject);
        
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
        

    }



}
