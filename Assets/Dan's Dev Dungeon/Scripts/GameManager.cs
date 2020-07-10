using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject resource;
    public TileManager allTiles;
    public GameObject terrainManager;
    public GameObject player;
    public GameObject AI;

    public GameObject Victory;
    public GameObject Defeat;

    public bool PlayerTurn = true;
    public bool PlayerSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = terrainManager.GetComponent<TileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.Find("Scene Manager").GetComponent<LevelControl>().QuitGame();
        }
    }

    public void StartPlayerTurn()
    {
        for (int i = 0; i < player.GetComponent<PlayerControls>().allUnits.Count; i++)
        {
            player.GetComponent<PlayerControls>().allUnits[i].GetComponent<UnitControl>().turnTaken = false;
        }
        PlayerSpawned = false;
        SpawnResource();
    }

    public void StartAITurn()
    {
        foreach(GameObject Unit in AI.GetComponent<EnemyAI>().enemyUnits)
        {
            Unit.GetComponent<UnitControl>().turnTaken = false;
        }
        AI.GetComponent<EnemyAI>().SpawningUnit = false;
    }

    public void SpawnResource()
    {
        for(int i = 0; i < allTiles.Tiles.Count; i++)
        {
            if(Random.Range(0, 5000) < 2)
            {
                //Debug.Log(allTiles.tiles[i] + " Spawning resource!");
                Instantiate(resource, allTiles.Tiles[i].transform.position, Quaternion.Euler(-90, 0, 0));

            }
        }

    }

    public void LoseGame()
    {
        // GameObject.Find("Defeat").gameObject.SetActive(true);
        
        Defeat.SetActive(true);
    }

    public void WinGame()
    {
        // GameObject.Find("Victory").gameObject.SetActive(true);

        Victory.SetActive(true);
    }
}
