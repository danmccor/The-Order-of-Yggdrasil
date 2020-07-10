using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{

    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Unit"))
        {
            gameManager.player.GetComponent<PlayerControls>().resources += 50;
            Destroy(this.gameObject);

        }
        if (collision.gameObject.CompareTag("enemyUnit"))
        {
            gameManager.AI.GetComponent<EnemyAI>().resources += 50;
            Destroy(this.gameObject);
        }
        
    }
}
