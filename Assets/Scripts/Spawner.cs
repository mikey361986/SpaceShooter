using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField]
    private GameObject[] enemyPrefab;
    [SerializeField]
    private TMP_Text messageTxt;
    [SerializeField]
    private TMP_Text levelTxt;
    [SerializeField]
    private TMP_Text phaseTxt;
    [SerializeField]
    private int level;
    [SerializeField]
    private float timeRatio;
    [SerializeField]
    private int numEnemies;


    public float TimeRatio { get => timeRatio; set => timeRatio = value; }
    public int NumEnemies { get => numEnemies; set => numEnemies = value; }
    public int Level { get => level; set => level = value; }



    public IEnumerator Spawn(int level, int phases, int numEnemies, float timeRatioL)
    {


            for (int j = 0; j < phases; j++)
            {
                levelTxt.text = level.ToString();
                phaseTxt.text = (j + 1).ToString();
                messageTxt.text = "Nivel: "+levelTxt.text + " Fase: "+ phaseTxt.text;
                yield return new WaitForSeconds(1f);
                messageTxt.text = "";

                for (int k = 0; k < numEnemies; k++)
                {

                Vector3 randomPoint = new Vector3(transform.position.x, Random.Range(-4f, 4f));


                    GameObject enemy= Instantiate(enemyPrefab[level-1], randomPoint, Quaternion.identity);
                    enemy.GetComponent<Enemy>().SetEnemyByLevel(level);
        
                    Destroy(enemy, 5f);

                    yield return new WaitForSeconds(timeRatioL);

                }
                yield return new WaitForSeconds(2f);
            }


    }

}


