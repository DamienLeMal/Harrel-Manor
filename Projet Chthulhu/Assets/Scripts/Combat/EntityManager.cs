using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EntityManager
{
    public List<GameObject> entities = new List<GameObject>();

    public void SetEnnemiesId () {
        foreach (GameObject g in entities) {
            if (g.GetComponent<EnnemyEntity>() == null) continue;
            g.GetComponent<EnnemyEntity>().id = "wp_" + SceneManager.GetActiveScene().name + "_" + entities.IndexOf(g).ToString();
        }
    }
}