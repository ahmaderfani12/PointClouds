using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DistortionMeshInputControl : MonoBehaviour
{
    [SerializeField] DistortionMesh DistortionMesh;
    Tween Increasetween;
    Tween Decreasetween;

    bool toggle;

    private void Awake()
    {
        Increasetween = DOTween.To(ChaosStep, 0, 1, 5).SetEase(Ease.InOutSine);
        Decreasetween = DOTween.To(ChaosStep, 1, 0, 5).SetEase(Ease.InOutSine);
        Decreasetween.Pause();
        Increasetween.Pause();
        Increasetween.SetAutoKill(false);
        Decreasetween.SetAutoKill(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (toggle && !Increasetween.IsPlaying() && !Decreasetween.IsPlaying())
            {
                toggle = false;
                Decreasetween.Restart();
            }
            else if(!toggle && !Increasetween.IsPlaying() && !Decreasetween.IsPlaying())
            {
                toggle = true;
                Increasetween.Restart();
            }
        }
    }
 
    void ChaosStep(float step)
    {
        DistortionMesh.Chaos = Mathf.Lerp(0, 1000, step);
    }
}
