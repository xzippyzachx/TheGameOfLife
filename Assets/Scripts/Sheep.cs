using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public TileManager tileManager;
    public int xPos;
    public int zPos;

    public int life;
    public int moveChanceAmount;
    public int eatChanceAmount;

    public int cyclesLastBaby;
    public int cyclesLastBabyAmount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cycle()
    {
        if (life <= 0)
            Die();

        int moveChance = Random.Range(1, moveChanceAmount);
        if (moveChance == 1)
            Move();

        CheckFire();

        life--;
        cyclesLastBaby--;

        int R = Random.Range(1, eatChanceAmount);
        if ((tileManager.tileIDs[xPos, zPos] == 0 || tileManager.tileIDs[xPos, zPos] == 4) && R == 1)
            Eat();
    }
    
    public void Die()
    {
        tileManager.sheepManager.sheep.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    public void CheckFire()
    {
        if(tileManager.tileIDs[xPos, zPos] == 3)
        {
            Die();
        }
    }

    public void Eat()
    {
        tileManager.tilesObjects[xPos,zPos].GetComponent<Tile>().SetTile(tileManager.tiles[1]);
        tileManager.tileIDs[xPos, zPos] = 1;
        tileManager.tilesObjects[xPos, zPos].GetComponent<Tile>().tileSelection = 1;

        int R = Random.Range(1, 2);
        if (R == 1 && cyclesLastBaby <= 0)
            Baby();
    }

    public void Baby()
    {
        cyclesLastBaby = cyclesLastBabyAmount;

        int newXPos = -1;
        int newZPos = -1;

        int dir = Random.Range(1, 8);
        if (dir == 1)
        {
            if (xPos - 1 >= 0 && zPos - 1 >= 0 && tileManager.tileIDs[xPos - 1, zPos - 1] != 2)
            {
                newXPos = xPos - 1;
                newZPos = zPos - 1;
            }
        }
        if (dir == 2)
        {
            if (xPos - 1 >= 0 && tileManager.tileIDs[xPos - 1, zPos] != 2)
            {
                newXPos = xPos - 1;
                newZPos = zPos;
            }
        }
        if (dir == 3)
        {
            if (xPos - 1 >= 0 && zPos + 1 < tileManager.gridSize && tileManager.tileIDs[xPos - 1, zPos + 1] != 2)
            {
                newXPos = xPos - 1;
                newZPos = zPos + 1;
            }
        }
        if (dir == 4)
        {

            if (zPos - 1 >= 0 && tileManager.tileIDs[xPos, zPos - 1] != 2)
            {
                newXPos = xPos;
                newZPos = zPos - 1;
            }
        }
        if (dir == 5)
        {
            if (zPos + 1 < tileManager.gridSize && tileManager.tileIDs[xPos, zPos + 1] != 2)
            {
                newXPos = xPos;
                newZPos = zPos + 1;
            }
        }
        if (dir == 6)
        {
            if (xPos + 1 < tileManager.gridSize && zPos - 1 >= 0 && tileManager.tileIDs[xPos + 1, zPos - 1] != 2)
            {
                newXPos = xPos + 1;
                newZPos = zPos - 1;
            }
        }
        if (dir == 7)
        {
            if (xPos + 1 < tileManager.gridSize && tileManager.tileIDs[xPos + 1, zPos] != 2)
            {
                newXPos = xPos + 1;
                newZPos = zPos;
            }
        }
        if (dir == 8)
        {
            if (xPos + 1 < tileManager.gridSize && zPos + 1 < tileManager.gridSize && tileManager.tileIDs[xPos + 1, zPos + 1] != 2)
            {
                newXPos = xPos + 1;
                newZPos = zPos + 1;
            }
        }

        if (newXPos != -1 && newZPos != -1)
        {
            xPos = newXPos;
            zPos = newZPos;

            tileManager.sheepManager.spawnSheep(newXPos, newZPos);

        }
    }

    public void Move()
    {
        int newXPos = -1;
        int newZPos = -1;

        int dir = Random.Range(1, 8);
        if (dir == 1)
        {            
            if (xPos - 1 >= 0 && zPos - 1 >= 0 && tileManager.tileIDs[xPos-1,zPos-1] != 2)
            {
                newXPos = xPos - 1;
                newZPos = zPos - 1;
            }            
        }
        if (dir == 2)
        {           
            if (xPos - 1 >= 0 && tileManager.tileIDs[xPos - 1, zPos] != 2)
            {
                newXPos = xPos - 1;
                newZPos = zPos;
            }
        }
        if (dir == 3)
        {           
            if (xPos - 1 >= 0 && zPos + 1 < tileManager.gridSize && tileManager.tileIDs[xPos - 1, zPos + 1] != 2)
            {
                newXPos = xPos - 1;
                newZPos = zPos + 1;
            }
        }
        if (dir == 4)
        {
            
            if (zPos - 1 >= 0 && tileManager.tileIDs[xPos, zPos - 1] != 2)
            {
                newXPos = xPos;
                newZPos = zPos - 1;
            }
        }
        if (dir == 5)
        {            
            if (zPos + 1 < tileManager.gridSize && tileManager.tileIDs[xPos, zPos + 1] != 2)
            {
                newXPos = xPos;
                newZPos = zPos + 1;
            }
        }
        if (dir == 6)
        {            
            if (xPos + 1 < tileManager.gridSize && zPos - 1 >= 0 && tileManager.tileIDs[xPos + 1, zPos - 1] != 2)
            {
                newXPos = xPos + 1;
                newZPos = zPos - 1;
            }
        }
        if (dir == 7)
        {          
            if (xPos + 1 < tileManager.gridSize && tileManager.tileIDs[xPos + 1, zPos] != 2)
            {
                newXPos = xPos + 1;
                newZPos = zPos;
            }
        }
        if (dir == 8)
        {            
            if (xPos + 1 < tileManager.gridSize && zPos + 1 < tileManager.gridSize && tileManager.tileIDs[xPos + 1, zPos + 1] != 2)
            {
                newXPos = xPos + 1;
                newZPos = zPos + 1;
            }
        }

        if (newXPos != -1 && newZPos != -1)
        {
            xPos = newXPos;
            zPos = newZPos;

            float moveX = tileManager.tilesObjects[xPos, zPos].transform.position.x;
            float moveZ = tileManager.tilesObjects[xPos, zPos].transform.position.z;

            transform.position = new Vector3(newXPos - (tileManager.gridSize / 2), 0, newZPos - (tileManager.gridSize / 2));
            
        }

    }

}
