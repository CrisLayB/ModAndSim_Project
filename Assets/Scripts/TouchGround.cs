using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGround : MonoBehaviour
{
    [SerializeField] private float yLimitDown;
    
    void Update()
    {
        if(transform.position.y < yLimitDown) Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag.Equals("ground")) Destroy(this.gameObject);
    }
}
