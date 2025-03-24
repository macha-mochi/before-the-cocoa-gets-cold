using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadGame(string level)
    {
        SceneManager.LoadScene(level);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Door Triggered");
        if (collision.gameObject.tag == "Player" && GameObject.Find("Game Manager").GetComponent<GameController>().canEnterHouse)
        {
            LoadGame("Cabin");
        }
    }
}
