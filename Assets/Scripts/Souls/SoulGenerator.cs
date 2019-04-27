using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulGenerator : MonoBehaviour
{
    public int ActiveSouls = 0;
    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private int lowerRange;
    [SerializeField] private int upperRange;
    private int delay;
    [SerializeField] List<Transform> SoulSpawns;
    private Transform spawnPoint;
    bool processingCouroutine = false;
    
    // Start is called before the first frame update
    void Start()
    {
        int randIndex = Random.Range(0, SoulSpawns.Count - 1);
        spawnPoint = SoulSpawns[randIndex];
        Instantiate(soulPrefab, spawnPoint.position, spawnPoint.rotation);
        ActiveSouls++;
        StartCoroutine(SpawnSoul());
    }

    // Update is called once per frame
    void Update()
    {
        if (!processingCouroutine && ActiveSouls < 2)
        {
            StartCoroutine(SpawnSoul());
        }
    }

    private IEnumerator SpawnSoul()
    {
        processingCouroutine = true;
        delay = Random.Range(lowerRange, upperRange);
        yield return new WaitForSeconds(delay);
        int randIndex = Random.Range(0, SoulSpawns.Count - 1);
        spawnPoint = SoulSpawns[randIndex];
        Instantiate(soulPrefab, spawnPoint.position, spawnPoint.rotation);
        ActiveSouls++;
        if (ActiveSouls < 2) StartCoroutine(SpawnSoul());
    }
}
