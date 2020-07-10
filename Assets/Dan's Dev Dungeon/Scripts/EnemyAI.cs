using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public List<GameObject> enemyTiles;
    public List<GameObject> enemyUnits;
    public GameManager gameManager;
    public TileManager tileManager;
    bool takingTurn = false;
    public bool FastTurns = false;
    public int resources = 50;
    GameObject Spawning = null;
    public GameObject EnemyUnit;
    public GameObject TargetUnit;
    public GameObject EnemyBoss;

    GameObject lastTile;

    int enemyTurnCount = 0;
    public bool SpawningUnit = false;
    // Start is called before the first frame update
    void Start()
    {
        EnemyBoss = GameObject.FindGameObjectWithTag("Boss");


        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        enemyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("enemyUnit"));
        tileManager = GameObject.FindGameObjectWithTag("Terrain").gameObject.GetComponent<TileManager>();
        for (int i = 0; i < tileManager.Tiles.Count - 1; i++)
        {
            if (tileManager.Tiles[i].GetComponent<TileScript>().enemySpawnTile)
            {
                enemyTiles.Add(tileManager.Tiles[i]);
            }
        }

        
     
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyBoss == null)
        {
            gameManager.WinGame();
        }
        if (Camera.main.GetComponent<PlayerControls>().allUnits.Count > 0)
        {
            TargetUnit = Camera.main.GetComponent<PlayerControls>().allUnits[0];
        }
        if (gameManager.PlayerTurn == false)
        {
            if (takingTurn == false)
            {
                takingTurn = true;
                if (!FastTurns)
                {
                    StartCoroutine(TakeTurns(1));
                }
                else
                {
                    StartCoroutine(TakeTurns(0));
                }
            }
            if (SpawningUnit == false)
            {
                SpawnUnit();
            }
            if (takingTurn == false)
            {
                enemyTurnCount++;
                EndTurn();
            }
        }
    }


    public void EndTurn()
    {
        gameManager.PlayerTurn = true;
        gameManager.StartPlayerTurn();
    }

    IEnumerator WaitTime(int time, GameObject Unit)
    {
        //Debug.Log("Waiting...");
        yield return new WaitForSeconds(5);
    }

    //public void TakeTurn()
    //{
    //    foreach (GameObject Unit in enemyUnits)
    //    {
    //        if (!Unit.GetComponent<UnitControl>().turnTaken)
    //        {
    //            Unit.transform.GetChild(0).gameObject.SetActive(true);
    //            for (int j = 0; j < Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles.Length; j++)
    //            {
    //                if (Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j] != null && Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupied == true && Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().enemyOccupied == false)
    //                {
    //                    if (Unit.GetComponent<unitStats>().AttackUnit(Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupiedObj))
    //                    {
    //                        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = false;
    //                        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = false;
    //                        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = null;
    //                        Unit.GetComponent<UnitControl>().currentTile = Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j];
    //                        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = true;
    //                        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = true;
    //                        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = Unit.gameObject;
    //                        Unit.GetComponent<UnitControl>().turnTaken = true;
    //                    }
    //                    else
    //                    {
    //                        Unit.GetComponent<UnitControl>().turnTaken = true;
    //                    }
    //                }
    //                else if (Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j] != null && Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupied == false)
    //                {
    //                    Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = false;
    //                    Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = false;
    //                    Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = null;
    //                    Unit.GetComponent<UnitControl>().currentTile = Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j];
    //                    Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = true;
    //                    Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = true;
    //                    Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = Unit.gameObject;
    //                    Unit.GetComponent<UnitControl>().turnTaken = true;
    //                }
    //            }
    //            Unit.transform.GetChild(0).gameObject.SetActive(false);
    //            Unit.GetComponent<UnitControl>().turnTaken = true;
    //        }
    //    }
    //}



    public void SpawnUnit()
    {
        SpawningUnit = true;

        if (enemyTurnCount % 3 == 0)
        {
            int RandomTile = Random.Range(0, enemyTiles.Count);
            //Debug.Log("Enemy Spawn Tile: " + enemyTiles[RandomTile].GetComponent<TileScript>().enemySpawnTile + " Tile Not Occupied: " + enemyTiles[RandomTile].GetComponent<TileScript>().occupied);
            if (enemyTiles[RandomTile].GetComponent<TileScript>().enemySpawnTile && !enemyTiles[RandomTile].GetComponent<TileScript>().occupied)
            {
                Spawning = Instantiate(EnemyUnit, enemyTiles[RandomTile].transform.position, Quaternion.Euler(-90, 0, 0));
                int unitSpawn = Random.Range(0, 4);
                switch (unitSpawn)
                {
                    case 0:
                        Spawning.GetComponent<unitStats>().Soldier = true;
                        break;
                    case 1:
                        Spawning.GetComponent<unitStats>().Guard = true;
                        break;
                    //case 2:
                    //    Spawning.GetComponent<unitStats>().Archer = true;
                    //    break;
                    //case 3:
                    //    Spawning.GetComponent<unitStats>().Priest = true;
                    //    break;
                    default:
                        Spawning.GetComponent<unitStats>().Soldier = true;
                        break;
                }
                Spawning.GetComponent<UnitControl>().turnTaken = true;
                Spawning.GetComponent<UnitControl>().currentTile = enemyTiles[RandomTile];
                enemyUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("enemyUnit"));
                enemyTiles[RandomTile].GetComponent<TileScript>().occupied = true;
                enemyTiles[RandomTile].GetComponent<TileScript>().enemyOccupied = true;
                enemyTiles[RandomTile].GetComponent<TileScript>().occupiedObj = Spawning;

            }

        }
    }
    IEnumerator TakeTurns(int time)
    {
        foreach (GameObject Unit in enemyUnits)
        {
            if (!Unit.GetComponent<UnitControl>().turnTaken)
            {
                Unit.transform.GetChild(0).gameObject.SetActive(true);
                yield return new WaitForSeconds(time);
                GameObject BestTile = null;
                float minDist = Mathf.Infinity;
                bool possibleToMove = false;

                foreach (GameObject tile in Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles)
                {
                    if (!tile.GetComponent<TileScript>().occupied)
                    {
                        possibleToMove = true;
                    }
                }

                if (possibleToMove)
                {
                    for (int j = 0; j < Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles.Count; j++)
                    {
                        lastTile = Unit.GetComponent<UnitControl>().currentTile;
                        if (Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j] != null && Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupied == true && Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().enemyOccupied == false && Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j] != lastTile && !Unit.CompareTag("Boss"))
                        {
                            Unit.GetComponent<UnitControl>().lookTowards(Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupiedObj);
                            if (Unit.GetComponent<unitStats>().AttackUnit(Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupiedObj))
                            {
                                float dist = Vector3.Distance(TargetUnit.GetComponent<UnitControl>().currentTile.transform.position, Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].transform.position);
                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    BestTile = Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j];
                                    break;
                                }
                            }
                            else
                            {
                                Unit.GetComponent<UnitControl>().turnTaken = true;
                                break;
                            }

                        }
                        else if (Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j] != null && Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupied == false)
                        {
                            if (TargetUnit != null)
                            {
                                float dist = Vector3.Distance(TargetUnit.GetComponent<UnitControl>().currentTile.transform.position, Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].transform.position);

                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    BestTile = Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j];
                                    Unit.GetComponent<UnitControl>().lookTowards(BestTile);
                                }
                            }
                        }
                    }
                    if (Unit.GetComponent<UnitControl>().turnTaken == false)
                    {
                        moveUnit(Unit, BestTile);

                    }
                    Unit.transform.GetChild(0).gameObject.SetActive(false);
                    Unit.GetComponent<UnitControl>().turnTaken = true;
                }
                else
                {
                    Unit.GetComponent<UnitControl>().turnTaken = true;
                    Unit.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        for (int j = 0; j < EnemyBoss.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles.Count; j++)
        {
            if (EnemyBoss.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupied)
            {
                EnemyBoss.GetComponent<unitStats>().AttackUnit(EnemyBoss.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupiedObj);
            }
            EnemyBoss.GetComponent<UnitControl>().turnTaken = true;
        }
        takingTurn = false;
    }


    public void moveUnit(GameObject Unit, GameObject newTile)
    {
        Unit.GetComponent<UnitControl>().lookTowards(newTile);
        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = false;
        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = false;
        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = null;
        AudioManager.AudioPlayer.PlayAudio("Step");
        Unit.GetComponent<UnitControl>().currentTile = newTile;
        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = true;
        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = true;
        Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = Unit.gameObject;
        Unit.GetComponent<UnitControl>().turnTaken = true;
        
    }
}



/*
 * if (Unit.GetComponent<unitStats>().AttackUnit(Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j].GetComponent<TileScript>().occupiedObj))
                        {
                            Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = false;
                            Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = false;
                            Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = null;
                            lastTile = Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j];
                            Unit.GetComponent<UnitControl>().currentTile = Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().touchingTiles[j];
                            Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupied = true;
                            Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().enemyOccupied = true;
                            Unit.GetComponent<UnitControl>().currentTile.GetComponent<TileScript>().occupiedObj = Unit.gameObject;
                            Unit.GetComponent<UnitControl>().turnTaken = true;
                            break;
                        }
 */
