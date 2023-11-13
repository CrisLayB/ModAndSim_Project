using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHover : MonoBehaviour
{
    [SerializeField] private Texture2D hoverPointerTexture;

    private void OnMouseEnter()
    {
        Cursor.SetCursor(hoverPointerTexture, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
