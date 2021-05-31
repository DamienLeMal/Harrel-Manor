using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActorStatUi : MonoBehaviour
{
    [SerializeField] private PlayerEntity player;
    private Text txt;
    [SerializeField] private Stat stat;

    private void Start() {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (stat) {
            case Stat.Ap :
                txt.text = player.ap.ToString();
                break;
            case Stat.Mp :
                txt.text = player.mp.ToString();
                break;
            case Stat.Pm :
                txt.text = player.pm.ToString();
                break;
        }
    }
}
