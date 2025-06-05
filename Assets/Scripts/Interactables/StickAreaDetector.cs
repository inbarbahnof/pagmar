using System;
using System.Collections;
using Interactables;
using UnityEngine;

public class StickAreaDetector : MonoBehaviour
{
    private bool _didStickLand;

    public bool didStickLand => _didStickLand;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stick") && other.GetComponent<ThrowablePickUpInteractable>().IsThrowing)
        {
            _didStickLand = true;
            StartCoroutine(SwitchOffLand());
        }
    }

    private IEnumerator SwitchOffLand()
    {
        yield return new WaitForSeconds(1f);
        _didStickLand = false;
    }
}
