using System;
using System.Collections;
using System.Collections.Generic;
using _StickBlast.Script.Game.Sticks;
using Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _StickBlast.Script.Game.Management
{
    public class StickManager : MonoBehaviour,IEventListener
    {
        [Header("------ Stick Manager Elements ------")] 
        [SerializeField] private StickData stickData;
        [SerializeField] private List<Transform> stickSpawnPoints;
        [SerializeField] private List<GameObject> spawnedSticks;
        [SerializeField] private int spawnedSticksCount;
        
        [Header("------ Stick Spawn Animation Settings ------")]
        [SerializeField] private Vector3 spawnOffset = new Vector3(1000f, 0f, 0f);
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private float spawnDelay = .5f;

        private void OnEnable()
        {
            EventManager.Instance.RegisterListener(this);
        }

        private void OnDisable()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        private void Start()
        {
            StartCoroutine(SpawnSticksAtPosition());
        }

        public void DecreaseItemsCount()
        {
            spawnedSticksCount--;
            StartCoroutine(SpawnSticksAtPosition());
        }

        public void ClearItems()
        {
            for (int i = spawnedSticks.Count - 1; i >= 0; i--)
            {
                if (spawnedSticks[i])
                {
                    Destroy(spawnedSticks[i]);
                }
                spawnedSticks.RemoveAt(i);
            }

            spawnedSticksCount = 0;
        }

        private IEnumerator SpawnSticksAtPosition()
        {
            if (spawnedSticksCount <= 0)
            {
                for (int i = 0; i < stickSpawnPoints.Count; i++)
                {
                    GameObject stickPrefab = GetStickPrefabByIndex(i);
                    if (stickPrefab)
                    {
                        Vector3 spawnPosition = stickSpawnPoints[i].position;
                
                        GameObject spawnedStick = Instantiate(stickPrefab, spawnPosition, Quaternion.Euler(0f, 0f, -90f), stickSpawnPoints[i]);
                
                        spawnedStick.transform.localScale = Vector3.zero;

                        spawnedStick.transform.DOScale(new Vector3(1.4f, 1.45f, 0f), animationDuration)
                            .SetEase(Ease.OutBack);
                
                        spawnedSticks.Add(spawnedStick);
                        spawnedSticksCount++;
                
                        yield return new WaitForSeconds(spawnDelay);
                    }
                }
            }
        }

        private GameObject GetStickPrefabByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return stickData.singleMoveableObjects[Random.Range(0, stickData.singleMoveableObjects.Count)];
                case 1:
                    return stickData.doubleMoveableObjects[Random.Range(0, stickData.doubleMoveableObjects.Count)];
                case 2:
                    return stickData.moreMoveableObjects[Random.Range(0, stickData.moreMoveableObjects.Count)];
                default:
                    Debug.LogWarning($"Undefined stick spawn index: {index}");
                    return null;
            }
        }
        
        
    }
}