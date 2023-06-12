using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    [SerializeField] static string playerName;
    [SerializeField] TMP_InputField mainInputField;

    private void Start()
    {
        mainInputField = mainInputField.GetComponent<TMP_InputField>();
    }

    public string GetName()
    { return playerName; }

    public void PlayGame()
    {
        if (!string.IsNullOrEmpty(mainInputField.text))
        {
            playerName = mainInputField.text;
            SceneManager.LoadScene("main");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            print("ingrese un nombre " + playerName);
        }
    }
}
