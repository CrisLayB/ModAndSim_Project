using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHover : MonoBehaviour
{
    [SerializeField] private Texture2D hoverPointerTexture, grabPointerTexture;
    [SerializeField] private float heightToGrab = -2.0f, offsetX = 2.3f, offSetZ = 2.5f;
    [SerializeField] private Color colorHover = Color.white;
    [SerializeField] private bool hasChildren = false;
    private Renderer renderer;
    private Color color;
    private Rigidbody rigidbody;
    private bool isGrabbed = false;
    private Renderer[] childrenList;
    private Color[] childrenColors;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        color = renderer.material.color;

        if(hasChildren)
        {
            childrenList = GetComponentsInChildren<Renderer>();
            childrenColors = new Color[childrenList.Length];
            GetChildren();
        }
    }

    private void Update()
    {
        if(isGrabbed) GrabbingObject();
    }

    private void OnMouseEnter()
    {
        if(hasChildren)
        {
            HoverChangueColorOfChildren();
            return;
        }

        Cursor.SetCursor(hoverPointerTexture, Vector2.zero, CursorMode.Auto);
        renderer.material.color = colorHover;
    }

    private void OnMouseExit()
    {
        if(hasChildren)
        {
            RegreatNormalColorsOfChildren();
            return;
        }

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        renderer.material.color = color;
    }

    private void OnMouseDown()
    {
        rigidbody.useGravity = false;
        isGrabbed = true;
    }    

    private void OnMouseUp()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        rigidbody.useGravity = true;
        isGrabbed = false;
    }

    private void GrabbingObject()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(hit.point.x + offsetX, heightToGrab, hit.point.z + offSetZ);
        }
        Cursor.SetCursor(grabPointerTexture, Vector2.zero, CursorMode.Auto);

        // Now we can set colors for the children elements
        if(hasChildren) HoverChangueColorOfChildren();
        else renderer.material.color = colorHover;
    }

    private void GetChildren()
    {
        for(int i = 0; i < childrenList.Length; i++)
        {
            Renderer childRenderer = childrenList[i];
            if(childRenderer != null)
            {
                Material material = childRenderer.material;
                Color materialColor = material.color;
                childrenColors[i] = materialColor;
            }
        }
    }

    private void HoverChangueColorOfChildren()
    {
        for(int i = 0; i < childrenList.Length; i++)
        {
            Renderer childRenderer = childrenList[i];            
            if(childRenderer != null)
            {
                childRenderer.material.color = colorHover;
            }
        }
    }

    private void RegreatNormalColorsOfChildren()
    {
        for(int i = 0; i < childrenList.Length; i++)
        {
            Renderer childRenderer = childrenList[i];            
            Color childColor = childrenColors[i];
            if(childRenderer != null)
            {
                childRenderer.material.color = childColor;
            }
        }
    }
}
