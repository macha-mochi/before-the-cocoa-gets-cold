using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneOnAwake : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene("Thanks");
    }
}
