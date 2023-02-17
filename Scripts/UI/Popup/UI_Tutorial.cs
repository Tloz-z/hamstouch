using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UI_Tutorial : UI_Popup
{
    enum GameObjects
    {
        BackGround,
        Pad,
        Cursor,
        Block,
        PauseButton,
    }

    Vector3 _dir;
    GameObject _pad;
    UI_Game _board;
    bool _target = false;
    bool _dialogue = false;

    Action _tutorialCallback;
    Action _endCallback;

    bool _complete = false;
    List<string> _texts = new List<string>();
    int _chat = 0;
    int _charIndex = 0;
    int _sentenseIndex = 0;

    float _blockFirstPosY;
    float _blockSecondPosY;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        GetObject((int)GameObjects.BackGround).gameObject.BindEvent(OnClickDialogue);

        _board = GameObject.Find("UI_Game").GetComponent<UI_Game>();

        _pad = Instantiate(_board.GetObject((int)UI_Game.GameObjects.ControlPad), GetObject((int)GameObjects.Pad).transform);
        _pad.transform.position = _board.GetObject((int)UI_Game.GameObjects.ControlPad).transform.position;
        _pad.GetComponent<Image>().color = new Color(162.0f / 255.0f, 255.0f / 255.0f, 250.0f / 255.0f, 0.0f / 255.0f);

        GetObject((int)GameObjects.PauseButton).transform.position = _board.GetObject((int)UI_Game.GameObjects.PauseButton).transform.position;
        GetObject((int)GameObjects.PauseButton).BindEvent(OnPauseButton);
        
        GameObject cursor = GetObject((int)GameObjects.Cursor);
        cursor.GetOrAddComponent<UI_Spine>().PlayAnimation(Managers.Data.Spine.tutoHamsterIdle);

        cursor.transform.position = _board.GetObject((int)UI_Game.GameObjects.ControlPad).transform.position;
        cursor.transform.localPosition += new Vector3(-435f, -10f, 0f);

        _blockFirstPosY = _board.GetObject((int)UI_Game.GameObjects.PauseButton).transform.position.y + 250f;
        _blockSecondPosY = _board.GetObject((int)UI_Game.GameObjects.Power).transform.position.y;
        GetObject((int)GameObjects.Block).transform.DOMoveY(_blockFirstPosY, 0f);

        StartCoroutine(Sequence());
        return true;
    }

    public void SetInfo(Action tutorialCallback, Action endCallback)
    {
        Init();

        _tutorialCallback = tutorialCallback;
        _endCallback = endCallback;
    }

    IEnumerator Sequence()
    {
        yield return new WaitForSeconds(0.5f);

        List<string> s = new List<string>();
        s.Add("�ݰ���! ���� ���̾�!");
        s.Add("�׸��� �����ִ� �� �Ϳ��� ģ���� ��!");
        s.Add("�츰 �ܽ� ��ī�̿� ��� �ѵ� ���� ģ����.\n�׸��� �ʿ͵� ģ���� �ǰ� ������!");

        yield return StartCoroutine(ShowTexts(s, 0));
        s.Clear();

        s.Add("�����Ԣ�");
        yield return StartCoroutine(ShowTexts(s, 3));
        s.Clear();

        s.Add("�� �Ҹ���.. �������� ����?!");
        yield return StartCoroutine(ShowTexts(s, 0));
        s.Clear();

        GetObject((int)GameObjects.Block).transform.DOMoveY(_blockSecondPosY, 2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(2.0f);

        s.Add("�� ��� �ϴ±���!! �� ���� �� �� �༮!!");
        s.Add("�Ϳ��� �� ����� �Ȱ��� �����ٰ�\n������ ���߿� �� ���� ���Ҵ�..");
        s.Add("�������� ������ ������ �ܶ� ���Ƹ�����\n�Ϻ��� � �� �� �ְ���!!");
        yield return StartCoroutine(ShowTexts(s, 3));
        s.Clear();

        _board.GetObject((int)UI_Game.GameObjects.Hamster).GetComponent<UI_Spine>().PlayAnimation(Managers.Data.Spine.hamsterPrologue);
        s.Add("��ü ���� �Ҹ� �ϴ°ž�?!");
        s.Add("�� ģ������ ����ϰ� ������ ����!!");
        yield return StartCoroutine(ShowTexts(s, 0));
        s.Clear();

        s.Add("��~��");
        s.Add("� �Ǹ� �� �̻� �� �ڸ� ���� ���̴�!!");
        s.Add("�عٶ�� ���� ������� ��ٷ���!!");
        yield return StartCoroutine(ShowTexts(s, 3));
        s.Clear();

        GetObject((int)GameObjects.Block).transform.DOMoveY(_blockFirstPosY, 1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(1.0f);

        s.Add("���� �̻��� �༮�̾�!");
        s.Add("���������� ���� �������� �� �� ������\n���󿡵� ū ȥ���� �ҷ��� �ž�!");
        s.Add("�������� �� ���� �ʾ�����\n���� � ���ݸ� �������� ������?");
        yield return StartCoroutine(ShowTexts(s, 0));
        s.Clear();

        _tutorialCallback?.Invoke();
        _board.GetObject((int)UI_Game.GameObjects.Hamster).GetComponent<UI_Spine>().PlayAnimation(Managers.Data.Spine.hamsterIdle);
        s.Add("�켱 �⺻ ���۹���� �˷��ٰ�");
        s.Add("��� ������ ���� �ִ� �� �Ʒ���\n��ġ�ؾ� ��");
        s.Add("��Ȯ�ϰԴ� �� ���� ������\n�װ� ���� ������ �����̾�!");
        yield return StartCoroutine(ShowTexts(s, 0));
        s.Clear();

        ViewPad();
        yield return new WaitForSeconds(2.5f);

        BlockInfo info = new BlockInfo
        {
            x = 3,
            y = 1,
            hp = 1,
        };
        UI_Block block = Managers.UI.makeSubItem<UI_Block>(_board.GetObject((int)UI_Game.GameObjects.BlockGroup).transform);
        block.SetInfo(info);

        yield return new WaitForSeconds(0.3f);

        s.Add("��..�ȳ�..?");
        yield return StartCoroutine(ShowTexts(s, 4));
        s.Clear();

        s.Add("���� �ĵ�� �Ծ�!");
        s.Add("�̿� �̷��� �Ȱ� ���� �ù��� �����ٰ�!");
        yield return StartCoroutine(ShowTexts(s, 0));
        s.Clear();

        Utils.MakePopupOpenSequence(GetObject((int)GameObjects.Cursor));
        yield return new WaitForSeconds(0.2f);

        s.Add("�Ʊ� �˷���� ������..");
        yield return StartCoroutine(ShowTexts(s, 1));
        s.Clear();

        GetObject((int)GameObjects.Cursor).GetOrAddComponent<UI_Spine>().PlayAnimation(Managers.Data.Spine.tutoHamsterTouch);
        _target = true;
        yield return new WaitForSeconds(0.2f);

        s.Add("�̷��� ��ġ�ϸ� �߻� ��ΰ� ����!");
        s.Add("�� ���·� ��ġ�� �����ϸ鼭\n�¿�� �����̴� �ž�");
        yield return StartCoroutine(ShowTexts(s, 1));
        s.Clear();

        Sequence target = DOTween.Sequence()
            .Append(GetObject((int)GameObjects.Cursor).transform.DOLocalMoveX(10f, 0.3f).SetEase(Ease.Linear))
            .Append(GetObject((int)GameObjects.Cursor).transform.DOLocalMoveX(-300f, 0.3f).SetEase(Ease.Linear))
            .Append(GetObject((int)GameObjects.Cursor).transform.DOLocalMoveX(10f, 0.3f).SetEase(Ease.Linear))
            .Append(GetObject((int)GameObjects.Cursor).transform.DOLocalMoveX(-300f, 0.3f).SetEase(Ease.Linear))
            .Append(GetObject((int)GameObjects.Cursor).transform.DOLocalMoveX(-150f, 0.3f).SetEase(Ease.Linear));
        target.Restart();
        yield return new WaitForSeconds(target.Duration());

        s.Add("����....");
        yield return StartCoroutine(ShowTexts(s, 4));
        s.Clear();

        s.Add("�̷��� �� ������ �ڿ� ���� ����?");
        yield return StartCoroutine(ShowTexts(s, 2));
        s.Clear();

        GetObject((int)GameObjects.Cursor).GetOrAddComponent<UI_Spine>().PlayAnimation(Managers.Data.Spine.tutoHamsterIdle);
        Shoot();
        yield return new WaitForSeconds(2.0f);

        s.Add("�����̾�!!");
        yield return StartCoroutine(ShowTexts(s, 2));
        s.Clear();

        _board.GetObject((int)UI_Game.GameObjects.TutorialBlind).GetComponent<Image>().DOFade(0f, 0.5f);
        _pad.GetComponent<Image>().DOFade(0f, 0.5f);
        Utils.MakePopupCloseSequence(GetObject((int)GameObjects.Cursor));
        yield return new WaitForSeconds(0.5f);

        _board.GetObject((int)UI_Game.GameObjects.Hamster).GetComponent<UI_Spine>().PlayAnimation(Managers.Data.Spine.hamsterPrologue);
        s.Add("�� ��¡?");
        s.Add("���� �����̾�!");
        s.Add("���������κ��� �츮 ���� ������!");
        yield return StartCoroutine(ShowTexts(s, 0));
        s.Clear();

        _board.GetObject((int)UI_Game.GameObjects.Hamster).GetComponent<UI_Spine>().PlayAnimation(Managers.Data.Spine.hamsterIdle);
        Managers.UI.ClosePopupUI(this);
        _endCallback.Invoke();
    }

    void Update()
    {
        _board.ClearArrow();

        UpdateTarget();
    }

    void UpdateTarget()
    {
        if (_target == false)
            return;

        Vector3 cursorPos = GetObject((int)GameObjects.Cursor).transform.position;
        cursorPos -= _board.GetObject((int)UI_Game.GameObjects.GameBoard).transform.position;
        cursorPos /= _board.gameObject.transform.localScale.x;
        cursorPos.x += 150f;

        if (_board.CalcShootDir(out _dir, cursorPos) == false)
            return;

        _board.GenerateArrow(_dir);
    }

    void Shoot()
    {
        _target = false;
        Managers.Game.ShootDir = _dir;
        _board.LoadBalls();
        _board._shoot = true;
        Managers.Game.FullBallCount = 0;
    }

    void ViewPad()
    {
        Sequence pad = DOTween.Sequence()
            .Append(_pad.GetComponent<Image>().DOFade(0.9f, 0.5f))
            .Append(_pad.GetComponent<Image>().DOFade(0f, 0.5f))
            .Append(_pad.GetComponent<Image>().DOFade(0.9f, 0.5f))
            .Append(_pad.GetComponent<Image>().DOFade(0f, 0.5f))
            .Append(_pad.GetComponent<Image>().DOFade(0.6f, 0.5f));

        pad.Restart();
    }

    IEnumerator ShowTexts(List<string> texts, int chat)
    {
        _texts = texts;
        _charIndex = 0;
        _sentenseIndex = 0;
        _complete = false;
        _dialogue = true;
        _chat = chat;

        _board.GetObject((int)UI_Game.GameObjects.Dialogue1 + _chat).transform.localScale = Vector3.zero;
        Utils.MakePopupOpenSequence(_board.GetObject((int)UI_Game.GameObjects.Dialogue1 + _chat)).Restart();

        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            if (_charIndex >= _texts[_sentenseIndex].Length)
            {
                _board.GetText((int)UI_Game.Texts.DialogueText1 + _chat).text = _texts[_sentenseIndex];
                _complete = true;
                while (_complete && _dialogue)
                    yield return null;
                if (_dialogue == false)
                    break;
                else
                    continue;
            }

            _charIndex++;
            string text = _texts[_sentenseIndex].Substring(0, _charIndex);
            _board.GetText((int)UI_Game.Texts.DialogueText1 + _chat).text = text;

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);
        _board.GetText((int)UI_Game.Texts.DialogueText1 + _chat).text = "";
    }

    void OnClickDialogue()
    {
        if (_dialogue == false)
            return;

        if (_complete == false)
        {
            _charIndex = _texts[_sentenseIndex].Length;
            _board.GetText((int)UI_Game.Texts.DialogueText1 + _chat).text = _texts[_sentenseIndex];
            return;
        }

        Managers.Sound.Play(Define.Sound.Effect, "uiTouch");

        _sentenseIndex++;
        _charIndex = 0;
        _complete = false;
        if (_sentenseIndex >= _texts.Count)
        {
            _dialogue = false;
            Utils.MakePopupCloseSequence(_board.GetObject((int)UI_Game.GameObjects.Dialogue1 + _chat)).Restart();
            return;
        }
    }

    void OnPauseButton()
    {
        Managers.Sound.Play(Define.Sound.Effect, "popup");
        Managers.UI.ShowPopupUI<UI_Pause>();
        Time.timeScale = 0;
    }
}
