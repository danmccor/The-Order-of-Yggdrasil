using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tiles;
    public List<GameObject> Tiles;
    // Start is called before the first frame update
    void Awake()
    {
       

        Tiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("Hexagon"));
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
