using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocations : MonoBehaviour
{
    private string tagName;

    public static event Action<string> showMapLocation;

    private void Start()
    {
        tagName = gameObject.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetType() == typeof(SphereCollider) && other.tag == "Player")
        {
            Vector3 direction = other.transform.position - transform.position;
            string location = WhatLocationToShow(tagName, direction);
            if (location != "")
            {
                showMapLocation?.Invoke(location);
            }
            
        }
    }

    private string WhatLocationToShow(string tag, Vector3 directionFromPlayerEntered)
    {
        string locationName = "";

        switch (tag)
        {
            case "HorizontalVestibulTrigger":
                if (directionFromPlayerEntered.y > 0)
                {
                    locationName = "Vestibul";
                }
                
                break;

            case "VerticalVestibulTriggerRight":
                if (directionFromPlayerEntered.x > 0)
                {
                    locationName = "Vestibul";
                }
                break;

            case "VerticalVestibulTriggerLeft":
                if (directionFromPlayerEntered.x < 0)
                {
                    locationName = "Vestibul";
                }
                break;

            case "RecepciaTrigger":
                if (directionFromPlayerEntered.y > 0)
                {
                    locationName = "Vestibul";
                }
                else
                {
                    locationName = "Recepcia";
                }
                break;

            case "AulaTrigger":
                if(directionFromPlayerEntered.x > 0)
                {
                    locationName = "Aula";
                }
                break;

            case "HorizontalKniznicaTrigger":
                if (directionFromPlayerEntered.y < 0)
                {
                    locationName = "Kni�nica";
                }
                
                break;

            case "VerticalKniznicaTriggerRight":
                if (directionFromPlayerEntered.x > 0)
                {
                    locationName = "Kni�nica";
                }
                break;

            case "VerticalKniznicaTriggerLeft":
                if (directionFromPlayerEntered.x < 0)
                {
                    locationName = "Kni�nica";
                }
                break;

            case "ToaletyTrigger":
                if(directionFromPlayerEntered.y < 0)
                {
                    locationName = "Toalety";
                }
                break;

            case "VerticalChodbaTriggerLeft":
                if (directionFromPlayerEntered.x < 0)
                {
                    locationName = "Chodba";
                }
                break;

            case "VerticalChodbaTriggerRight":
                if (directionFromPlayerEntered.x > 0)
                {
                    locationName = "Chodba";
                }
                break;

            case "HorizontalJedalenTrigger":
                if (directionFromPlayerEntered.y < 0)
                {
                    locationName = "Jed�le�";
                }
                break;
            case "VerticalJedalenTrigger":
                if (directionFromPlayerEntered.x < 0)
                {
                    locationName = "Jed�le�";
                }
                break;

            case "HorizontalStrojovnaTriggerUpper":
                if (directionFromPlayerEntered.y > 0)
                {
                    locationName = "Strojov�a";
                }
                break;

            case "HorizontalStrojovnaTriggerLower":
                if (directionFromPlayerEntered.y < 0)
                {
                    locationName = "Strojov�a";
                }
                break;

            case "VerticalStrojovnaTrigger":
                if (directionFromPlayerEntered.x < 0)
                {
                    locationName = "Strojov�a";
                }
                break;

            case "VerticalPCucebnaTriggerRight":
                if(directionFromPlayerEntered.x > 0){
                    locationName = "PC U�eb�a";
                }
                break;

            case "VerticalPCucebnaTriggerLeft":
                if (directionFromPlayerEntered.x < 0){
                    locationName = "PC U�eb�a";
                }
                break;

            case "HorizontalPrednaskovaTrigger":
                if (directionFromPlayerEntered.y < 0)
                {
                    locationName = "Predn�kov� Miestnos�";
                }
                break;

            case "VerticalPrednaskovaTrigger":
                if (directionFromPlayerEntered.x > 0)
                {
                    locationName = "Predn�kov� Miestnos�";
                }
                break;
        }

        return locationName;
    }
}
