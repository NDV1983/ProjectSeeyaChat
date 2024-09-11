using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerService : MonoBehaviourPun
{
    private Vector3 targetPosition;
    private Vector3 previousPosition;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        previousPosition = transform.position;

        if (photonView.IsMine)
        {
            Camera.main.GetComponent<CameraFollowView>().StartFollowing(this.gameObject);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetTargetPosition();
            }
        }

        UpdateAnimationState();
    }

    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            targetPosition = hit.point;
            navMeshAgent.SetDestination(targetPosition);
        }
    }

    void UpdateAnimationState()
    {
        // Compare current position with previous position
        Vector3 currentPosition = transform.position;
        if (Vector3.Distance(currentPosition, previousPosition) > 0.01f)
        {
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
        previousPosition = currentPosition;
    }
}
