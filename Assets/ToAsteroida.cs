using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToAsteroida : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            SceneManager.LoadScene(3);
        }
    }
}
