using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    private Scene simulationScene;
    private PhysicsScene physicsScene;
    private Transform scenaryParent;

    [SerializeField] private LineRenderer line;
    [SerializeField] private int maxPhysicsFrameIterations = 100;

    private void Start()
    {
        CreatePhysicsScene();
    }

    private void CreatePhysicsScene()
    {
        simulationScene  = SceneManager.CreateScene("SimulationScene", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsScene = simulationScene.GetPhysicsScene();

        scenaryParent = GameObject.FindGameObjectWithTag("Scenery").transform;

        foreach(Transform obj in scenaryParent)
        {
            var ghostObject = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObject.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObject, simulationScene);

        }
    }

    public void SimulateVelocity(GameObject bulletPrefab, Vector3 spawnPoint, Vector3 velocity)
    {
        var ghostObject = Instantiate(bulletPrefab, spawnPoint, Quaternion.identity);
        ghostObject.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObject.gameObject, simulationScene);

        //ghostObject.GetComponent<Bullet>().AddForceWhenSpawned(velocity);

        line.positionCount = maxPhysicsFrameIterations;

        for (int i = 0; i < maxPhysicsFrameIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            line.SetPosition(i, ghostObject.transform.position);

        }
        //Destroy(ghostObject);
    }


}
