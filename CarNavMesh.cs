using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CarNavMesh : MonoBehaviour
{
    [SerializeField] private Transform[] target;
    private NavMeshAgent agent;
    public int targetIndex = 0;
    private bool isFinished = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = target[targetIndex].position;
    }

    void Update()
    {
        if (!isFinished)
        {
            if (targetIndex + 1 < target.Length)
                if (transform.position.x == target[targetIndex].position.x || transform.position.z == target[targetIndex].position.z)
                {
                    targetIndex++;
                    agent.destination = target[targetIndex].position;
                }

            //Debug.Log(transform.position + " " + target[target.Length - 1].position);
            if (transform.position.x == target[target.Length - 1].position.x && transform.position.z == target[target.Length - 1].position.z)
            {
                Debug.Log("finished");
                isFinished = true;
                SceneManager.LoadScene("Distributor");
            }
        }
    }
}
