using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHover : MonoBehaviour
{
    [SerializeField] private Texture2D hoverPointerTexture, grabPointerTexture;
    [SerializeField] private float heightToGrab = -2.0f;
    private Rigidbody rigidbody;
    private bool isGrabbed = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isGrabbed) GrabbingObject();
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(hoverPointerTexture, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
            transform.position = new Vector3(hit.point.x + 2.3f, heightToGrab, hit.point.z + 2.5f);
        }
        Cursor.SetCursor(grabPointerTexture, Vector2.zero, CursorMode.Auto);
    }
}
