using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public int gridSize = 10;
    public int waterChance = 10;

    public SheepManager sheepManager;
    public int[,] tileIDs;
    public GameObject[,] tilesObjects;
    public GameObject[] tiles;
    public GameObject startTile;

    public GameObject tornadoPrefab;
    public List<GameObject> tornados = new List<GameObject>();

    public int tileSelection;

    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        tileIDs = new int[gridSize, gridSize];
        tilesObjects = new GameObject[gridSize, gridSize];
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if(Input.GetMouseButton(0) && hit.transform.tag == "Tile" && hit.transform.gameObject.GetComponent<Tile>().tileSelection != tileSelection && hit.transform.gameObject.GetComponent<Tile>().tileSelection != 2)
            {
                hit.transform.gameObject.GetComponent<Tile>().tileSelection = tileSelection;
                hit.transform.gameObject.GetComponent<Tile>().SetTile(tiles[tileSelection]);
                tileIDs[hit.transform.gameObject.GetComponent<Tile>().xPos,hit.transform.gameObject.GetComponent<Tile>().zPos] = tileSelection;
            }
        }
    }

    public void SpawnTornado(int x, int z)
    {
        float spawnX = tilesObjects[x, z].transform.position.x;
        float spawnZ = tilesObjects[x, z].transform.position.z;

        GameObject newTornado = Instantiate(tornadoPrefab, new Vector3(x - (gridSize / 2), 0, z - (gridSize / 2)), Quaternion.identity);
        newTornado.GetComponent<Tornado>().tileManager = GetComponent<TileManager>();
        tornados.Add(newTornado);
    }

    void CreateGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize;  z++)
            {
                GameObject newTile = Instantiate(startTile, new Vector3(x-(gridSize / 2), 0, z - (gridSize / 2)), Quaternion.identity);
                if (Random.Range(0, waterChance) == 0)
                {
                    newTile.GetComponent<Tile>().SetTile(tiles[2]);
                    tileIDs[x,z] = 2;
                    newTile.GetComponent<Tile>().tileSelection = 2;
                }
                else
                {
                    tileIDs[x,z] = 1;                    
                    newTile.GetComponent<Tile>().tileSelection = 1;
                }
                tilesObjects[x,z] = newTile;
                newTile.GetComponent<Tile>().xPos = x;
                newTile.GetComponent<Tile>().zPos = z;
                newTile.transform.parent = this.gameObject.transform;                
            }
        }

        for (int i = 0; i < sheepManager.spawnAmount; i++)
        {
            sheepManager.spawnSheep(Random.Range(1, gridSize), Random.Range(1, gridSize));
        }       

    }

    public void CheckTiles()
    {
        for (int i = 0; i < tornados.Count; i++)
        {
            tornados[i].GetComponent<Tornado>().Cycle();
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (tilesObjects[x,z].GetComponent<Tile>().currentTile.GetComponent<TileObject>().isSpreadable == true)
                {
                    Spread(tileIDs[x, z], x, z);
                    
                }
                if(tilesObjects[x, z].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnable == true)
                {
                    Fire(x, z);
                }
                if (tilesObjects[x, z].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnTick > 0)
                {
                    Burn(x, z);
                }
                Life(x, z);                
              
            }
        }
    }
    public void Life(int _xPos, int _zPos)
    {
        tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().life++;
        if (tileIDs[_xPos, _zPos] == 0)
        {
            if (tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().life >= 30)
            {
                tilesObjects[_xPos, _zPos].GetComponent<Tile>().tileSelection = 4;
                tilesObjects[_xPos, _zPos].GetComponent<Tile>().SetTile(tiles[4]);
                tileIDs[_xPos, _zPos] = 4;
            }
        }
        if (tileIDs[_xPos, _zPos] == 4)
        {
            
            if (tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().life >= 100 && sheepManager.sheep.Count < 5)
            {
                sheepManager.spawnSheep(_xPos, _zPos);
            }
            
        }
    }

    public void Fire(int _xPos, int _zPos)
    {
        int burnChance = tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnChance;
        int chance = Random.Range(1, 2000);
        
        if (chance <= burnChance && !CheckWater(_xPos, _zPos))
        {
            int newBurnAmount = tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnAmount;
            
            tilesObjects[_xPos, _zPos].GetComponent<Tile>().tileSelection = 3;
            tilesObjects[_xPos, _zPos].GetComponent<Tile>().SetTile(tiles[3]);            
            tileIDs[_xPos, _zPos] = 3;
            tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnTick = newBurnAmount;
        }
    }

    public void Burn(int _xPos, int _zPos)
    {
        tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnTick--;
        if(tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnTick == 0)
        {
            tilesObjects[_xPos, _zPos].GetComponent<Tile>().tileSelection = 1;
            tilesObjects[_xPos, _zPos].GetComponent<Tile>().SetTile(tiles[1]);
            tileIDs[_xPos, _zPos] = 1;
        }

    }

    public bool CheckWater(int _xPos, int _zPos)
    {
        int newXPos = -1;
        int newZPos = -1;

        for (int dir = 1; dir <= 8; dir++)
        {
            if (dir == 1)
            {
                if (_xPos - 1 >= 0 && _zPos - 1 >= 0)
                {
                    newXPos = _xPos - 1;
                    newZPos = _zPos - 1;
                }
            }
            if (dir == 2)
            {
                if (_xPos - 1 >= 0)
                {
                    newXPos = _xPos - 1;
                    newZPos = _zPos;
                }
            }
            if (dir == 3)
            {
                if (_xPos - 1 >= 0 && _zPos + 1 < gridSize)
                {
                    newXPos = _xPos - 1;
                    newZPos = _zPos + 1;
                }
            }
            if (dir == 4)
            {

                if (_zPos - 1 >= 0)
                {
                    newXPos = _xPos;
                    newZPos = _zPos - 1;
                }
            }
            if (dir == 5)
            {
                if (_zPos + 1 < gridSize)
                {
                    newXPos = _xPos;
                    newZPos = _zPos + 1;
                }
            }
            if (dir == 6)
            {
                if (_xPos + 1 < gridSize && _zPos - 1 >= 0)
                {
                    newXPos = _xPos + 1;
                    newZPos = _zPos - 1;
                }
            }
            if (dir == 7)
            {
                if (_xPos + 1 < gridSize)
                {
                    newXPos = _xPos + 1;
                    newZPos = _zPos;
                }
            }
            if (dir == 8)
            {
                if (_xPos + 1 < gridSize && _zPos + 1 < gridSize)
                {
                    newXPos = _xPos + 1;
                    newZPos = _zPos + 1;
                }
            }

            if (newXPos != -1 && newZPos != -1)
            {
                if (tileIDs[newXPos, newZPos] == 2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Spread(int _tileID, int _xPos, int _zPos)
    {
        int spreadChance = tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().spreadChance;
        int spreadID = tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().spreadID;
        int[] spreadable = tilesObjects[_xPos, _zPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().spreadable;
        int chance = Random.Range(1, 100);

        int newXPos = -1;
        int newZPos = -1;

        if (chance <= spreadChance)
        {
            int dir = Random.Range(1, 8);
            if(dir == 1)
            {
                for (int i = 0; i < spreadable.Length; i++)
                {
                    if(_xPos - 1 >= 0 && _zPos - 1 >= 0 && tileIDs[_xPos-1, _zPos-1] == spreadable[i])
                    {
                        newXPos = _xPos - 1;
                        newZPos = _zPos - 1;                     
                    }
                }
            }
            if (dir == 2)
            {
                for (int i = 0; i < spreadable.Length; i++)
                {
                    if (_xPos - 1 >= 0 && tileIDs[_xPos - 1, _zPos] == spreadable[i])
                    {
                        newXPos = _xPos - 1;
                        newZPos = _zPos;
                    }
                }
            }
            if (dir == 3)
            {
                for (int i = 0; i < spreadable.Length; i++)
                {
                    if (_xPos - 1 >= 0 && _zPos + 1 < gridSize && tileIDs[_xPos - 1, _zPos + 1] == spreadable[i])
                    {
                        newXPos = _xPos - 1;
                        newZPos = _zPos + 1;
                    }
                }
            }
            if (dir == 4)
            {
                for (int i = 0; i < spreadable.Length; i++)
                {
                    if (_zPos - 1 >= 0 && tileIDs[_xPos, _zPos - 1] == spreadable[i])
                    {
                        newXPos = _xPos;
                        newZPos = _zPos - 1;
                    }
                }
            }
            if (dir == 5)
            {
                for (int i = 0; i < spreadable.Length; i++)
                {
                    if (_zPos + 1 < gridSize && tileIDs[_xPos, _zPos + 1] == spreadable[i])
                    {
                        newXPos = _xPos;
                        newZPos = _zPos + 1;
                    }
                }
            }
            if (dir == 6)
            {
                for (int i = 0; i < spreadable.Length; i++)
                {
                    if (_xPos + 1 < gridSize && _zPos - 1 >= 0 && tileIDs[_xPos + 1, _zPos - 1] == spreadable[i])
                    {
                        newXPos = _xPos + 1;
                        newZPos = _zPos - 1;
                    }
                }
            }
            if (dir == 7)
            {
                for (int i = 0; i < spreadable.Length; i++)
                {
                    if (_xPos + 1 < gridSize && tileIDs[_xPos + 1, _zPos] == spreadable[i])
                    {
                        newXPos = _xPos + 1;
                        newZPos = _zPos;
                    }
                }
            }
            if (dir == 8)
            {
                for (int i = 0; i < spreadable.Length; i++)
                {
                    if (_xPos + 1 < gridSize && _zPos + 1 < gridSize && tileIDs[_xPos + 1, _zPos + 1] == spreadable[i])
                    {
                        newXPos = _xPos + 1;
                        newZPos = _zPos + 1;
                    }
                }
            }

            if (newXPos != -1 && newZPos != -1)
            {
                int newBurnAmount = tilesObjects[newXPos, newZPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnAmount;

                tilesObjects[newXPos, newZPos].GetComponent<Tile>().tileSelection = spreadID;
                tilesObjects[newXPos, newZPos].GetComponent<Tile>().SetTile(tiles[spreadID]);
                

                if(spreadID == 3)
                    tilesObjects[newXPos, newZPos].GetComponent<Tile>().currentTile.GetComponent<TileObject>().burnTick = newBurnAmount;

                tileIDs[newXPos, newZPos] = spreadID;
            }
            
        }

    }

}
