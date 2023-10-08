using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorsController : MonoBehaviour
{
    public List<GameObject> mirrors = new List<GameObject>();
    private int currentMirror = 0;

    public float rotationSpeed = 10f;
    
    private void Update()
    {
        SwitchMirror();
        RotateMirror();
    }

    void RotateMirror()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            mirrors[currentMirror].transform.localEulerAngles += new Vector3(0f, Time.deltaTime * rotationSpeed, 0f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            mirrors[currentMirror].transform.localEulerAngles -= new Vector3(0f, Time.deltaTime * rotationSpeed, 0f);
        }
    }
    
    void SwitchMirror()
    {
        mirrors[currentMirror].GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentMirror++;
            if (currentMirror == mirrors.Count) currentMirror = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentMirror--;
            if (currentMirror < 0) currentMirror = mirrors.Count - 1;
        }

        mirrors[currentMirror].GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
    }
}
