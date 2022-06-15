using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rogue.Stats;
using System;
using UnityEngine.SceneManagement;

namespace Rogue.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints;
        private MapManager mapManager;

        private GameObject gameManager;
        private VisionManager visionManager;

        sfxPlayer sfx;

        private void Awake()
        {
            mapManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>();

            gameManager = GameObject.FindGameObjectWithTag("GameManager");
            visionManager = gameManager.GetComponent<VisionManager>();

            sfx = GameObject.FindGameObjectWithTag("SFXplayer").GetComponent<sfxPlayer>();
        }

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        public float GetHealth()
        {
            return healthPoints;
        }

        public float GetBaseHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stats.Stats.Health);
        }

        public void Heal(int healPoint)
        {
            healthPoints = Math.Min(GetComponent<BaseStats>().GetStat(Stats.Stats.Health), healthPoints + healPoint);
        }

       /* public void TakeDamage(GameObject attacker, float damage)
        {
            StartCoroutine(AnimateTakeDamage());
            GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText("Poison", Color.green);

            healthPoints = Mathf.Max(0, healthPoints - damage);
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(attacker);
            }
        }*/

        public void TakeDamage(float damage,Color color)
        {
            if (healthPoints == 0)
            {
                return;
            }
                StartCoroutine(AnimateTakeDamage());
            GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText(string.Format("{0:0}", damage), color);
            healthPoints = Mathf.Max(0, healthPoints - damage);
            if (healthPoints == 0)
            {
                Die(null);
            }
        }

        public void TakePercentageDamage(float percent, Color color)
        {
            if (healthPoints == 0)
            {
                return;
            }
            int damage = (int) (GetBaseHealth() * percent / 100);
            StartCoroutine(AnimateTakeDamage());
            GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText(string.Format("{0:0}", damage), color);
            healthPoints = Mathf.Max(0, healthPoints - damage);
            if (healthPoints == 0)
            {
                Die(null);
            }
        }

        public void TakeDamage(float damage)
        {
            if (healthPoints == 0)
            {
                return;
            }
            StartCoroutine(AnimateTakeDamage());
            GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText(string.Format("{0:0}", damage), Color.red);
            healthPoints = Mathf.Max(0, healthPoints - damage);
            if (healthPoints == 0)
            {
                Die(null);
            }
        }

        public void TakeBossDamage(float damage)
        {
            if (healthPoints == 0)
            {
                return;
            }
            StartCoroutine(AnimateTakeDamage());
            GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText(string.Format("{0:0}", damage), Color.red);
            healthPoints = Mathf.Max(0, healthPoints - damage);
            if (healthPoints == 0)
            {
                DieByBoss();
            }
        }

        private void DieByBoss()
        {
            if (GetComponent<Movement>() != null && GetComponent<Movement>().GetIsMoving())
            {
                mapManager.UpdateMap(GetComponent<Movement>().GetExpectedPos(), 1, null);
            }
            else
            {
                mapManager.UpdateMap(transform.position, 1, null);
            }

            if (visionManager.IsInPlayerVision(transform.position))
            {
                sfx.PlayDie();
            }

            if (GetComponent<Player>() != null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
                return;
            }

            GetComponent<MonsterDropper>().RandomDrop();

            StartCoroutine(DestroyAfterDead());
     
        }

        public void TakeDamage(float damage,GameObject attacker)
        {
            if (healthPoints == 0)
            {
                return;
            }

            StartCoroutine(AnimateTakeDamage());
            GetComponentInChildren<WorldSpaceTextSpawner>().spawnWorldSpaceText(string.Format("{0:0}", damage), Color.red);
            healthPoints = Mathf.Max(0, healthPoints - damage);
            if (healthPoints == 0)
            {
                
                AwardExperience(attacker);
                Die(attacker);
            }
        }

        private void AwardExperience(GameObject attacker)
        {
            Experience experience = attacker.GetComponent<Experience>();
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stats.Stats.ExperienceReward));
        }

        private void Die(GameObject attacker)
        {
            if (GetComponent<Movement>() != null && GetComponent<Movement>().GetIsMoving())
            {
                mapManager.UpdateMap(GetComponent<Movement>().GetExpectedPos(), 1, null);
            }
            else
            {
                mapManager.UpdateMap(transform.position, 1, null);
            }

            if (visionManager.IsInPlayerVision(transform.position))
            {
                sfx.PlayDie();
            }

            if (attacker != null) {
                Enemy enemy = attacker.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (enemy.GetCurrentHuntingTarget()==gameObject) {
                        Debug.Log("GiveEgg");
                        enemy.CancelHunting();
                        enemy.GiveEgg();
                    }
            }
            }
            if (attacker != null)
            {
                 if(attacker.GetComponent<Player>()!=null)GetComponent<MonsterDropper>().RandomDrop();
            }
            if (GetComponent<Player>() == null)
            {
                StartCoroutine(DestroyAfterDead());
            }
            if (GetComponent<Player>() != null)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +2);
            }
        }

        private IEnumerator DestroyAfterDead()
        {
            Debug.Log("Destroy");
            Material mat = GetComponentInChildren<SpriteRenderer>().material;
            float fadeValue = 1f;
            while (fadeValue > 0) {
                fadeValue -= 0.01f;
                mat.SetFloat("_Fade", fadeValue);
                yield return null;
            }
            Destroy(gameObject);
        }

        public bool IsDead()
        {
            if (healthPoints == 0) return true;
            else return false;
        }

        private IEnumerator AnimateTakeDamage()
        {
            if (visionManager.IsInPlayerVision(transform.position))
            {
                SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
                renderer.color = Color.red;
                yield return new WaitForSeconds(0.25f);
                renderer.color = Color.white;
            }
        }
    }
}