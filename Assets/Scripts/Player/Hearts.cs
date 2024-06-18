using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{
    private int numOfHearts;
    private int numOfLives;

    public Image[] lives;
    public Image[] hearts;

    // Update is called once per frame
    void Update()
    {
        numOfHearts = GameManager.Instance.health;
        numOfLives = GameManager.Instance.lives;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < numOfLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }
}
