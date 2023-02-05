using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private Vector3 uppertreePosition;
    [SerializeField] private Vector3 lowertreePosition;
    [SerializeField] private Vector3 groundPosition;
    [SerializeField] private Ease ease;
    [SerializeField] private float lerpSpeed = 5f;

    public void ShowBattleGrounds() => transform.DOMove(groundPosition, lerpSpeed).SetEase(ease).OnComplete(() => transform.position = groundPosition);
    public void ShowUpperSkillTree() => transform.DOMove(uppertreePosition, lerpSpeed).SetEase(ease).OnComplete(() => transform.position = uppertreePosition);
    public void ShowLowerSkillTree() => transform.DOMove(lowertreePosition, lerpSpeed).SetEase(ease).OnComplete(() => transform.position = lowertreePosition);
}
