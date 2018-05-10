using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pauser : MonoBehaviour
{
    static public List<Pauser> targets = new List<Pauser>();   // ポーズ対象のスクリプト

    // ポーズ対象のコンポーネント
    Behaviour[] pauseBehavs = null;

    Rigidbody2D[] rg2dBodies = null;
    Vector2[] rg2dBodyVels = null;
    float[] rg2dBodyAVels = null;

    // 破棄されるとき
    void OnDestory()
    {
        // ポーズ対象から除外する
        targets.Remove(this);
    }

    // ポーズされたとき
    void OnPause()
    {
        if (pauseBehavs != null)
        {
            return;
        }

        // 有効なコンポーネントを取得
        pauseBehavs = Array.FindAll
            (
            GetComponentsInChildren<Behaviour>(),
            (obj) => { return obj.enabled&&obj!=GetComponent<Slicer2D>()
                && obj != GetComponent<Collider2D>()
                && obj != GetComponent<BoxCollider2D>()
                && obj != GetComponent<PolygonCollider2D>()
                && obj != GetComponent<CircleCollider2D>()
                && obj!=GetComponent<SpriteMesh2D>(); }
            );
        foreach (var com in pauseBehavs)
        {
               com.enabled = false; 
        }

        rg2dBodies = Array.FindAll(GetComponentsInChildren<Rigidbody2D>(), (obj) => { return !obj.IsSleeping(); });
        rg2dBodyVels = new Vector2[rg2dBodies.Length];
        rg2dBodyAVels = new float[rg2dBodies.Length];
        for (var i = 0; i < rg2dBodies.Length; ++i)
        {
            rg2dBodyVels[i] = rg2dBodies[i].velocity;
            rg2dBodyAVels[i] = rg2dBodies[i].angularVelocity;
            rg2dBodies[i].Sleep();
           // rg2dBodies[i].isKinematic=true;
        }
    }

    // ポーズ解除されたとき
    void OnResume()
    {
        if (pauseBehavs == null)
        {
            return;
        }

        // ポーズ前の状態にコンポーネントの有効状態を復元
        foreach (var com in pauseBehavs)
        {
              com.enabled = true; 
        }

        for (var i = 0; i < rg2dBodies.Length; ++i)
        {
           // rg2dBodies[i].isKinematic = false;
            rg2dBodies[i].WakeUp();
            rg2dBodies[i].velocity = rg2dBodyVels[i];
            rg2dBodies[i].angularVelocity = rg2dBodyAVels[i];
        }
        pauseBehavs = null;

        rg2dBodies = null;
        rg2dBodyVels = null;
        rg2dBodyAVels = null;
    }

    // ポーズ
    public void Pause()
    {
        // ポーズ対象に追加する
        targets.Add(this);
           this.OnPause();
    }

    // ポーズ解除
    public static void Resume()
    {
        foreach (var obj in targets)
        {
            if (obj != null)
            {
                obj.OnResume();
            }
        }
        targets.Clear();
    }
}
