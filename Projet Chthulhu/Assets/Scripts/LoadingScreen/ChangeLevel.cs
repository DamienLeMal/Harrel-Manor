using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private string targetScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChargeSuite()
    {
        PlayerPrefs.SetString("targetscene", targetScene);
        SceneManager.LoadScene("LoadingScreen",LoadSceneMode.Single);
    }
}
