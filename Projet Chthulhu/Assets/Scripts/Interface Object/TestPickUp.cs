using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPickUp : MonoBehaviour, IClicked
{
    public void OnClickAction()
    {
        //Destroy(gameObject);
        GetComponent<MeshRenderer>().material.color = new Color(255, 1, 1);
    }

}
