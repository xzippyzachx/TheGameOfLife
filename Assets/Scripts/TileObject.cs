using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MonoBehaviour
{
    public int spreadChance;
    public int[] spreadable;
    public bool isSpreadable;
    public int life;
    public int spreadID;

    public bool burnable;
    public int burnChance;
    public int burnAmount;
    public int burnTick;

}
