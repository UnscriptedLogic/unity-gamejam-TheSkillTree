using System;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using UnscriptedLogic;

namespace Game.Spawning
{
    public class WaveSpawner : MonoBehaviour
    {
        private enum SpawnerStates
        {
            Stopped,
            SpawningWave,
            SpawningSegment,
            Waiting,
            Preparation,
            FinalWait
        }

        [SerializeField] private WavesSO wavesSO;
        [SerializeField] private float startDelay = 5f;
        [SerializeField] private int waveIndex;
        [SerializeField] private Transform[] spawnLocations;
        [SerializeField] private Transform cam;
        [SerializeField] private Transform target;
        [SerializeField] private SceneChanger changer;

        private Wave currWave;
        private WaveSegment currSegment;
        private SpawnerStates currentState = SpawnerStates.Stopped;

        private float _interval;
        private int _spawnAmount;

        private int segmentIndex;
        private int waveCount;
        private bool stopSpawning = true;

        public int WaveCount => waveCount + 1;
        public WavesSO WavesSO => wavesSO;
        public Action<int, int> OnWaveCompleted;
        public Action<int, int> OnWaveStarted;
        public Action OnCompleted;
        public Action OnBaseHealthDepleted;

        private void Start()
        {
            StartSpawner();
        }

        public void StartSpawner()
        {
            stopSpawning = false;
            currWave = wavesSO.Waves[waveIndex];
            currSegment = currWave.waveSegments[segmentIndex];
            waveCount = 0;

            SwitchState(SpawnerStates.Preparation);
        }

        private void Update()
        {
            if (stopSpawning)
                return;

            UpdateState();
        }

        private void EnterState()
        {
            switch (currentState)
            {
                case SpawnerStates.Stopped:
                    break;
                case SpawnerStates.SpawningWave:
                    currWave = wavesSO.Waves[waveIndex];
                    break;
                case SpawnerStates.SpawningSegment:
                    OnWaveStarted?.Invoke(waveIndex, wavesSO.Waves.Length - 1);
                    currSegment = wavesSO.Waves[waveIndex].waveSegments[segmentIndex];
                    _spawnAmount = 0;
                    break;
                case SpawnerStates.Waiting:
                    _interval = currSegment.segmentInterval;
                    break;
                case SpawnerStates.Preparation:
                    _interval = startDelay;
                    break;
                case SpawnerStates.FinalWait:
                    break;
                default:
                    break;
            }
        }

        private void UpdateState()
        {
            switch (currentState)
            {
                case SpawnerStates.Stopped:
                    break;
                case SpawnerStates.SpawningWave:
                    if (_interval <= 0f)
                    {
                        SwitchState(SpawnerStates.SpawningSegment);
                        break;
                    }
                    else
                    {
                        _interval -= Time.deltaTime;
                        if (transform.childCount == 0 && _interval > 2f)
                        {
                            _interval = 2f;
                        }
                    }
                    break;
                case SpawnerStates.SpawningSegment:
                    if (_spawnAmount >= currSegment.amount)
                    {
                        SwitchState(SpawnerStates.Waiting);
                        break;
                    }

                    if (_interval <= 0f)
                    {
                        SpawnEnemy(currSegment.enemyToSpawn);
                        _spawnAmount++;
                        _interval = currSegment.interval;
                    }
                    else
                    {
                        _interval -= Time.deltaTime;
                    }
                    break;
                case SpawnerStates.Waiting:
                    if (_interval <= 0f)
                    {
                        segmentIndex++;
                        if (segmentIndex >= wavesSO.Waves[waveIndex].waveSegments.Length)
                        {
                            segmentIndex = 0;
                            waveIndex++;
                            waveCount++;
                            OnWaveCompleted?.Invoke(waveIndex, wavesSO.Waves.Length - 1);
                            if (waveIndex >= wavesSO.Waves.Length)
                            {
                                SpawningCompleted();
                                break;
                            }

                            _interval = currWave.waveInterval;
                            SwitchState(SpawnerStates.SpawningWave);
                            break;
                        }

                        SwitchState(SpawnerStates.SpawningSegment);
                    }
                    else
                    {
                        _interval -= Time.deltaTime;
                    }
                    break;
                case SpawnerStates.Preparation:
                    if (_interval <= 0f)
                    {
                        SwitchState(SpawnerStates.SpawningWave);
                    }
                    else
                    {
                        _interval -= Time.deltaTime;
                    }
                    break;
                case SpawnerStates.FinalWait:
                    if (transform.childCount <= 0)
                    {
                        OnCompleted?.Invoke();
                        GameManager.hasWon = true;
                        changer.ChangeScene(0);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ExitState()
        {
            switch (currentState)
            {
                case SpawnerStates.Stopped:
                    break;
                case SpawnerStates.SpawningWave:
                    break;
                case SpawnerStates.SpawningSegment:
                    break;
                case SpawnerStates.Waiting:
                    break;
                case SpawnerStates.Preparation:
                    break;
                case SpawnerStates.FinalWait:
                    break;
                default:
                    break;
            }
        }

        private void SpawningCompleted()
        {
            SwitchState(SpawnerStates.FinalWait);
        }

        private void SwitchState(SpawnerStates newState)
        {
            ExitState();
            currentState = newState;
            EnterState();
        }

        public void StopSpawner() => stopSpawning = true;
        public void ContinueSpawner() => stopSpawning = false;

        public void ResetSpawner()
        {
            waveIndex = 0;
            segmentIndex = 0;
            _interval = 0f;

            currWave = wavesSO.Waves[waveIndex];
            currSegment = currWave.waveSegments[segmentIndex];

            stopSpawning = false;

            SwitchState(SpawnerStates.SpawningWave);
        }

        private void SpawnEnemy(GameObject enemyToSpawn)
        {
            Vector3 position = RandomLogic.FromArray(spawnLocations).position;
            GameObject unitObject = Instantiate(enemyToSpawn, position, Quaternion.identity);
            Unit unit = unitObject.GetComponent<Unit>();
            unit.Initialize(target);
        }

        public void ClearEntities()
        {

        }
    }
}