using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DissolutionScript : MonoBehaviour
{
    [Range(0.0f, 10f)] public float effectDuration = 5;
    [Range(2.0f, 0.0f)] public float startDissolveAmount = 2;
    [Range(0.0f, -2.0f)] public float endDissolveAmount = -2;

    private float startTime;
    private Renderer _renderer;
    private MaterialPropertyBlock mpb;

    private float _dissolveAmount;
    private bool effectOn = false;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log(collision.gameObject.layer);
        if (collision.gameObject.layer == 6)
        {
            DissolutionFunc();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DissolutionFunc();
        }
    }

    public void DissolutionFunc()
    {
        _renderer = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        StartCoroutine(Dissolvement());
    }

    IEnumerator Dissolvement()
    {
        startTime = Time.time;
        effectOn = true;
        Debug.Log("Start coroutine ");
        while (effectOn)
        {
            _dissolveAmount = f(Time.time - startTime);
            //change the Material properties
            _renderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_DissolveAmount", _dissolveAmount);
            _renderer.SetPropertyBlock(mpb);

            //need to stop effect ?
            if ((Time.time - startTime) > effectDuration)
            {
                effectOn = false;
            }

            yield return new WaitForSeconds(0.01f);
        }
        Debug.Log("End coroutine ");
    }

    private float f(float x)
    {
        float a = (endDissolveAmount - startDissolveAmount) / effectDuration;
        float b = startDissolveAmount;
        return a * x + b;
    }
}
