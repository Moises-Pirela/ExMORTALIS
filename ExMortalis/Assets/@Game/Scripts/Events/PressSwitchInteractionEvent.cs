using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PressSwitchInteractionEvent : MonoBehaviour
{
    private Vector3 StartPosition;
    [SerializeField] private Transform TargetSwitch;
    [SerializeField] private Vector3 PressedPosition;
    [SerializeField] private float TimeToRelease = 1f;
    [SerializeField] private AudioSource AudioSource;

    private void Awake()
    {
        StartPosition = TargetSwitch.transform.position;
    }

    public void Press()
    {
        TargetSwitch.DOLocalMove(PressedPosition, TimeToRelease);
        AudioSource.Play();
    }
}
