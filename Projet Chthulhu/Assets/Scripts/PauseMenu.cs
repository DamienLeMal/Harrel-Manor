using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    [SerializeField] private GameObject canvasPause;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
    }

    private void PauseGame () {
        if (!gamePaused) {
            canvasPause.SetActive(true);
            Time.timeScale = 0;
            SoundEventManager.current.Pause();
        }else{
            canvasPause.SetActive(false);
            Time.timeScale = 1;
            SoundEventManager.current.Unpause();
        }
        gamePaused = !gamePaused;
    }
}
