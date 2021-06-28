using UnityEngine;

public class EnemyPointPatrolling : MonoBehaviour
{
    int currentPatrolPoint;
    float movementSpeed = 1f;
    public bool isPatrolling;
    Animator anim;
    public Transform[] patrolPoints;


    private void Start()
    {
        currentPatrolPoint = 0;
        anim = GetComponent<Animator>();
    }

    public void PatrollingBetweenPoints()
    {
        if (patrolPoints.Length <= 1 || !isPatrolling)
        {
            anim.SetBool("isWaiting", true);
        }
        else
        {
            anim.SetBool("isWaiting", false);
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolPoint].position, movementSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.2f)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
            }

            if (patrolPoints[currentPatrolPoint].transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void Update()
    {
        PatrollingBetweenPoints();
    }
}
