using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Rogue.Title
{
    public class FadePanel : MonoBehaviour
    {
        [SerializeField] float fadeSpeed;
        private AudioSource audioSource;
        private CanvasGroup panel;

        private void Awake()
        {
            panel = GetComponent<CanvasGroup>();
            audioSource = GetComponent<AudioSource>();
        }

        public IEnumerator FadeOutStartScene()
        {
            while (panel.alpha != 1) {
                panel.alpha += Time.deltaTime / fadeSpeed;
                yield return null;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public IEnumerator FadeInVideo()
        {
            GameObject g = GameObject.FindGameObjectWithTag("VideoPlayer");
            VideoPlayer v = g.GetComponentInChildren<VideoPlayer>();
            audioSource.Play();
            v.Play();
            v.Pause();

            while (panel.alpha != 0)
            {
                panel.alpha -= Time.deltaTime / fadeSpeed;
                yield return null;
            }

            v.Play();
            

            StartCoroutine(AfterVideo(v));
        }

        private IEnumerator AfterVideo(VideoPlayer v)
        {
            while (v.isPlaying)
            {
                yield return null;
            }
            StartCoroutine(FadeOutVideo());

        }

        private IEnumerator FadeOutVideo()
        {
            while (panel.alpha != 1)
            {
                panel.alpha += Time.deltaTime / fadeSpeed;
                yield return null;
            }
            (transform.parent).GetComponentInChildren<TextPlayer>().CancelWaiting();
            
        }
    }
}
