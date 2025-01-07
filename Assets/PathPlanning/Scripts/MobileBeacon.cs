using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class MobileBeacon : Agent
{
    public GameObject path;
    public bool useVectorObs;
    Rigidbody agentRB;
    Transform respwan;
    //Transform goal;
    // Speed of agent rotation.
    public float turnSpeed = 100;
    float last_distance, last_angle;
    // Speed of agent movement.
    public float moveSpeed = 100;
    //public Transform root; // 这是包含所有子级的根对象
    public List<Vector3> positions; // 用于存储位置的列表
    int current_index = 0;

    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        agentRB = GetComponent<Rigidbody>();
        CollectPositions();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        last_distance = (transform.position -  positions[current_index]).sqrMagnitude;
        current_index++;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            var localVelocity = transform.InverseTransformDirection(agentRB.linearVelocity);
            sensor.AddObservation(localVelocity.x);
            sensor.AddObservation(localVelocity.z);
            sensor.AddObservation(gameObject.transform.position);
            sensor.AddObservation(positions[current_index]);
        }
    }

    public void MoveAgent(ActionBuffers actionBuffers)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var continuousActions = actionBuffers.ContinuousActions;

        var forward = Mathf.Clamp(continuousActions[0], -1f, 1f);
        //var right = Mathf.Clamp(continuousActions[1], -1f, 1f);
        var rotate = Mathf.Clamp(continuousActions[1], -1f, 1f);

        dirToGo = transform.forward * forward;
        //dirToGo += transform.right * right;
        rotateDir = transform.up * rotate;
        agentRB.AddForce(dirToGo * moveSpeed, ForceMode.Force);
        agentRB.AddTorque(rotateDir * turnSpeed, ForceMode.Force);
        //transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
        AddReward(0.001f * forward);
        //AchieveGoalPoint();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        //var distance = (transform.position -  positions[current_index]).sqrMagnitude;
        //AddReward(-1f / MaxStep);
        //var nav_reward = 1.2f - ((transform.position - goal.position).sqrMagnitude / (respwan.position - goal.position).sqrMagnitude);
        //AddReward(0.1f * nav_reward);
        MoveAgent(actionBuffers);
        var currentGoal = positions[current_index];
        float distance = (transform.position - currentGoal).sqrMagnitude;
        float crossTrackError = CalculateCrossTrackError(currentGoal);
        float headingError = CalculateHeadingError(currentGoal);

        // Update distance reward
        float distanceReward = -0.001f * distance;
        AddReward(distanceReward);

        // Cross-track and heading error penalties
        AddReward(-crossTrackError - headingError);

        // Check if reached the goal
        if (distance < 1.0f) // Threshold for "reaching" the goal
        {
            SetReward(1.0f); // Assign a positive reward
            current_index++;
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //var continuousActionsOut = actionsOut.ContinuousActions;
        //continuousActionsOut[0] = Input.GetAxis("Horizontal");
        //continuousActionsOut[1] = Input.GetAxis("Vertical");
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.D))
        {
            continuousActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            continuousActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            continuousActionsOut[1] = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            continuousActionsOut[0] = -1;
        }
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    continuousActionsOut[2] = -1;
        //}
        //if (Input.GetKey(KeyCode.E))
        //{
        //    continuousActionsOut[2] = 1;
        //}

    }
    //void AchieveGoalPoint()
    //{
    //    if((transform.position -  positions[current_index]).sqrMagnitude<1)
    //    {

    //    }
    //}
    public override void OnEpisodeBegin()
    {
        agentRB.linearVelocity = Vector3.zero;
        transform.position = positions[current_index];
        //transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

        //SetResetParameters();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            SetReward(-0.1f);
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-0.1f);
            EndEpisode();
        }
        //if (collision.gameObject.CompareTag("goal"))
        //{
        //    SetReward(100f);
        //    EndEpisode();
        //}
    }
    void CollectPositions()
    {
        if (path == null) return; // 确保根对象已经被赋值

        // 通过遍历每个层级，按顺序访问和添加子对象的位置
        foreach (Transform firstLevel in path.transform)
        {
            // 假设每个firstLevel下面的子对象需要按名称排序访问
            foreach (Transform secondLevel in firstLevel)
            {
                // 对secondLevel下的子对象进行迭代
                List<Transform> sortedChildren = new List<Transform>();
                foreach (Transform child in secondLevel)
                {
                    sortedChildren.Add(child);
                }
                // 对子对象按名称进行排序（这假设子对象已被正确命名为0, 1, 2, ...）
                sortedChildren.Sort((a, b) => int.Parse(a.name).CompareTo(int.Parse(b.name)));

                // 添加排序后的位置到列表中
                foreach (Transform child in sortedChildren)
                {
                    positions.Add(child.position);
                }
            }
        }

        // 仅用于调试输出位置信息
        foreach (Vector3 pos in positions)
        {
            Debug.Log("Position: " + pos);
        }
    }
    float CalculateCrossTrackError(Vector3 goalPosition)
    {
        Vector3 pathVector = goalPosition - positions[current_index - 1]; // Vector from previous to goal
        Vector3 beaconVector = transform.position - positions[current_index - 1]; // Vector from previous to beacon
        float crossTrackMagnitude = Vector3.Cross(pathVector.normalized, beaconVector.normalized).magnitude;
        return crossTrackMagnitude * beaconVector.magnitude; // Cross-track error magnitude
    }

    float CalculateHeadingError(Vector3 goalPosition)
    {
        Vector3 desiredHeading = (goalPosition - transform.position).normalized;
        Vector3 currentHeading = transform.forward;
        return 1.0f - Vector3.Dot(desiredHeading, currentHeading); // 1 - cosine of angle between headings
    }

}
