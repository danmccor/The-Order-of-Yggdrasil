using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    GameManager gameManager;
    public Camera camera;

    public bool holdingUnit = false;
    public bool firstLoop = true;
    public string unitSpawn = null;
    GameObject spawning = null;

    public GameObject currentHold;
    GameObject terrainManager; 
    public List<GameObject> allUnits;
    public List<GameObject> spawnTiles;
    public GameObject freeTile;
    public GameObject unitGameObject;
    public Text text;

    public bool spawningUnit = false;

    public int resources = 500;
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        camera = FindObjectOfType<Camera>();
        allUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));
        spawnTiles = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        allUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));
        if (allUnits.Count == 0)
        {
            gameManager.LoseGame();
        }
        if (gameManager.PlayerTurn)
        {
            int layerMask = 1 << 9;
            RaycastHit hit;
            if (spawningUnit && firstLoop)
            {
                for (int i = 0; i < GameObject.FindGameObjectWithTag("Terrain").GetComponent<TileManager>().Tiles.Count; i++)
                {
                    if (GameObject.FindGameObjectWithTag("Terrain").GetComponent<TileManager>().Tiles[i].gameObject.GetComponent<TileScript>().playerSpawnTile &&
                        GameObject.FindGameObjectWithTag("Terrain").GetComponent<TileManager>().Tiles[i].gameObject.GetComponent<TileScript>().occupied == false)
                    {
                        spawnTiles.Add(Instantiate(freeTile, GameObject.FindGameObjectWithTag("Terrain").GetComponent<TileManager>().Tiles[i].transform.position, Quaternion.Euler(-90, 0, 0)));
                    }
                    firstLoop = false;
                }
            }
            if (currentHold != null)
            {
                holdingUnit = currentHold.GetComponent<UnitControl>().pickedUp;
            }
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                //Debug.Log("Hit a unit!");
                if (hit.transform.gameObject.GetComponent<UnitControl>().turnTaken == false && !holdingUnit)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        currentHold = hit.transform.gameObject;
                        hit.transform.gameObject.GetComponent<UnitControl>().pickedUp = true;
                        holdingUnit = true;
                    }
                }

            }
            if (spawningUnit)
            {
                int layerMask2 = 1 << 8;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask2))
                {
                    if (hit.transform.gameObject.GetComponent<TileScript>().playerSpawnTile)
                    {
                        if (Input.GetMouseButtonUp(0))
                        {
                            spawning = Instantiate(unitGameObject, hit.transform.gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
                            switch (unitSpawn)
                            {
                                case "Soldier":
                                    spawning.GetComponent<unitStats>().Soldier = true;
                                    break;
                                case "Guard":
                                    spawning.GetComponent<unitStats>().Guard = true;
                                    break;
                                default:
                                    spawning.GetComponent<unitStats>().Soldier = true;
                                    break;
                            }
                            spawning.GetComponent<UnitControl>().turnTaken = true;
                            allUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));
                            spawningUnit = false;
                            firstLoop = true;
                            gameManager.PlayerSpawned = true;

                            for (int i = 0; i < spawnTiles.Count; i++)
                            {
                                Destroy(spawnTiles[i]);
                            }
                        }
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("AI is still playing");
        }
        
        text.text = "Resources: " + resources;

        
    }

    public void spawnUnit(string unit)
    {
        if(unit == "Soldier")
        {
            if(resources >= 100)
            {
                resources -= 100;
                unitSpawn = unit;
                spawningUnit = true;
            }
        }
        if (unit == "Guard")
        {
            if (resources >= 150)
            {
                resources -= 150;
                unitSpawn = unit;
                spawningUnit = true;
            }
        }
        if (unit == "Archer")
        {
            if (resources >= 200)
            {
                resources -= 200;
                unitSpawn = unit;
                spawningUnit = true;
            }
        }
        if (unit == "Priest")
        {
            if (resources >= 250)
            {
                resources -= 250;
                unitSpawn = unit;
                spawningUnit = true;
            }
        }
        
    }
    public void EndTurn()
    {
        if (gameManager.PlayerTurn)
        {
            gameManager.PlayerTurn = false;
            gameManager.StartAITurn();
        }
        else
        {
            Debug.Log("Wait your turn!");
        }
    }

   
}
