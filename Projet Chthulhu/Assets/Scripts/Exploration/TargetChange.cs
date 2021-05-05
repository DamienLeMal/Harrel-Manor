using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetChange : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 ennemisStartingPos;
    private bool hasMoved;
    public GameObject ennemisLink;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        ennemisStartingPos = ennemisLink.transform.position;
        hasMoved = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ennemis")
        {
            if (!hasMoved)
            {
                this.gameObject.transform.position = ennemisStartingPos;
                hasMoved = !hasMoved;
            }
            else
            {
                this.gameObject.transform.position = startPos;
                hasMoved = !hasMoved;
            }
        }
    }
}