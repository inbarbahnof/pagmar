using System;
using System.Collections;
using UnityEngine;

public class StickAreaDetector : MonoBehaviour
{
    private bool _didStickLand;

    public bool didStickLand => _didStickLand;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stick"))
        {
            print("stick landed");
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
