using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE : MonoBehaviour {
    private AudioSource SEPlayer;
    public AudioClip cameraShot;

    void Start()
    {
        //AudioSourceコンポーネントを取得し、変数に格納
        SEPlayer = GetComponent<AudioSource>();
    }
    public void CameraShot()
    {
        SEPlayer.PlayOneShot(cameraShot);
    }
}
