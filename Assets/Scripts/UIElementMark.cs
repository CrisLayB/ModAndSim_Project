using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElementMark : MonoBehaviour
{
    private Sprite spriteElementNotMarked;
    [SerializeField] private Sprite spriteElementMarked;


    // Start is called before the first frame update
    private void Start()
    {
        spriteElementNotMarked = this.GetComponent<Image>().sprite;

        if(spriteElementMarked == null)
        {
            Debug.LogError("You must need to put the image Element Done");
        }
    }

    public void ElementMarked()
    {
        this.GetComponent<Image>().sprite = spriteElementMarked;
    }

    public void ElementNotMarked()
    {
        this.GetComponent<Image>().sprite = spriteElementNotMarked;
    }
}
