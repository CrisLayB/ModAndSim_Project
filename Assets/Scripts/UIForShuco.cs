using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIForShuco : MonoBehaviour
{
    [SerializeField] private GameObject textSelectedShuco, shucoSelected;
    [SerializeField] private UIElementMark[] elementsMarked;
    private Dictionary<int, bool> elementsDone;

    public int TotalElements
    {
        get { return elementsMarked.Length;}
    }

    // Start is called before the first frame update
    private void Start()
    {
        if(textSelectedShuco == null)
        {
            Debug.LogError("the GameObject TextSelectedShuco is Null");
        }
        else
        {
            textSelectedShuco.SetActive(false);
        }

        if(shucoSelected == null)
        {
            Debug.LogError("the GameObject shucoSelected is Null");
        }
        else
        {
            shucoSelected.SetActive(false);
        }
    }

    private void Update()
    {
        if(elementsDone != null && shucoSelected.activeSelf)
        {
            // Iterar todo el diccionario
            foreach(KeyValuePair<int, bool> element in elementsDone)
            {
                int numElement = element.Key;
                bool isMark = element.Value;
                
                if(isMark) elementsMarked[numElement].ElementMarked();
                else elementsMarked[numElement].ElementNotMarked();
            }
        }
    }

    public void ActiveTextSelectedShuco(bool active)
    {
        textSelectedShuco.SetActive(active);
    }

    public void ActiveShucoElementsSelected(bool active)
    {
        shucoSelected.SetActive(active);
    }

    public void ChangeStateShuco(Dictionary<int, bool> elements)
    {
        elementsDone = elements;
    }
}
