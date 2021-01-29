using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private KeyCode _switchDirKey;
    [SerializeField]
    private Unit _follower;
    [SerializeField]
    private Vector2 _offset;
    [SerializeField]
    private float _followSpeed = 0.1f;
    private Vector3 m_desPos;
    private int m_direction;
    public Vector3 desPos => m_desPos;
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        m_direction = _follower.direction;
    }

    private void Update()
    {
        if (_follower == null || !_follower.isAlive)
        {
            return;
        }

        if (Input.GetKeyDown(_switchDirKey))
        {
            m_direction *= -1;
        }
    }

    private void FixedUpdate()
    {
        m_desPos = _follower.position;

        m_desPos.x += m_direction * _offset.x;
        m_desPos.y += _offset.y;
        m_desPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, m_desPos, _followSpeed);
    }

    public static void DoShake(float duration = 0.1f, float strength = 0.3f)
    {
        if (Mathf.Abs(instance.desPos.x - instance.transform.position.x) < 1.0f &&
           Mathf.Abs(instance.desPos.y - instance.transform.position.y) < 1.0f)
        {

            Camera.main.DOShakePosition(duration, strength, 60);
        }
    }
}