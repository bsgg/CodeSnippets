using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObjectPoolingManager : MonoBehaviour {

    #region FIELDS
    public List<GameObject> objectToRecycle = new List<GameObject>(); // Used to keep traking od the objects that we can use in the game
    public Transform originlPoolPosition; // used to reset the position of the object that we've pulled
    private GameObject selectedObject;
	#endregion
	
	#region MONOBHEAVIOR
	void Start() {
        InvokeRepeating("Shot", 1.0f, 2f); //Shot an object every 2 seconds
    }

    private void Shot()
    {
        this.SetObjectFromPools();
        this.selectedObject.transform.position = Vector3.zero; // Move the object to the center
        StartCoroutine(ResetTheObject());
    }

    //Get an object from the pool and remove it from the pool once is selected
    private void SetObjectFromPools()
    {
        this.selectedObject = this.objectToRecycle[Random.Range(0, this.objectToRecycle.Count)];
        this.objectToRecycle.Remove(this.selectedObject);
    }

    //Readd the object to the pool and reset his position
    private IEnumerator ResetTheObject()
    {
        yield return new WaitForSeconds(1);
        this.selectedObject.transform.position = originlPoolPosition.position;
        this.objectToRecycle.Add(this.selectedObject);
    }
	#endregion
}