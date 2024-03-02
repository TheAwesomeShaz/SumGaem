using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private const string CUT = "Cut";

    [SerializeField]
    private CuttingCounter cuttingCounter;
    private Animator mAnimator;

    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        mAnimator.SetTrigger(CUT);
    }
     
}
