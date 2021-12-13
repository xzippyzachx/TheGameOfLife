using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject currentTile;
    public int tileSelection;
    public int xPos;
    public int zPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTile(GameObject tile)
    {           

        Destroy(currentTile);
        int R = Random.Range(0, 3);
        int rotationY = 0;
        if (R == 0)
            rotationY = 90;
        if (R == 1)
            rotationY = 180;
        if (R == 2)
            rotationY = 270;
        currentTile = Instantiate(tile, new Vector3(transform.position.x, 0, transform.position.z), tile.transform.rotation);
        currentTile.transform.rotation = Quaternion.Euler(-90, rotationY, 0);
        currentTile.transform.parent = this.gameObject.transform;

    }

    

}
