using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour {
    private Texture2D blackTexture;//暗転用黒エフェクト
    private float fadeAlpha = 0;　　//フィード中の透明度
    private bool isFading = false;　　//フェード中かどうかを確認する
    private static bool created = false;

    public void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            blackTexture = new Texture2D(32, 32, TextureFormat.RGB24, false);
            blackTexture.ReadPixels(new Rect(0, 0, 32, 32), 0, 0, false);
            blackTexture.SetPixel(0, 0, Color.white);
            blackTexture.Apply();
        }
    }
　　public void OnGUI()
    {
        if (!isFading)
            return;
        //透明度を更新して黒テクスチャを描画
        GUI.color = new Color(0, 0, 0, this.fadeAlpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), this.blackTexture);
    }

    /// <summary>
    /// 暗転
    /// scene 遷移したいステージの名前を入力する　nullなら、遷移しない
    /// interval 暗転にかかる時間（秒）
    /// </summary>
    public void Fade(string scene, float interval)
    {
        if (!isFading)
        {
            StartCoroutine(TransScene(scene, interval));
        }
    }

    private IEnumerator TransScene(string scene,float interval)
    {
        //だんだん暗く
        this.isFading = true;
        float time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }
        if (scene !="null")
        {
            SceneManager.LoadScene(scene);
        }
        //だんだん明るく
        time = 0;
        while (time <= interval)
        {
            this.fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
            time += Time.deltaTime;
            yield return 0;
        }
        this.isFading = false;
    }
}
