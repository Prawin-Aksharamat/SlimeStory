using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    [SerializeField] GameObject[] spaceTiles;
    private GameObject gameManager;
    private MapManager mapManager;

    [SerializeField] AudioClip rockBreakSound;
    sfxPlayer sfx;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        mapManager = gameManager.GetComponent<MapManager>();
        sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
    }

    public void DestroyWall()
    {
        StartCoroutine(DestroyItSelf());
    }

    private IEnumerator DestroyItSelf()
    {
        yield return new WaitForSeconds(0.5f);
        InstantDestroy();
    }

    public void InstantDestroy()
    {
        sfx.PlayThisClip(rockBreakSound);
        GameObject tile = spaceTiles[Random.Range(0, spaceTiles.Length)];
        Instantiate(tile, transform.position, Quaternion.identity);
        mapManager.UpdateMap(transform.position, 1, null);
        Destroy(gameObject);
    }
}
