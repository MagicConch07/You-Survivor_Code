using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _virtualCam;

    private FPSInput.PlayerActions _inputAction;
    private Rigidbody _myrigid;
    private bool _isCursor = false;

    public float speed = 12f;

    [SerializeField] private float _jumpPower = 5f;
    [SerializeField] private float _rayDistance = 1.2f;

    [SerializeField] private LayerMask _groundLayer;
    void Awake()
    {
        _myrigid = GetComponent<Rigidbody>();
        _inputAction = _inputReader.FPSInputInstance.Player;

        _inputReader.OnSettingsEvent += OnSettingsHandle;
        _inputReader.OnSprintEvent += OnSprintHandle;
        _inputReader.OnSitEvent += OnSitHandle;
        _inputReader.OnJumpEvent += OnJumpHandle;
    }

    void OnDisable()
    {
        _inputReader.OnSettingsEvent -= OnSettingsHandle;
        _inputReader.OnSprintEvent -= OnSprintHandle;
        _inputReader.OnSitEvent -= OnSitHandle;
        _inputReader.OnJumpEvent -= OnJumpHandle;
    }

    private void OnSettingsHandle(bool isPress)
    {
        _isCursor = !isPress;
    }

    private void OnSprintHandle(bool isPress)
    {
        if (isPress == false)
        {
            speed = 12;
            return;
        }

        speed = 20;
    }

    private void MoveSit(bool isPress)
    {
        if (isPress == false)
        {
            _virtualCam.transform.localPosition = new Vector3(_virtualCam.transform.localPosition.x, _virtualCam.transform.localPosition.y, _virtualCam.transform.localPosition.z);
            return;
        }

        _virtualCam.transform.localPosition = new Vector3(_virtualCam.transform.localPosition.x, -_virtualCam.transform.localPosition.y, _virtualCam.transform.localPosition.z);
    }

    private void OnSitHandle(bool isPress)
    {
        if (isPress == false)
        {
            _virtualCam.transform.localPosition = new Vector3(_virtualCam.transform.localPosition.x, 2f, _virtualCam.transform.localPosition.z);
            return;
        }

        _virtualCam.transform.localPosition = new Vector3(_virtualCam.transform.localPosition.x, 1f, _virtualCam.transform.localPosition.z);
    }

    private void OnJumpHandle()
    {

        RaycastHit[] hits = new RaycastHit[1];
        Physics.RaycastNonAlloc(transform.position, Vector3.down, hits, _rayDistance, _groundLayer);
        int hit = Physics.RaycastNonAlloc(transform.position, Vector3.down, hits, _rayDistance, _groundLayer);
        if (hit != 0)
        {
            _myrigid.velocity = new Vector3(_myrigid.velocity.x, _jumpPower, _myrigid.velocity.z);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Vector3.down);
    }
}
