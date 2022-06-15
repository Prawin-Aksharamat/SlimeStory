using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneSlime : MonoBehaviour
{
    sfxPlayer sfx;
    GameObject mainCamera;
    private Animator animator;
    [SerializeField] GameObject heart;
    [SerializeField] GameObject alert;
    [SerializeField] GameObject slimeEgg;

    [SerializeField] GameObject FinaleText;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        animator.SetTrigger("IdleDown");
        StartCoroutine(MovingToEnding());
    }

    private IEnumerator MovingToEnding()
    {
        yield return new WaitForSeconds(2f);

        animator.SetTrigger("IdleLeft");

        Vector3 targetPos = new Vector3(4,0,0);

        sfx.PlayCuteWalk();

        while (Vector2.Distance(transform.position, targetPos) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 2f * Time.deltaTime);
            yield return null;
        }

        targetPos = new Vector3(3, 0, 0);

        sfx.PlayCuteWalk();

        while (Vector2.Distance(transform.position, targetPos) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 2f * Time.deltaTime);
            yield return null;
        }

        targetPos = new Vector3(2, 0, 0);

        sfx.PlayCuteWalk();

        while (Vector2.Distance(transform.position, targetPos) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 2f * Time.deltaTime);
            yield return null;
        }

        targetPos = new Vector3(1, 0, 0);

        sfx.PlayCuteWalk();

        while (Vector2.Distance(transform.position, targetPos) > 0.1)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 2f * Time.deltaTime);
            yield return null;
        }

        animator.SetTrigger("IdleDown");

        targetPos = new Vector3(0.5f, 0.5f, -10);

        while (Vector2.Distance(mainCamera.transform.position, targetPos) > 0.1)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, targetPos, 2f * Time.deltaTime);
            Debug.Log(mainCamera.transform.position.x);
            yield return null;
        }

        GameObject x= Instantiate(alert, new Vector3(0.4f, 0.5f, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Destroy(x);

        yield return new WaitForSeconds(1f);

        Instantiate(heart, new Vector3(0.5f, 0.5f, 0), Quaternion.identity);

        yield return new WaitForSeconds(1f);

        Instantiate(slimeEgg, new Vector3(0.55f, 0.7f, 0), Quaternion.identity);

        yield return new WaitForSeconds(3f);

        Action a = Finale;
        StartCoroutine(GameObject.FindObjectOfType<FadeBetweenScene>().FinaleFade(a));
    }

    private void ToStartScreen()
    {
        
        SceneManager.LoadScene(0);
    }

    private void Finale()
    {
        Action x = ToStartScreen;
        StartCoroutine(StartFinale(x));
    }

    private IEnumerator StartFinale(Action x) {
        while (FinaleText.GetComponent<CanvasGroup>().alpha != 1)
        {
            FinaleText.GetComponent<CanvasGroup>().alpha += Time.deltaTime / 3;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        while (FinaleText.GetComponent<CanvasGroup>().alpha != 0)
        {
            FinaleText.GetComponent<CanvasGroup>().alpha -= Time.deltaTime / 3;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        x();
    }
}
