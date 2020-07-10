using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    public float speed = 2;
    public bool loadingLevel = false;
    AsyncOperation asyncLoad;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Scene Manager").GetComponent<LevelControl>().asyncLoadLevel(2);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), 0, 0);

        if (!transform.GetChild(0).gameObject.GetComponent<AudioSource>().isPlaying && !loadingLevel)
        {
            GameObject.Find("Scene Manager").GetComponent<LevelControl>().allowAysncLoad();
        }
    }


    
}