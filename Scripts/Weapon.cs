using System;
using DG.Tweening;
using System.Collections;
using Cinemachine;
using ObjectPooling;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Player Valuse

    private Agent _owner;
    [field: SerializeField] public WeaponStat _stat;
    [SerializeField] private InputReader _inputReader;

    #endregion

    #region Gun Values

    [Header("Gun Setting")]
    [SerializeField] private Transform _gun;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _casingTrm;
    [SerializeField] private MuzzleFlame _muzzleFlame;

    [Header("Gun Tween")]
    public float gunRotationDuration = 0.12f;
    public float endDuration = 0.3f;
    public float gunTweenBackDuration = 0.1f;
    public float gun_power = -5;
    public float recoilPosPower = -0.07f;

    private Sequence _gunSequence;
    private bool isRecoilTween = false;

    [Header("Muzzle Tween")]
    public Vector3 muzzle_Str;
    public int muzzle_Vibrato;
    public float muzzle_duration;
    private Sequence _muzzleSequence;

    private float _cashKonckPower;
    private int _currentMagazine;
    private float _firerateFloat;
    private float _reloadFloat;
    private bool _isAttack = false;
    private bool _isFire = false;
    private bool _isReload = false;
    private Ray _gunRay;

    #endregion

    #region Camera Valuse

    [Header("Cam Settings")]
    [SerializeField] private CinemachineVirtualCamera _virCam;
    private CinemachineBasicMultiChannelPerlin _perlin;

    [Header("Cam Tween Settings")]
    public float DG_ShakePositionDuration = 0.5f;
    public int DG_Vibrato = 10;
    public Vector3 Cam_Strength = Vector3.one;
    private Sequence _CamSequence;

    #endregion

    #region  Event

    public event Action OnFireFlame;
    public event Action OnFireEvent;
    public event Action OnReloadingEvent;

    #endregion

    public void InitCaster(Agent agent)
    {
        _owner = agent;
    }

    private void OnEnable()
    {
        // Handle
        _inputReader.OnReloadEvent += HandleReload;
        _inputReader.OnFireEvent += HandleFire;
    }

    void Awake()
    {
        _perlin = _virCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _firerateFloat = _stat.firerate.GetValue() * 0.01f;
        _reloadFloat = _stat.reloading.GetValue() * 0.01f;
        _currentMagazine = _stat.maxMagazine.GetValue();
        _muzzleFlame.gameObject.SetActive(false);
        _cashKonckPower = _stat.knockBackPower.GetValue() / 10f;
    }

    void Update()
    {
        _gunRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        Attack();
    }

    private void OnDisable()
    {
        _inputReader.OnReloadEvent -= HandleReload;
    }

    private void HandleFire(bool isPress)
    {
        _isAttack = isPress;
    }

    private void Attack()
    {
        if (_isAttack == false || _isFire || _isReload || _currentMagazine <= 0) return;
        StartCoroutine(Shoot());
    }

    private void HandleReload()
    {
        // TODO : Create Reload Animation
        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        _isReload = true;
        yield return new WaitForSeconds(_reloadFloat);
        OnReloadingEvent?.Invoke();
        _currentMagazine = _stat.maxMagazine.GetValue();
        _isReload = false;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Shoot()
    {
        _isFire = true;
        _currentMagazine--;

        OnFireEvent?.Invoke();
        _muzzleFlame.gameObject.SetActive(true);
        _muzzleFlame.PlayParticle();

        RaycastHit[] hitInfo = new RaycastHit[3];
        int hit = Physics.RaycastNonAlloc(_gunRay, hitInfo, _stat.range.GetValue(), _enemyLayer);
        if (hit >= 1)
        {
            ZombieHit zombieHit = PoolManager.Instance.Pop(PoolingType.ZombieHit) as ZombieHit;
            if (zombieHit != null) zombieHit.transform.position = hitInfo[0].point;

            if (hitInfo[0].collider.TryGetComponent<IDamageable>(out IDamageable health))
            {
                int damage = _owner.Stat.GetDamage(); // Onwer Damage
                health.ApplyDamage(damage, hitInfo[0].point, hitInfo[0].normal, _cashKonckPower / 10, _owner, DamageType.Range, true);
            }
        }

        Recoil();
        MuzzleTween();
        CamTween();
        CreateBullet();

        yield return new WaitForSeconds(_firerateFloat);
        _muzzleFlame.gameObject.SetActive(false);
        _isFire = false;
    }

    private void MuzzleTween()
    {
        _muzzleSequence = DOTween.Sequence()
            .Append(_muzzle.DOShakeRotation(muzzle_duration, muzzle_Str, muzzle_Vibrato, 1, false));
    }

    private void CamTween()
    {
        _CamSequence = DOTween.Sequence()
            .Append(_virCam.transform.DOShakePosition(DG_ShakePositionDuration, Cam_Strength, DG_Vibrato, 1, false))
            .OnComplete(() =>
            {
                _virCam.transform.localRotation = Quaternion.Euler(Vector3.zero);
            });
        _virCam.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void Recoil()
    {
        if (isRecoilTween) return;
        isRecoilTween = true;

        _gunSequence = DOTween.Sequence()
            .Append(_gun.DOLocalRotate(new Vector3(gun_power, 0, 0), gunRotationDuration).SetEase(Ease.Linear))
            .Join(_gun.DOLocalMoveZ(recoilPosPower, gunTweenBackDuration).SetEase(Ease.InOutQuad))
            .OnComplete(() =>
            {
                _gun.DOLocalMoveZ(0, gunTweenBackDuration).SetEase(Ease.InOutQuad);
                _gun.DOLocalRotate(new Vector3(0, 0, 0), endDuration);
                isRecoilTween = false;
            });
    }

    private void PerlinCam(int amplitude = 0, int frequency = 0)
    {
        _perlin.m_AmplitudeGain = amplitude;
        _perlin.m_FrequencyGain = frequency;
    }

    private void CreateBullet()
    {
        PoolObjTargetToPos(PoolingType.Bullet, _muzzle);
        PoolObjTargetToPos(PoolingType.CasingBullet, _casingTrm);
        OnFireFlame?.Invoke();
    }

    private void PoolObjTargetToPos(PoolingType poolType, Transform target)
    {
        PoolableMono obj = PoolManager.Instance.Pop(poolType);
        if (obj != null) TargetToPos(obj.transform, target);
    }

    private void TargetToPos(Transform pos, Transform target)
    {
        pos.position = target.position;
        pos.rotation = target.rotation;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_gunRay.origin, _gunRay.direction * _stat.range.GetValue());
    }
#endif
}

