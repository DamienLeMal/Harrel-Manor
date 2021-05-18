using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
            gamePaused = !gamePaused;
        }
    }

    private void PauseGame () {
        if (gamePaused) {
            Time.timeScale = 1;
            SoundEventManager.current.Pause();
        }else{
            Time.timeScale = 0;
            SoundEventManager.current.Unpause();
        }
    }
}
