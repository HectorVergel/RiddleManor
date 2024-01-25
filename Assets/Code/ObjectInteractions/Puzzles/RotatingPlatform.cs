using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public List<Collider> noRotate = new List<Collider>();
    private List<PusheableObject> pushObjects = new List<PusheableObject>();

    public float degreesToRotate;
    public float rotationTime = 10f;
    private bool isRotating;

    public void RotatePlatformRight()
    {
        if (!isRotating)
        {
            AudioManager.Play("swivelPlate1").Volume(0.25f);
            StartCoroutine(RotateObject(transform.rotation, transform.rotation * Quaternion.Euler(0, degreesToRotate, 0), rotationTime));
        }
    }
    public void RotatePlatformLeft()
    {
        if (!isRotating)
        {
            AudioManager.Play("swivelPlate1").Volume(0.25f);
            StartCoroutine(RotateObject(transform.rotation, transform.rotation * Quaternion.Euler(0, -degreesToRotate, 0), rotationTime));
        }
    }

    IEnumerator RotateObject(Quaternion startRotation, Quaternion endRotation, float duration)
    {
        for (int i = 0; i < pushObjects.Count; i++)
        {
            if(pushObjects[i] == null)
            {
                pushObjects.RemoveAt(i);
                i++;
            }
            else pushObjects[i].rb.interpolation = RigidbodyInterpolation.None;
        }

        foreach (PusheableObject item in pushObjects)
        {
            item.canBePushed = false;
            if (item == null) continue;
            if(!item.rb.isKinematic) PlayerController.instance.StopPushing();
        }

        float t = 0f;
        isRotating = true;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        isRotating = false;
        for (int i = 0; i < pushObjects.Count; i++)
        {
            if(pushObjects[i] == null)
            {
                pushObjects.RemoveAt(i);
                i++;
            }
            else pushObjects[i].rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        foreach (PusheableObject item in pushObjects)
        {
            item.canBePushed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CharacterController") return;

        if (noRotate.Contains(other)) return;
        PusheableObject pusheableObject = other.GetComponentInParent<PusheableObject>();
        if (pusheableObject != null) 
        {
            pusheableObject.transform.SetParent(transform);
            pushObjects.Add(pusheableObject);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CharacterController") return;
        
        PusheableObject pusheableObject = other.GetComponentInParent<PusheableObject>();
        if (pushObjects.Contains(pusheableObject))
        {
            pusheableObject.transform.SetParent(null);
            pushObjects.Remove(other.GetComponentInParent<PusheableObject>());
        }
    }
}
