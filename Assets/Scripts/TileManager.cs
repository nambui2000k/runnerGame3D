using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabsEasy;// mang prefabs
    public GameObject[] tilePrefabsMedium;
    public GameObject[] tilePrefabsDifficult;
    private Transform player;
    private float spawnZ = 0.0f;// vi tri thay doi
    private float tileLength = 10f;// chiu dai doan duong
    private int tilesOnScreen = 10;// so doan duong hien thi tren man hinh
    private int firstPrefabIndex = 0;
    private float safeZone = 15.0f;
    private List<GameObject> activeTiles;// list chua prefabs
    private bool isEasy = true, isMedium = false, isDifficult = false;

    // Start is called before the first frame update
    void Start()
    {
        activeTiles = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < tilesOnScreen; i++)
        {
            if (i < 2)
            {
                SpawnTile(tilePrefabsEasy, 0);
            }
            else
            {
                SpawnTile(tilePrefabsEasy);
            }
        }
        StartCoroutine(setMedium());
    }
    // ham sinh ra chi so ngau nhien
    private int RandomFrefabIndex(GameObject[] tilePrefabs)
    {
        if (tilePrefabs.Length <= 1)
        {
            return 0;
        }
        int randomIndex = firstPrefabIndex;
        while (randomIndex == firstPrefabIndex)
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }
        firstPrefabIndex = randomIndex;
        return randomIndex;
    }
    // Ham goi 1 prefabs theo chi so
    private void SpawnTile(GameObject[] tilePrefabs, int prefabIndex = -1)
    {
        GameObject g;
        if (prefabIndex == -1)
        {
            g = Instantiate(tilePrefabs[RandomFrefabIndex(tilePrefabs)]) as GameObject;
        }
        else
        {
            g = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        }
        // doi cho 
        g.transform.SetParent(transform);
        g.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(g);
    }
    // Update is called once per frame
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
    void Update()
    {
        if (PlayerManager.isGameStarted)
        {
            if (player.position.z - safeZone > (spawnZ - tilesOnScreen * tileLength))
            {

                if (isEasy)
                {
                    SpawnTile(tilePrefabsEasy);
                }
                else if (isMedium)
                {
                    SpawnTile(tilePrefabsMedium);
                }
                else if (isDifficult)
                {
                    SpawnTile(tilePrefabsDifficult);
                }
                DeleteTile();
            }
        }

    }
    private IEnumerator setMedium()
    {
        yield return new WaitForSeconds(30f);
        isEasy = false;
        isMedium = true;
        StartCoroutine(setDifficult());
    }
    private IEnumerator setDifficult()
    {
        yield return new WaitForSeconds(45f);
        isEasy = false;
        isMedium = false;
        isDifficult = true;
    }
}
