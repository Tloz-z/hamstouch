using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Spine : UI_Base
{
    protected SkeletonGraphic _anim = null;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _anim = GetComponent<SkeletonGraphic>();
        return true;
    }

    #region Spine Animation
    public void SetSkeletonAsset(string path)
    {
        Init();
        _anim.skeletonDataAsset = Managers.Resource.Load<SkeletonDataAsset>(path);
        _anim.Initialize(true);
    }

    public void SetCustomEvent(Spine.AnimationState.TrackEntryEventDelegate callback)
    {
        Init();
        _anim.AnimationState.Event -= callback;
        _anim.AnimationState.Event += callback;
    }

    public float GetAnimationLength(string name)
    {
        return _anim.skeletonDataAsset.GetSkeletonData(true).FindAnimation(name).Duration;
    }

    public string GetCurrentAnimation()
    {
        return _anim.startingAnimation;
    }

    public string GetCurrentSkin()
    {
        return _anim.initialSkinName;
    }

    public void PlayAnimation(string name, bool loop = true)
    {
        Init();
        if (_anim.startingAnimation == name)
            return;

        _anim.startingAnimation = name;
        _anim.startingLoop = loop;
        _anim.Initialize(true);
    }

    public void PlayAnimationForce(string name, bool loop = true)
    {
        Init();
        _anim.startingAnimation = name;
        _anim.startingLoop = loop;
        _anim.Initialize(true);
    }

    public void ChangeSkin(string name)
    {
        Init();
        _anim.initialSkinName = name;
        _anim.Skeleton.SetSkin(name);
    }

    public void Refresh()
    {
        Init();
        _anim.Initialize(true);
    }

    public void PlayAnimationOnce(string name)
    {
        if (_anim.startingAnimation == name)
            return;

        StartCoroutine(CoPlayAnimationOnce(name));
    }

    IEnumerator CoPlayAnimationOnce(string name)
    {
        bool defaultLoop = _anim.startingLoop;
        string defaultName = _anim.startingAnimation;

        _anim.startingLoop = false;
        _anim.startingAnimation = name;
        _anim.Initialize(true);

        float length = _anim.skeletonDataAsset.GetSkeletonData(true).FindAnimation(name).Duration;
        yield return new WaitForSeconds(length); // �ִ� �ð���ŭ ���

        if (_anim.startingAnimation != name)
            yield break;

        // ���� �ִ� ����
        PlayAnimation(defaultName, defaultLoop);
    }

    public void PlayAnimationOnce(string skin, string name)
    {
        StartCoroutine(CoPlayAnimationOnce(skin, name));
    }

    IEnumerator CoPlayAnimationOnce(string skin, string name)
    {
        bool defaultLoop = _anim.startingLoop;
        string defaultSkin = _anim.initialSkinName;
        string defaultName = _anim.startingAnimation;

        _anim.startingLoop = false;
        _anim.startingAnimation = name;
        ChangeSkin(skin);

        float length = _anim.skeletonDataAsset.GetSkeletonData(true).FindAnimation(name).Duration;
        yield return new WaitForSeconds(length); // �ִ� �ð���ŭ ���

        // ���� �ִ� ����
        PlayAnimation(defaultName, defaultLoop);
        ChangeSkin(defaultSkin);
    }
    #endregion
}
