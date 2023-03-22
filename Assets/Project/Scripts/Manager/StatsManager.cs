using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDev
{
    public abstract class StatsManager : MonoBehaviour
    {
        protected int MaxHealth = 100;
        public int CurrentHealth;
        protected bool isDied;

        protected virtual void Start()
        {
            CurrentHealth = MaxHealth;
            isDied= false;
        }
        public virtual void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if(CurrentHealth <= 0)
            {
                HandleDeath();
                isDied = true;
                CurrentHealth = 0;
            }
        }
        protected abstract void HandleDeath();
    }
}
