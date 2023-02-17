﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
            return transform.gameObject;
        return null;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Vector3 ClampDir(Vector3 dir)
    {
        float angle = Quaternion.FromToRotation(Vector3.left, dir).eulerAngles.z;
        angle -= 180;
        if (angle < 0)
            angle += 360;

        // 오른쪽 방향 보정
        angle = Mathf.Clamp(angle, 0f + Define.CLAMP_ANGLE, 360f - Define.CLAMP_ANGLE);

        // 왼쪽 방향 보정
        if (angle >= 180 - Define.CLAMP_ANGLE && angle <= 180)
            angle = 180 - Define.CLAMP_ANGLE;
        else if (angle > 180 && angle <= 180 + Define.CLAMP_ANGLE)
            angle = 180 + Define.CLAMP_ANGLE;

        return new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized;
    }

    public static void Vibrate()
    {
        if (Managers.Game.Vibrate)
            Handheld.Vibrate();
    }

    public static Sequence MakePopupOpenSequence(GameObject obj)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(obj.transform.DOScale(0, 0))
            .Append(obj.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack));
        return sequence;
    }

    public static Sequence MakePopupCloseSequence(GameObject obj, float length = 0.3f)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(obj.transform.DOScale(0, length).SetEase(Ease.InBack));
        return sequence;
    }

    public static Sequence MakeSpawnSequence(GameObject obj)
    {
        Sequence sequence = DOTween.Sequence()
            .Append(obj.transform.DOScale(0, 0))
            .Append(obj.transform.DOScale(1, 0.3f).SetEase(Ease.OutQuad));
        return sequence;
    }

    public static Sequence MakeIncreaseTextSequence(GameObject obj, float distance)
    {
        Vector3 defaultPos = obj.transform.localPosition;

        Sequence sequence = DOTween.Sequence()
            .Append(obj.transform.DOScale(0, 0))
            .Append(obj.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad))
            .Join(obj.transform.DOLocalMoveY(distance, 0.9f).SetRelative().SetEase(Ease.Linear))
            .Insert(0.8f, obj.transform.DOScale(0, 0.3f).SetEase(Ease.InQuad))
            .Append(obj.transform.DOLocalMove(defaultPos, 0));
        return sequence;
    }
}

