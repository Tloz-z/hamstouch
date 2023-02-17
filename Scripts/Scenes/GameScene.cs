﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.DevScene;
        Managers.Game.Init();
        Managers.UI.ShowPopupUI<UI_Title>();
        return true;
    }
}
