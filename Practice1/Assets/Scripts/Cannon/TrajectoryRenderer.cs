using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    [Header("Отрисовка")]
    [SerializeField] private int pointsCount = 50;
    [SerializeField] private float timeStep = 0.1f;
    [SerializeField] private float widthLine = 0.02f;

    [Header("Физика воздуха")]
    [SerializeField] private float mass = 1f;
    [SerializeField, Range(0.01f, 1f)] private float radius = 0.1f;
    [SerializeField] private float dragCoefficient = 0.47f;
    [SerializeField] private float airDensity = 1.225f;
    [SerializeField] private Vector3 wind = Vector3.zero;

    private float _area;
    private LineRenderer _line;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.useWorldSpace = true;
        _line.material = new Material(Shader.Find("Sprites/Default"));
        
        _area = Mathf.PI * radius * radius;
    }

    public void DrawWithAirEuler(Vector3 startPosition, Vector3 startVelocity)
    {
        Vector3 p = startPosition;
        Vector3 v = startVelocity;
        _line.positionCount = pointsCount;

        for (int i = 0; i < pointsCount; i++)
        {
            _line.SetPosition(i, p);

            Vector3 vRel = v - wind;
            float speed = vRel.magnitude;
            Vector3 drag = speed > 1e-6f ? (-0.5f * airDensity * dragCoefficient * _area * speed) * vRel : Vector3.zero;
            Vector3 a = Physics.gravity + drag / mass;

            v += a * timeStep;
            p += v * timeStep;
        }
    }
    
    public void DrawWithAirEuler(float mass, float radius, Vector3 startPosition, Vector3 startVelocity)
    {
        _area = Mathf.PI * radius * radius;
        
        Vector3 p = startPosition;
        Vector3 v = startVelocity;
        _line.positionCount = pointsCount;

        for (int i = 0; i < pointsCount; i++)
        {
            _line.SetPosition(i, p);

            Vector3 vRel = v - wind;
            float speed = vRel.magnitude;
            Vector3 drag = speed > 1e-6f ? (-0.5f * airDensity * dragCoefficient * _area * speed) * vRel : Vector3.zero;
            Vector3 a = Physics.gravity + drag / mass;

            v += a * timeStep;
            p += v * timeStep;
        }
    }

    public void DrawVacuum(Vector3 startPosition, Vector3 startVelocity)
    {
        if (pointsCount < 2) pointsCount = 2;
        _line.positionCount = pointsCount;
        

        for (int i = 0; i < pointsCount; i++)
        {
            float t = i * timeStep;
            Vector3 newPosition = startPosition + t*startVelocity + 0.5f*t*t*Physics.gravity;

            _line.SetPosition(i, newPosition);
        }
    }

}
