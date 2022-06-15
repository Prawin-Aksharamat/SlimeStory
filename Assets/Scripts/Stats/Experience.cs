using System;
using UnityEngine;

namespace Rogue.Stats
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePoints = 0;
        //public delegate void ExperienceGainDelegate();
        //Action = predefined delegate without return type
        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public float GetPoints()
        {
            return experiencePoints;
        }
    }
}