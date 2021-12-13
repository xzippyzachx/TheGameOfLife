using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCycle : MonoBehaviour
{
    public float cycleSpeed;
    public int Day;
    public Text dayCount;
    public Text speed;

    public Slider speedSlider;

    public SheepManager SheepManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cycle());
    }

    // Update is called once per frame
    void Update()
    {
        dayCount.text = Day.ToString();
        cycleSpeed  = Mathf.Round(cycleSpeed * 10000f) / 10000f;
        speed.text = cycleSpeed.ToString();
        cycleSpeed = speedSlider.GetComponent<Slider>().value;
    }

    IEnumerator Cycle()
    {
        yield return new WaitForSeconds(cycleSpeed);
        Day++;
        GetComponent<TileManager>().CheckTiles();
        SheepManager.Cycle();
        StartCoroutine(Cycle());

        int R = Random.Range(1, 200);
        if (R == 1)
            GetComponent<TileManager>().SpawnTornado(Random.Range(1, GetComponent<TileManager>().gridSize), Random.Range(1, GetComponent<TileManager>().gridSize));
    }
}
