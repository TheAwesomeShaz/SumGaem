using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject selectedVisual;

    private void Start()
    {
        PlayerController.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == clearCounter)
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
        selectedVisual.SetActive(true);
    }

    private void Hide()
    {
        selectedVisual.SetActive(false);
    }

}
