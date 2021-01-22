using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFinale : MonoBehaviour
{
    EnemyStatus _target = null;

    WaitForSeconds waitTime = new WaitForSeconds(1f);

    Camera _cam;
    PlayerMove _player;
    CameraController _camController;

    [Header("UI")]
    [SerializeField] GameObject _canvas = null;
    [SerializeField] Text _txtLine = null;
    [SerializeField] float _colorAlphaSpeed = 0.35f;
    [SerializeField] string[] _line = null;

    [Header("Cam")]
    [SerializeField] Transform _tfCamPos1 = null;
    [SerializeField] Transform _tfCamPos2 = null;
    [SerializeField] Transform _tfCamPos3 = null;
    [SerializeField] Transform _tfCamPos4 = null;
    [SerializeField] float _camSpeed = 0.035f;

    public void SetTargetLink(EnemyStatus target)
    {
        _cam = Camera.main;
        _player = FindObjectOfType<PlayerMove>();
        _camController = _player.GetComponent<CameraController>();
        _target = target;
        StartCoroutine(CheckTriggerActive());
    }

    IEnumerator CheckTriggerActive()
    {
        while (true)
        {
            yield return waitTime;
            if (_target.IsDead())
            {
                StartCoroutine(StartEvent());
                break;
            }
        }
    }


    IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(0.25f);

        // 모든 동작 스톱
        PlayerMove.s_canMove = false;
        InteractionManager._isOpen = true;
        GameHudMenu.instance.HideMenu();
        _camController.SetCanMove(false);


        yield return new WaitForSeconds(1f);

        // 캠 이동
        while (true)
        {
            if (Vector3.SqrMagnitude(_cam.transform.position - _tfCamPos1.position) <= 0.5f)
                if (Quaternion.Angle(_cam.transform.rotation, _tfCamPos1.rotation) <= 0.75f)
                    break;

            _cam.transform.position = Vector3.Lerp(_cam.transform.position, _tfCamPos1.position, _camSpeed);
            _cam.transform.rotation = Quaternion.Lerp(_cam.transform.rotation, _tfCamPos1.rotation, _camSpeed);

            yield return null;
        }

        yield return new WaitForSeconds(2.5f);

        // 캠 전환 1
        _cam.transform.position = _tfCamPos2.position;
        _cam.transform.rotation = _tfCamPos2.rotation;
        yield return new WaitForSeconds(2.5f);

        // 캠 전환 2
        _cam.transform.position = _tfCamPos3.position;
        _cam.transform.rotation = _tfCamPos3.rotation;
        yield return new WaitForSeconds(2.5f); 
        
        // 캠 전환 3
        _cam.transform.position = _tfCamPos4.position;
        _cam.transform.rotation = _tfCamPos4.rotation;
        yield return new WaitForSeconds(2.5f);


        // 페이드 아웃
        ScreenEffect.instance._isFinished = false;
        ScreenEffect.instance.ExecuteFadeOut();
        yield return new WaitUntil(() => ScreenEffect.instance._isFinished);


        // 잠시 후
        yield return new WaitForSeconds(1.5f);


        // 텍스트 연출 시작
        _canvas.SetActive(true);
      
        for(int i = 0; i < _line.Length; i++)
        {
            Color color = _txtLine.color;
            color.a = 0f;
            _txtLine.color = color;
            _txtLine.text = _line[i];

            // 텍스트 보였다가
            while(color.a < 1)
            {
                yield return null;
                color.a += Time.deltaTime * _colorAlphaSpeed;
                _txtLine.color = color;
            }

            // 대기 
            yield return new WaitForSeconds(2.5f);
            
            // 텍스트 사라짐
            while (color.a > 0)
            {
                yield return null;
                if(i == _line.Length - 1)
                    color.a -= Time.deltaTime * (_colorAlphaSpeed * 0.5f);
                else
                    color.a -= Time.deltaTime * _colorAlphaSpeed;
                _txtLine.color = color;
            }

            yield return new WaitForSeconds(2f);
        }

        PlayerMove.s_canMove = true;
        InteractionManager._isOpen = false;
        GameHudMenu.instance.ShowMenu();
        _camController.SetCanMove(true);

        // 요술사 앞으로 맵 이동
        MapManager.instance.ChangeMap("Vegonia");
    }

}
