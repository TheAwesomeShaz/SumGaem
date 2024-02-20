using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjects;

    private void Start()
    {
        PlayerController.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == baseCounter)
        {
            Show();   
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjects)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjects)
        {
            visualGameObject.SetActive(false);
        }
    }

}
