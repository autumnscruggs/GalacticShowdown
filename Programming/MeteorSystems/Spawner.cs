using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    public bool enabled = true;

    protected List<GameObject> gameObjects;
    protected List<GameObject> objectsToRemove;

    public Spawner()
    {
        gameObjects = new List<GameObject>();
        objectsToRemove = new List<GameObject>();
    }

    public void Update()
    {
        RemovesObjectInList();
        CheckForInactiveOnes();
    }

    protected void RemovesObjectInList()
    {
        //remove objects in Object to remove list
        foreach (GameObject go in objectsToRemove)
        {
            gameObjects.Remove(go);
            DestroyObject(go);
        }
    }

    public virtual void Spawn()
    {
        if (enabled)
        {
            GameObject spawn = (GameObject)Instantiate(spawnObject, this.transform.position, Quaternion.identity);
            SetupSpawnObject(spawn); //virtual hook for setting up game object
            AddGameObject(spawn);
        }
    }

    protected virtual void AddGameObject(GameObject spawn)
    {
        gameObjects.Add(spawn);
    }

    protected virtual void SetupSpawnObject(GameObject go)
    {
        //Nothing todo should override with specific type
    }

    private void CheckForInactiveOnes()
    {
        foreach (GameObject go in gameObjects)
        {
            if (!go.activeInHierarchy)
            {
                objectsToRemove.Add(go);
            }
        }
    }
}
