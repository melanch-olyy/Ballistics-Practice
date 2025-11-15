using DefaultNamespace;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using Slider = UnityEngine.UI.Slider;


[RequireComponent(typeof(TrajectoryRenderer))]
public class BallisticController : MonoBehaviour
{
    [Header("Settings Controller")]
    [SerializeField] private Transform launchPoint;
    [SerializeField] private float muzzleVelocity = 20f;

    [SerializeField, Range(5, 85)] private float muzzleAngle = 20f;

    [Header("Physics")] 
    [SerializeField] private float mass = 1f;
    [SerializeField, Range(0.01f, 1f)] private float radius = 0.1f;
    [SerializeField] private float dragCoefficient = 0.47f;
    [SerializeField] private float airDensity = 1.225f;
    [SerializeField] private Vector3 wind = Vector3.zero;
    
    [Header("Стрельба")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField, Range(0.01f, 100f)] private float massMin;
    [SerializeField, Range(0.01f, 100f)] private float massMax;
    [SerializeField, Range(0.01f, 2f)] private float radiusMin;
    [SerializeField, Range(0.01f, 2f)] private float radiusMax;

    private TrajectoryRenderer _trajectoryRender;

    private QuadraticDrag _quadraticDrag;
    
    private Vector3 v0;

    private void Awake()
    {
        _trajectoryRender = GetComponent<TrajectoryRenderer>();
    }

    public void SetVelocity(Slider slider)
    {
        muzzleVelocity = slider.value;
    }

    void Update()
    {
        v0 = CalculateVelocity(muzzleAngle); 
        // _trajectoryRender.DrawWithAirEuler(launchPoint.position, v0);
    }

    public void Fire()
    {
        if (projectilePrefab == null) return;
        
        GameObject newCore = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        var qd = newCore.GetComponent<QuadraticDrag>();
        
        var massRandom = Random.Range(massMin, massMax);
        var radiusRandom = Random.Range(radiusMin, radiusMax);
        _trajectoryRender.DrawWithAirEuler(massRandom, radiusRandom, launchPoint.position, v0);
        
        qd.SetPhysicalParams(massRandom, radiusRandom, dragCoefficient, airDensity, wind, v0);
        
        Destroy(newCore, 4f);
    }

    Vector3 CalculateVelocity(float angle)
    {
        float vx = muzzleVelocity * Mathf.Cos(angle * Mathf.Deg2Rad);
        float vy = muzzleVelocity * Mathf.Sin(angle * Mathf.Deg2Rad);
        return launchPoint.forward * vx + launchPoint.up * vy;
    }
}