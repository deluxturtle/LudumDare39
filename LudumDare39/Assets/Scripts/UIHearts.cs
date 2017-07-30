using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author: Andrew Seba
/// Descirption: UI hearts.
/// </summary>
public class UIHearts : MonoBehaviour {

    //Prefabs of the UI element.
    public GameObject greyHeartPrefab;
    public GameObject redHeartPrefab;
    //Panels
    public GameObject greyHeartPanel;
    public GameObject redHeartPanel;

    private List<GameObject> hearts = new List<GameObject>();

    /// <summary>
    /// Sets up the amount of hearts for the scene.
    /// </summary>
    public void InitializeHearts(int maxHearts, int curHealth)
    {
        for(int i = 0; i < maxHearts; i++)
        {
            Instantiate(greyHeartPrefab, greyHeartPanel.transform);
        }
        for(int i = 0; i < curHealth; i++)
        {
            GameObject tempHeart = Instantiate(redHeartPrefab, redHeartPanel.transform);
            hearts.Add(tempHeart);
        }
    }
    
    public void DeductHealth(int amount)
    {
        for(int i = amount; i > 0; i--)
        {
            Destroy(hearts[hearts.Count-1].gameObject);
            hearts.RemoveAt(hearts.Count-1);
        }
    }
}
