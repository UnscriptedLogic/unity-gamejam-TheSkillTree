using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Spawning
{
    [CreateAssetMenu(fileName = "New Level Wave", menuName = "ScriptableObjects/New Wave Object")]
    public class WavesSO : ScriptableObject
    {
        [SerializeField] private Wave[] waves;

        public Wave[] Waves => waves;
    }

    [Serializable]
    public class WaveSegment
    {
        public GameObject enemyToSpawn;
        public int amount;
        public float interval;
        public float segmentInterval;
    }

    [Serializable]
    public class Wave
    {
        public WaveSegment[] waveSegments;
        public float waveInterval = 5f;
    }
}