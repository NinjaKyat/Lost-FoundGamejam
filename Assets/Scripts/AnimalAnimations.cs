using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnimations : MonoBehaviour
{
    private Transform animalBody;
    private Animal animal;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    Vector3 velocity = Vector3.zero;
    float smoothTime = 0.15F;
    void Start()
    {
        animal = GetComponent<Animal>();
        animalBody = transform.GetChild(0);
        originalPosition = animalBody.localPosition;
        originalRotation = animalBody.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animal.isMoving)
        {
            animalBody.localPosition = Vector3.SmoothDamp(animalBody.localPosition, originalPosition, ref velocity, smoothTime);
            animalBody.localRotation = originalRotation;
        }
        else
        {
            
            Vector3 position = animalBody.localPosition;
            Vector3 rotation = animalBody.localRotation.eulerAngles;

            float frequencyP = 1, amplitudeP = 1;
            float frequencyR = 1, amplitudeR = 1;
            if (!animal.predator)
            {
                frequencyP = 20;
                amplitudeP = 1 / 4f;
                frequencyR = 15;
                amplitudeR = 2;
            }
            else
            {
                frequencyP = 5;
                amplitudeP = 1 / 4f;
                frequencyR = 10;
                amplitudeR = 2;
            }
            float yOffset = (Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * frequencyP)) * amplitudeP);
            float zOffset = Mathf.Pow(Mathf.Sin(Time.realtimeSinceStartup * frequencyR) * amplitudeR, 3f);
            
            animalBody.localPosition =  new Vector3(position.x, yOffset , position.z);
            animalBody.localEulerAngles = new Vector3(rotation.x, rotation.y, zOffset);
        }
    }
}
