/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{

    // Create a Damage Popup
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit, int fontSize = 0)
    {
        // Adjust position
        Vector3 newPosition = position;
        newPosition.x -= 1;

        // Instantiate the Damage Popup prefab
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, newPosition, Quaternion.identity);

        // Get the DamagePopup component
        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);

        // Set the font size (you can change the size here)
        damagePopup.textMesh.fontSize = 4;


        return damagePopup;
    }

    // Sorting order for rendering order
    private static int sortingOrder = 10000;

    // Duration for the popup to disappear
    private const float DISAPPEAR_TIMER_MAX = 1f;

    // TextMeshPro component for displaying text
    private TextMeshPro textMesh;

    // Timer for controlling the disappearing effect
    private float disappearTimer;

    // Color of the text
    private Color textColor;

    // Movement vector for the popup
    private Vector3 moveVector;

    private void Awake()
    {
        // Get TextMeshPro component
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    // Set up the Damage Popup with damage amount and critical hit status
    public void Setup(int damageAmount, bool isCriticalHit)
    {
        // Set text to display damage amount
        textMesh.SetText(damageAmount.ToString());

        // Set font size and text color based on critical hit status
        if (!isCriticalHit)
        {
            // Normal hit
            textMesh.fontSize = 5;
            textColor = UtilsClass.GetColorFromString("F8F8FF");
        }
        else
        {
            // Critical hit
            textMesh.fontSize = 5;
            textColor = UtilsClass.GetColorFromString("FF2B00");
        }

        // Set text color, disappear timer, sorting order, and movement vector
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(.7f, 1) * 60f;
    }

    private void Update()
    {
        // Move the popup
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 20f * Time.deltaTime;

        // Scale the popup during its lifetime
        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            // First half of the popup lifetime
            float increaseScaleAmount = 0.01f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half of the popup lifetime
            float decreaseScaleAmount = 0.01f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        // Decrease disappear timer
        disappearTimer -= Time.deltaTime;

        // Start disappearing if the timer is less than zero
        if (disappearTimer < 0)
        {
            float disappearSpeed = 2f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            // Destroy the object when text becomes fully transparent
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
