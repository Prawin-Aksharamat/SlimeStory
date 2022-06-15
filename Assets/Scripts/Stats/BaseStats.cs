using Rogue.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rogue.Stats
{

    public class BaseStats : MonoBehaviour
    {
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifier = false;

        private bool alreadyLevelUp=false;
        private int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            experience.onExperienceGained += UpdateLevel;

            Material mat = GetComponentInChildren<SpriteRenderer>().material;
            
            if (currentLevel == 2)
            {
                mat.SetFloat("_OutlineThickness", 0.3f);
                mat.SetColor("_OutlineColor", new Color32(227, 255, 0, 255));
                mat.SetColor("_OutlineColor2", new Color32(255, 197, 0, 255));
            }
            else if (currentLevel == 3)
            {
                mat.SetFloat("_OutlineThickness", 0.3f);
                mat.SetColor("_OutlineColor", new Color32(255, 0, 3, 255));
                mat.SetColor("_OutlineColor2", new Color32(225, 50, 50, 255));
            }

        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
            }

        }

        private void LevelUpEffect()
        {
            GetComponent<Health>().Heal(int.MaxValue);

            Material mat = GetComponentInChildren<SpriteRenderer>().material;
            mat.SetFloat("_OutlineThickness", 0.3f);

            if (currentLevel==2)
            {
                mat.SetColor("_OutlineColor", new Color32(227, 255, 0, 255));
                mat.SetColor("_OutlineColor2", new Color32(255, 197, 0, 255));
            }
            else if(currentLevel == 3)
            {
                mat.SetColor("_OutlineColor", new Color32(255, 0, 3, 255));
                mat.SetColor("_OutlineColor2", new Color32(225, 50, 50, 255));
            }
            if (levelUpParticleEffect == null) return;
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stats stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1+ GetPercentageModifier(stat)/100) ;
        }

        private float GetPercentageModifier(Stats stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetBaseStat(Stats stat)
        {
            return progression.GetStat(stat,characterClass,GetLevel());
        }

        private float GetAdditiveModifier(Stats stat)
        {
            if (!shouldUseModifier) return 0;
            float total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        public int GetLevel()
        {
            if (currentLevel < 1)
            {
                return CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
        {
            float currentXP = GetComponent<Experience>().GetPoints();
            int penultimateLevel = progression.GetLevels(Stats.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel;level++){
                float XPToLevelUp = progression.GetStat(Stats.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}
