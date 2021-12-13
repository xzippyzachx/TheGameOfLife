using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour
{
    public int spawnAmount;

    public TileManager tileManager;
    public List<GameObject> sheep = new List<GameObject>();
    public GameObject sheepPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnSheep(int x, int z)
    {
        if (tileManager.tileIDs[x, z] != 2)
        {            
            float spawnX = tileManager.tilesObjects[x, z].transform.position.x;
            float spawnZ = tileManager.tilesObjects[x, z].transform.position.z;

            GameObject newSheep = Instantiate(sheepPrefab, new Vector3(x - (tileManager.gridSize / 2), 0, z - (tileManager.gridSize / 2)), Quaternion.identity);
            newSheep.GetComponent<Sheep>().tileManager = tileManager;
            newSheep.GetComponent<Sheep>().xPos = x;
            newSheep.GetComponent<Sheep>().zPos = z;

            sheep.Add(newSheep);
        }
        else
        {
            spawnSheep(Random.Range(1, tileManager.gridSize), Random.Range(1, tileManager.gridSize));
        }
    }

    public void Cycle()
    {
        for (int i = 0; i < sheep.Count; i++)
        {
            sheep[i].GetComponent<Sheep>().Cycle();
        }
    }

}
