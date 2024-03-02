using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainterCounterVisual : MonoBehaviour
{
    private const string OPEN_CLOSE = "OpenClose";

    [SerializeField]
    private ContainterCounter containterCounter;
    private Animator mAnimator;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        containterCounter.OnPlayerGrabbedObject += ContainterCounter_OnPlayerGrabbedObject;
    }

    private void ContainterCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        mAnimator.SetTrigger(OPEN_CLOSE);
    }
}
