using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public TextMeshProUGUI upperText;
    public GameObject aboutPanel;

    public GameObject playButton;
    public GameObject aboutButton;
    public GameObject quitButton;
    public GameObject returnButton;

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void About()
    {
        playButton.SetActive(false);
        aboutButton.SetActive(false);
        quitButton.SetActive(false);
        
        returnButton.SetActive(true);
        aboutPanel.SetActive(true);
    }

    public void Return()
    {
        playButton.SetActive(true);
        aboutButton.SetActive(true);
        quitButton.SetActive(true);

        
        aboutPanel.SetActive(false);
	returnButton.SetActive(false);
    }

}
