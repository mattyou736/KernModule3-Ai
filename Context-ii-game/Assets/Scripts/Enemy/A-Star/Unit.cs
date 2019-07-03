using UnityEngine;
using System.Collections;
using Panda;

public class Unit : MonoBehaviour
{

    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public Transform target, playerTarget;
    public Transform[] patrolTarget;
    public float speed = 20;
    public float turnSpeed = 3;
    public float turnDst = 5;
    public float stoppingDst = 0;

    Path path;

    public int patrolCounter;

    public bool canMove,alerted;

    public GameObject[] enemies;

    [Task]
    public bool playerIsTarget;

    [Task]
    public bool stunned;

    [Task]
    void StunnedTask()
    {
        alerted = false;
        canMove = false;
        StartCoroutine(StunnedState());
        Task.current.Succeed();   
    }

    [Task]
    void TargetPlayer()
    {
        canMove = true;
        target = playerTarget;
        speed = 6;
        Task.current.Succeed();
    }

    [Task]
    void Alert()
    {
        target = patrolTarget[2];
        if (!alerted)
        {
            
            foreach(GameObject enemies in enemies)
            {
                enemies.GetComponent<Unit>().playerIsTarget = true;
            }
            alerted = true;
        }
        Task.current.Succeed();
    }

    [Task]
    void SetTarget()
    {
        canMove = true;
        if (patrolCounter == 1)
        {
            target = patrolTarget[0];
        }
        else if (patrolCounter == 2)
        {
            target = patrolTarget[1];
            patrolCounter = 0;
        }


        Task.current.Succeed();
    }

    [Task]
    void StartMoving()
    {
        StopCoroutine(UpdatePath());
        StartCoroutine(UpdatePath());
        Task.current.Succeed();
    }

    [Task]
    void nextPoint()
    {
        patrolCounter++;
        Task.current.Succeed();
    }



    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new Path(waypoints, transform.position, turnDst, stoppingDst);

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {

        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);
            print(((target.position - targetPosOld).sqrMagnitude) + "    " + sqrMoveThreshold);
            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {

        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.lookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            if (canMove)
            {

          
                while (path.turnBoundaries[pathIndex].HasCrossedLine(pos2D))
                {
                    if (pathIndex == path.finishLineIndex)
                    {
                        followingPath = false;
                        break;
                    }
                    else
                    {
                        pathIndex++;
                    }
                }

                if (followingPath)
                {

                    if (pathIndex >= path.slowDownIndex && stoppingDst > 0)
                    {
                        speedPercent = Mathf.Clamp01(path.turnBoundaries[path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                        if (speedPercent < 0.01f)
                        {
                            followingPath = false;

                        }
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(path.lookPoints[pathIndex] - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                    transform.Translate(Vector3.forward * Time.deltaTime * speed * speedPercent, Space.Self);
                }
            }

            yield return null;

        }
    }

    IEnumerator StunnedState()
    {
        
        
        yield return new WaitForSeconds(5);
        stunned = false;
        target = patrolTarget[0];
        

    }

    /*public void SetPandaConditionsToFalse()
    {
        stunned = false;
        playerIsTarget = false;
    }*/

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            path.DrawWithGizmos();
        }
    }
}
