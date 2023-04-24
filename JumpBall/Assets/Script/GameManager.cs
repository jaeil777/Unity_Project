using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalItemCount;
    public int Stage;
    public Text needItemText;
    public Text nowItemText;

    public void GoGame()
    {
        SceneManager.LoadScene("Stage1");
    }
    public void GoHome()
    {
        SceneManager.LoadScene("GameStart");
    }
    private void Awake()
    {
        needItemText.text = "/" + totalItemCount.ToString();
    }


    public void GetItem(int count)
    {
        nowItemText.text = count.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("Stage" + Stage.ToString());
        }
    }
}
