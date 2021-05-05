using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUi : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Text content;
    // Update is called once per frame
    void Update()
    {
    }

    public void SetText (string name, string description) {
        title.text = name;
        content.text = description;
    }

    public void SetTooltipPosition (Transform relative) {
        float posX = relative.position.x;
        float posY = relative.position.y + GetComponent<RectTransform>().rect.height/2;

        transform.position = new Vector3(posX, posY, 0);
    }
}
