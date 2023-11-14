using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCut : Cook
{
    [SerializeField] private GameObject elementCut;
    [SerializeField] private float heightInstantiate = 4f;

    private void OnCollisionEnter(Collision other)
    {
        StartToCook(other);

        if (other.gameObject.tag.Equals("knife") && GetTimeCook() <= 0)
        {
            Destroy(this.gameObject);
            AudioManager.Play(AudioClipName.knifesharpener1);
            Vector3 vector = new Vector3(transform.position.x, heightInstantiate, transform.position.z);
            Instantiate(elementCut, vector, Quaternion.identity);
        }
    }
}
