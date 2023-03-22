using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static GDev.Helper;

namespace GDev
{
    public class Character : MonoBehaviour
    {
        NavMeshAgent agent;
        Animator animator;

        [Header("Character Marker")]
        [SerializeField] SpriteRenderer characterSelecterMarkerPrefab;

        [Header("Effects")]
        [SerializeField] ParticleSystem sparks;

        bool CanMove = true;

        void Start()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            animator.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude));
        }

        #region character selection
        public void SelectCharacter()
        {
            characterSelecterMarkerPrefab.enabled = true;
            characterSelecterMarkerPrefab.transform.DOScale(0.1f, .8f).SetEase(Ease.OutBack);
        }

        public void DeselectCharacter()
        {
            characterSelecterMarkerPrefab.transform.DOScale(0f, .8f).SetEase(Ease.OutBack).OnComplete(() => { characterSelecterMarkerPrefab.enabled = false; });
        }
        #endregion

        #region Character Jobs
        public void DoJob(JobType type, Vector3 destination, Resource resource = null, Building building = null)
        {
            if (type == JobType.None)
                return;

            if (type == JobType.Move)
            {
                SetDestination(destination,2f);
            }
            else if (type == JobType.TakeResources)
            {
                TakeResocurces(resource);
            }
            else if (type == JobType.BuildingItem)
            {
                DoBuilding(building);
            }
        }
        private void SetDestination(Vector3 destination,float range)
        {
            Vector3 offsetDestination = GetPositionAroundObject(destination, range);
            if (CanMove)
                agent.destination = offsetDestination;
        }
        private async void TakeResocurces(Resource resource)
        {
            Resource currentResource = resource;
            SetDestination(resource.transform.position, 2f);
            CanMove = false;
            await Helper.WaitUntil(CalculateDistanceFromTarget);
            transform.rotation = Quaternion.LookRotation((resource.transform.position - transform.position).normalized);
            while (!currentResource.IsDied())
            {
                animator.SetTrigger("DoJob");
                await Helper.WaitForSecond(1200);
                sparks.Play();
                if (currentResource)
                    currentResource.Tick(10);

            }
            CanMove = true;
        }
        private async void DoBuilding(Building building)
        {

            Building currentBuilding = building;
            SetDestination(building.transform.position,currentBuilding.range);
            CanMove = false;
            await Helper.WaitUntil(CalculateDistanceFromTarget);
            transform.rotation = Quaternion.LookRotation((currentBuilding.transform.position - transform.position).normalized);
            while (!currentBuilding.IsBuild())
            {
                animator.SetTrigger("DoJob");
                await Helper.WaitForSecond(1200);
                sparks.Play();
                if (currentBuilding)
                    currentBuilding.Tick();
            }
            CanMove = true;
        }
        #endregion

        #region Utils
        private bool CalculateDistanceFromTarget()
        {
            float distanceBetweenPoints = (agent.destination - transform.position).magnitude;

            if (distanceBetweenPoints <= agent.stoppingDistance && !agent.pathPending)
            {
                if (agent.velocity == Vector3.zero)
                    return true;
            }
            return false;
        }
        private Vector3 GetPositionAroundObject(Vector3 orignalPosition,float radius)
        {
            Vector3 jobPosition = orignalPosition;
            Vector2 randomPosition = Random.insideUnitCircle.normalized * radius;
            jobPosition.x += randomPosition.x;
            jobPosition.z += randomPosition.y;
            return jobPosition;
        }
        #endregion
    }
}

