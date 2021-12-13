using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public TileManager tileManager;

    public GameObject parts;
    public float rotateSpeed;

    public int life;
    private float xDir;
    private float zDir;

    // Start is called before the first frame update
    void Start()
    {
        int R = Random.Range(100, 200);
        life = R;

        xDir = Random.Range(-1f, 1f);
        zDir = Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        parts.transform.Rotate(0, 0, rotateSpeed);
        
        if (transform.position.x < -(tileManager.gridSize / 2) || transform.position.x > tileManager.gridSize / 2 || transform.position.z < -(tileManager.gridSize / 2) || transform.position.z > tileManager.gridSize / 2)
            Die();
    }

    public void Cycle()
    {
        DestroyTiles();
        DestroySheep();

        if (life <= 0)
            Die();

        life--;


        transform.position = transform.position + new Vector3 (xDir, 0, zDir);
    }

    public void Die()
    {
        tileManager.tornados.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void DestroyTiles()
    {
        for (int x = 0; x < tileManager.gridSize; x++)
        {
            for (int z = 0; z < tileManager.gridSize; z++)
            {
                float dist = Vector3.Distance(tileManager.tilesObjects[x, z].GetComponent<Tile>().currentTile.transform.position, transform.position);
                if (dist <= 3 && tileManager.tileIDs[x, z] != 2)
                {
                    tileManager.tilesObjects[x, z].GetComponent<Tile>().tileSelection = 1;
                    tileManager.tilesObjects[x, z].GetComponent<Tile>().SetTile(tileManager.tiles[1]);
                    tileManager.tileIDs[x, z] = 1;
                }
            }
        }
    }

    public void DestroySheep()
    {
        for (int i = 0; i < tileManager.sheepManager.sheep.Count; i++)
        {
            float dist = Vector3.Distance(tileManager.sheepManager.sheep[i].transform.position, transform.position);
            if (dist <= 3)
            {
                tileManager.sheepManager.sheep[i].GetComponent<Sheep>().Die();
            }
        }
    }
}
