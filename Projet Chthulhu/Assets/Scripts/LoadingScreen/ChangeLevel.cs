using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private string targetScene;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Vector3 spawnRot;
    private void ChargeSuite()
    {
        PlayerPrefs.SetString("targetscene", targetScene);
        SceneManager.LoadScene("LoadingScreen",LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log(SaveSystem.current);
            SaveSystem.current.SaveGame(spawnPos, spawnRot);
            ChargeSuite();
        }
    }
}
