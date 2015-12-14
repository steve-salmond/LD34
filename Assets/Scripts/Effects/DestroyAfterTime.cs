using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{

    public float Lifetime = 1;

	void Start()
    {
        Invoke("Cleanup", Lifetime);
	}
	
	void Cleanup()
    {
        Destroy(gameObject);
	}
}
