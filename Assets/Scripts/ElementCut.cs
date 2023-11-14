using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementCut : Cook
{
    [SerializeField] private GameObject elementCut;

    private void OnCollisionEnter(Collision other)
    {
        StartToCook(other);

        if (other.gameObject.tag.Equals("knife") && GetTimeCook() <= 0)
        {
            Destroy(this.gameObject);
            AudioManager.Play(AudioClipName.knifesharpener1);
            Instantiate(elementCut, transform.position, transform.rotation);
        }
    }
}
