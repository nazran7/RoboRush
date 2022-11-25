using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoManager : MonoBehaviour
{
    #region singleton
    public static VideoManager instance { get; private set; }
    private void Awake()
    {
        instance = this;
    }
    #endregion
    public float closeTimer;
    public AudioSource bgound;
    public VideoPlayer player;
    public GameObject cam;
    public void StartVideo()
    {
        cam.SetActive(true);
        StartCoroutine(CloseVideo());
        Invoke("Disble", 5);
    }
    void Disble()
    {
        cam.SetActive(false);
    }
    IEnumerator CloseVideo()
    {

        bgound.Stop();
        player.Play();
        yield return new WaitForSeconds(closeTimer);
        player.Stop();
    }

}
