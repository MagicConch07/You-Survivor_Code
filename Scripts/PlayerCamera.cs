using System.Reflection;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    private FPSInput.PlayerActions _inputAction;

    [SerializeField] private Rigidbody _myrigid;
    private Transform _originTrm;

    // Cam Setting
    [SerializeField] private Transform _eye;
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float cameraRotationLimit;

    public float mouseSpeed = 1f;

    private bool _isHit = false;
    private bool _isSetting = false;

    void Awake()
    {
        _inputAction = _inputReader.FPSInputInstance.Player;
        _originTrm = _eye;
    }

    void OnEnable()
    {
        _inputReader.OnSettingsEvent += HandleSettingEvent;
    }

    void OnDisable()
    {
        _inputReader.OnSettingsEvent -= HandleSettingEvent;
    }

    private void HandleSettingEvent(bool isSetting)
    {
        _isSetting = isSetting;
    }

    void LateUpdate()
    {
        if (_isHit || _isSetting) return;

        CameraRotation();

        float mouseY = _inputAction.MouseView.ReadValue<Vector2>().x * Mathf.Pow(_sensitivity, 2) * mouseSpeed;

        Quaternion rotationYaw = Quaternion.Euler(0.0f, mouseY, 0.0f);

        _myrigid.MoveRotation(_myrigid.rotation * rotationYaw);
    }

    public float DG_Duration = 0.5f;
    public Vector3 DG_Strength = Vector3.one;
    public int DG_Vibrato = 10;

    private Sequence _hitSequence;

    public void HitPlayer()
    {
        if (_isHit) return;

        _isHit = true;
        _hitSequence = DOTween.Sequence()
            .Append(_eye.DOShakeRotation(DG_Duration, DG_Strength, DG_Vibrato, 1f, false))
            .OnComplete(() =>
            {
                _eye = _originTrm;
                _isHit = false;
            });
    }

    private void CameraRotation()
    {
        float _xRotation = _inputAction.MouseView.ReadValue<Vector2>().y * Mathf.Pow(_sensitivity, 2) * mouseSpeed;

        Quaternion localRotation = transform.localRotation;
        Quaternion rotationPitch = Quaternion.Euler(-_xRotation, 0.0f, 0.0f);

        //Local Rotation.

        localRotation *= rotationPitch;

        localRotation = Clamp(localRotation);

        transform.localRotation = localRotation;
    }

    private Quaternion Clamp(Quaternion rotation)
    {
        rotation.Normalize();

        //Pitch.
        float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

        //Clamp.
        pitch = Mathf.Clamp(pitch, -cameraRotationLimit, cameraRotationLimit);

        // 다시 쿼터니언으로 변환
        rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

        //Return.
        return rotation;
    }
}
