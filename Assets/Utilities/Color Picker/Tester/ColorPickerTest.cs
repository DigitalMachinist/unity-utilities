using System;
using System.Collections;﻿
using System.Collections.Generic;﻿
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class ColorPickerTest : MonoBehaviour
{
    public ColorPicker Picker;

    new Renderer renderer;

    void Awake()
    {
        Picker = GetComponent<ColorPicker>();
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if ( Application.isPlaying )
        {
            transform.position = new Vector3(
                1.2f * Mathf.Sin( Time.time ),
                0.3f * Mathf.Cos( Time.time ) + 0.8f,
                0f
            );
        }

        renderer.sharedMaterial.color = Picker.Color;
    }
}
