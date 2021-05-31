using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenButton : MonoBehaviour
{
    [Header("Cat�gorie")]
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject tutoriels;

    [Header("Variables")]
    [SerializeField] private bool hasSeenTuto = false;

    [Header("Bouton Important")]
    public GameObject boutonJouer;
    public GameObject boutonRetourOption;
    public GameObject boutonRetourTuto;

    [Header("Nom des sc�ne")]
    [SerializeField] private string Hall;
    [SerializeField] private string EastWing;
    [SerializeField] private string WestWing;
    [SerializeField] private string FirstFloor;

    private void Awake() {
        PlayerPrefs.DeleteAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeAffiche(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jouer()
    {
        if (hasSeenTuto)
        {
            SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
            PlayerPrefs.SetString("targetscene", Hall);
        }
        else
        {
            hasSeenTuto = true;
            ChangeAffiche(2);
        }
    }


    public void ChangeAffiche(int menu)
    {
        switch (menu)
        {
            case 0:
                //MenuPrincipal
                menuPrincipal.SetActive(true);
                options.SetActive(false);
                tutoriels.SetActive(false);
                break;
            case 1:
                //Option
                menuPrincipal.SetActive(false);
                options.SetActive(true);
                tutoriels.SetActive(false);
                break;
            case 2:
                //Tuto
                menuPrincipal.SetActive(false);
                options.SetActive(false);
                tutoriels.SetActive(true);
                break;
        }
    }

    public void Quitter()
    {
        Application.Quit();
    }

}
