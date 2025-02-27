﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisplay : MonoBehaviour
{
    public Transform source;
    public Vector3 offset;
    public Vector3 travelVector;
    public float timeToTravel = 1.5f;
    public Text text;
    public float delay = 0.5f;
    float clock;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        text.color = new Color(0, 0, 0, 0);
        clock = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (clock < 1f)
        {
            Vector3 pos = Vector3.Lerp(source.position + offset, source.position + offset + travelVector, Mathf.Max(clock, 0f));
            transform.position = pos;

            text.color = new Color(text.color.r, text.color.g, text.color.b, 1f - Mathf.Max(clock, 0f));

            clock += Time.deltaTime / timeToTravel;

            transform.LookAt(Camera.main.transform);
        }
        else
        {
            clock = 1f;
            damage = 0;
        }
    }

    public void AddDamage(float dmg, DamageType type)
    {
        this.damage += dmg;
        clock = -delay;
        text.text = Mathf.Round(this.damage).ToString();
        text.color = FXController.GetColorForDamageType(type);
    }
}
