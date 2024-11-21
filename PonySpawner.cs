using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PonySpawner : MonoBehaviour
{
    public int initialPoniesPerWave = 5;
    public int currentPoniesPerWave;

    public float spawnDelay = 0.5f; // Delay between spawning each pony in a wave

    public int currentWave = 0;
    public float waveCooldown = 10.0f; // Time in seconds between waves

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List<PonyAI> currentPoniesAlive;

    public List<GameObject> ponyPrefabs; // List of pony prefabs

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;

    public TextMeshProUGUI currentWaveUI; 

    private void Start()
    {
        currentPoniesPerWave = initialPoniesPerWave;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentPoniesAlive.Clear();

        currentWave++;
        currentWaveUI.text = "Wave : " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentPoniesPerWave; i++)
        {
            // Generate a random offset within a specified range
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            // Randomly select a pony prefab from the list
            GameObject selectedPrefab = ponyPrefabs[Random.Range(0, ponyPrefabs.Count)];

            // Instantiate the selected pony
            var pony = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

            // Get Enemy Script
            PonyAI ponyScript = pony.GetComponent<PonyAI>();

            // Track this pony
            currentPoniesAlive.Add(ponyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        // Get all dead ponies
        List<PonyAI> poniesToRemove = new List<PonyAI>();
        foreach (PonyAI pony in currentPoniesAlive)
        {
            if (pony.currentState == PonyAI.PonyState.Dead)
            {
                poniesToRemove.Add(pony);
            }
        }

        // Actually remove all dead ponies
        foreach (PonyAI pony in poniesToRemove)
        {
            currentPoniesAlive.Remove(pony);
        }

        poniesToRemove.Clear();

        // Start cooldown if all ponies are dead
        if (currentPoniesAlive.Count == 0 && inCooldown == false)
        {
            // start cooldown for next wave
            StartCoroutine(WaveCooldown());
        }

        // Run the cooldown counter
        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }

        cooldownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;
        waveOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);

        currentPoniesPerWave *= 2;

        StartNextWave();
    }
}
