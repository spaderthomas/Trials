using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTimeToShader : MonoBehaviour
{
    IAffectedByTimeTravel timeTravelHandler;
    float time;
    MaterialPropertyBlock block;
    Renderer renderer;
    public bool applyRandomness;
    float rand;
    // Start is called before the first frame update
    void Start()
    {
        timeTravelHandler = GetComponentInParent<IAffectedByTimeTravel>();
        time = 0f;
        renderer = this.GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        block.SetFloat("_UseTime", 1f);
        block.SetFloat("_InputTime", time);
        renderer.SetPropertyBlock(block);
        if (applyRandomness)
        {
            rand = Random.Range(0f, Mathf.PI * 2f);
        }
        else
        {
            rand = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeTravelHandler.IsFrozen())
        {
            time += Time.deltaTime;
            block.SetFloat("_UseTime", 1f);
            block.SetFloat("_InputTime", time + rand);
            renderer.SetPropertyBlock(block);
        }
    }
}
