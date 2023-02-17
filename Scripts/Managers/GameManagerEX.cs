using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using static Define;

[Serializable]
public class ItemInfo
{
    public int x;
    public int y;
}

[Serializable]
public class BlockInfo
{
    public int x;
    public int y;
    public int hp;
}

public enum GameState
{
    idle,
    shoot,
    skill,
}

[Serializable]
public class GameData
{
    public GameState gameState;
    public int volume;
    public bool vibrate;

    public int score;
    public int highscore;
    public int fullBallCount;
    public int ballSpeed;
    public Vector3 shootDir;

    //��ų
    public int glassesCooltime;
    public int lineCount;
    public int powerUpCooltime;
    public int ballDamage;

    //����ź
    public int nuclearDivisionCount;
    public int nuclearStack;

    public bool newGame;

    public Vector3 hamsterPos;
    public BlockInfo[] blockList = new BlockInfo[MAX_BLOCK_COUNT];
    public ItemInfo[] itemList = new ItemInfo[MAX_BLOCK_COUNT];
}

public class GameManagerEX
{
    GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value; } }

    public GameState State
    {
        get { return _gameData.gameState; }
        set { _gameData.gameState = value; }
    }

    public float Volume
    {
        get { return (_gameData.volume == 0) ? 0.0f : (float)_gameData.volume / Define.MAX_VOLUME_COUNT * Define.MAX_VOLUME + 0.01f; }
        set { _gameData.volume = (int)value; }
    }

    public bool Vibrate
    {
        get { return _gameData.vibrate; }
        set { _gameData.vibrate = value; }
    }

    public int Score
    {
        get { return _gameData.score; }
        set
        {
            if (value > MAX_SCORE)
                return;
            _gameData.score = value;
        }
    }

    public int Highscore
    {
        get { return _gameData.highscore; }
        set
        {
            if (value > MAX_SCORE)
                return;
            _gameData.highscore = value;
        }
    }

    public int FullBallCount
    {
        get { return _gameData.fullBallCount; }
        set { _gameData.fullBallCount = value; }
    }

    public int BallSpeed
    {
        get { return _gameData.ballSpeed; }
        set { _gameData.ballSpeed = value; }
    }

    public Vector3 ShootDir
    {
        get { return _gameData.shootDir; }
        set { _gameData.shootDir = value; }
    }

    public Vector3 HamsterPos
    {
        get { return _gameData.hamsterPos; }
        set { _gameData.hamsterPos = value; }
    }

    public int GlassesCooltime
    {
        get { return _gameData.glassesCooltime; }
        set { _gameData.glassesCooltime = value; }
    }

    public int LineCount
    {
        get { return _gameData.lineCount; }
        set { _gameData.lineCount = value; }
    }

    public int PowerUpCooltime
    {
        get { return _gameData.powerUpCooltime; }
        set { _gameData.powerUpCooltime = value; }
    }

    public int BallDamage
    {
        get { return _gameData.ballDamage; }
        set { _gameData.ballDamage = value; }
    }

    public int NuclearDivisionCount
    {
        get { return _gameData.nuclearDivisionCount; }
        set { _gameData.nuclearDivisionCount = value; }
    }

    public int NuclearStack
    {
        get { return _gameData.nuclearStack; }
        set { _gameData.nuclearStack = value; }
    }

    public BlockInfo[] BlockList
    {
        get { return _gameData.blockList; }
        set { _gameData.blockList = value; }
    }

    public ItemInfo[] ItemList
    {
        get { return _gameData.itemList; }
        set { _gameData.itemList = value; }
    }

    public bool NewGame
    {
        get { return _gameData.newGame; }
        set { _gameData.newGame = value; }
    }


    public float CanvasScale { get; set; } = 1.0f;

    public void Init()
    {
        StartData data = Managers.Data.Start;

        State = GameState.idle;
        Score = data.score;
        FullBallCount = data.fullBallCount;
        BallSpeed = data.ballSpeed;
        ShootDir = Vector3.zero;
        HamsterPos = new Vector3(data.hamsterPosX, 0f, 0f);
        GlassesCooltime = 0;
        PowerUpCooltime = 0;
        LineCount = data.lineCount;
        BallDamage = data.ballDamage;
        NuclearDivisionCount = 0;
        NuclearStack = 0;

        BlockList = new BlockInfo[MAX_BLOCK_COUNT];
        ItemList = new ItemInfo[MAX_BLOCK_COUNT];

        NewGame = true;

        // �׻� �����Ǵ� ������
        if (File.Exists(Managers._savePath))
        {
            string fileStr = File.ReadAllText(Managers._savePath);
            GameData loadData = JsonUtility.FromJson<GameData>(fileStr);
            Highscore = loadData.highscore;
            Volume = loadData.volume;
            Vibrate = loadData.vibrate;
        }
        else
        {
            Highscore = data.score;
            Volume = data.volume;
            Vibrate = data.vibrate;
            NewGame = false;
        }
    }

    #region Save & Load	
    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
        File.WriteAllText(Managers._savePath, jsonStr);
        Debug.Log($"Save Game Completed : {Managers._savePath}");
    }

    public bool LoadGame()
    {
        if (File.Exists(Managers._savePath) == false)
            return false;

        string fileStr = File.ReadAllText(Managers._savePath);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);
        if (data == null || data.shootDir == Vector3.zero)
            return false;

        Managers.Game.SaveData = data;
        Debug.Log($"Save Game Loaded : {Managers._savePath}");
        return true;
    }
    #endregion
}