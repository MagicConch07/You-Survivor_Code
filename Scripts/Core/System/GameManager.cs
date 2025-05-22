using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private Transform _globalDirectionLight;
    private bool _isClear = false;

    void Start()
    {
        StartCoroutine(GameCoreLight());
    }

    private IEnumerator GameCoreLight()
    {
        while (_globalDirectionLight.rotation.x <= 20f)
        {
            yield return new WaitForSecondsRealtime(1f);
            _globalDirectionLight.DORotate(new Vector3(_globalDirectionLight.eulerAngles.x + 0.22f, _globalDirectionLight.eulerAngles.y, _globalDirectionLight.eulerAngles.z), 0.3f);
        }
        _isClear = true;
        Clear();
    }

    public void Clear()
    {
        UIManager.Instance.ClaerUI();
    }

    public void GameOver()
    {
        UIManager.Instance.DeadUI();
    }
}
