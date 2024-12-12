using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private AudioSource aSource;
    [SerializeField]
    private AudioClip[] clips;
    [SerializeField]
    private GameObject menuUI;
    [SerializeField]
    private int level = 3;
    [SerializeField]
    private int phases = 2;
    [SerializeField]
    private float timeRatio;
    [SerializeField]
    private int numEnemies;
    [SerializeField]
    private GameObject explotionPrefab;
    private float timer;
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private TMP_Text messageTxt;

    private bool isPaused = false;
    [SerializeField]
    private int enemiesKilled = 0;
    [SerializeField]
    private TMP_Text enemiesKilledTxt;

    [SerializeField]
    private Player playerCtr;
    
    private int score = 0;
    [SerializeField]
    private TMP_Text scoreTxt;

    public int Level { get => level; set => level = value; }
    public float TimeRatio { get => timeRatio; set => timeRatio = value; }
    public int NumEnemies { get => numEnemies; set => numEnemies = value; }
    public int Phases { get => phases; set => phases = value; }
    public int EnemiesKilled { get => enemiesKilled; set => enemiesKilled = value; }
    public TMP_Text MessageTxt { get => messageTxt; set => messageTxt = value; }
    public Player PlayerCtr { get => playerCtr; set => playerCtr = value; }
    public TMP_Text EnemiesKilledTxt { get => enemiesKilledTxt; set => enemiesKilledTxt = value; }




    // Start is called before the first frame update
    void Start()
    {

        aSource = GetComponent<AudioSource>();
        StartCoroutine(SetLevels());
        PauseGame();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }

    }


    public IEnumerator SetLevels()
    {
        yield return StartCoroutine(spawner.Spawn(1, 1, 10, 1f));
        playerCtr.LifeUp();
        yield return StartCoroutine(spawner.Spawn(2, 2, 20, 0.5f));
        playerCtr.LifeUp();
        yield return StartCoroutine(spawner.Spawn(3, 2, 30, 0.3f));
        messageTxt.text = "Has ganado! :D";
        PlaySound(4);
    }



    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            messageTxt.text = "Presiona P para continuar y R para reiniciar";
            PlaySound(3);
        }
        else
        {
            Time.timeScale = 1;
            messageTxt.text = "";
        }

        

    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(0);
    }


    public void PlaySound(int index)
    {
        aSource.PlayOneShot(clips[index]);
    }


    public void OnEnemyKilled()
    {
        
        EnemiesKilled++;
        EnemiesKilledTxt.text = EnemiesKilled.ToString();
        score += 2000;
        scoreTxt.text = score.ToString();
    }

    public void GenerateExplotion(Transform parent)
    {
        GameObject explotion= Instantiate(explotionPrefab, parent.position, Quaternion.identity);
        Destroy(explotion, 1f);
        PlaySound(2);
    }



}
