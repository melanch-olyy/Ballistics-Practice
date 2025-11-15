using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Targets
{
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject targetPrefab;
        
        [Header("Зона спавна")]
        [SerializeField] private Vector2 spawnAreaSize = new Vector2(10f, 10f);
        [SerializeField] private float spawnDepthOffset  = 0.5f;
        [SerializeField] private float cooldownSpawning;
        
        [Header("Физические параметры мишеней")]
        [SerializeField] private Vector3 initialVelocity = new Vector3(5f, 0, 0);
        [SerializeField, Range(0.01f, 100f)] private float massMin;
        [SerializeField, Range(0.01f, 100f)] private float massMax;
        [SerializeField, Range(0.01f, 2f)] private float radiusMin;
        [SerializeField, Range(0.01f, 2f)] private float radiusMax;
        
        private Vector3 _spawningZoneCenter;

        private void Awake()
        {
            StartCoroutine(SpawnTargetsCoroutine());
        }

        private IEnumerator SpawnTargetsCoroutine()
        {
            while (true)
            {
                float randomLocalX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
                float randomLocalY = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
                
                Vector3 localSpawnPoint = new Vector3(randomLocalX, randomLocalY, spawnDepthOffset);
                Vector3 worldSpawnPoint = transform.TransformPoint(localSpawnPoint);
                
                var target = Instantiate(targetPrefab, worldSpawnPoint, Quaternion.identity);
                
                var radius = Random.Range(radiusMin, radiusMax);
                var mass =  Random.Range(massMin, massMax);

                target.transform.localScale = new Vector3(0.1f, radius, radius);
                
                var rb = target.GetComponent<Rigidbody>();
                rb.mass = mass;
                rb.useGravity = false;
                rb.linearVelocity = initialVelocity;
                
                Destroy(target, 15f);
                yield return new WaitForSeconds(cooldownSpawning);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            
            Matrix4x4 originalMatrix = Gizmos.matrix;
            
            Gizmos.matrix = transform.localToWorldMatrix;
            
            Vector3 gizmoSize = new Vector3(spawnAreaSize.x, spawnAreaSize.y, 0.01f);
            Vector3 gizmoCenter = new Vector3(0, 0, spawnDepthOffset);
            Gizmos.DrawWireCube(gizmoCenter, gizmoSize);
            
            Gizmos.matrix = originalMatrix;
        }
    }
}