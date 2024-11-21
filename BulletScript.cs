using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour {

	[Tooltip("Furthest distance bullet will look for target")]
	public float maxDistance = 1000000;
	RaycastHit hit;
	[Tooltip("Prefab of wall damange hit. The object needs 'LevelPart' tag to create decal on it.")]
	public GameObject decalHitWall;
	[Tooltip("Decal will need to be sligtly infront of the wall so it doesnt cause rendeing problems so for best feel put from 0.01-0.1.")]
	public float floatInfrontOfWall;
	[Tooltip("Blood prefab particle this bullet will create upoon hitting enemy")]
	public GameObject bloodEffect;
	[Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
	public LayerMask ignoreLayer;

	/*
	* Uppon bullet creation with this script attatched,
	* bullet creates a raycast which searches for corresponding tags.
	* If raycast finds somethig it will create a decal of corresponding tag.
	*/
	void Update()
	{

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, ~ignoreLayer))
        {
            if (hit.transform.CompareTag("LevelPart"))
            {
                // Instantiate wall decal
                if (decalHitWall)
                    Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
            }
            else if (hit.transform.CompareTag("Dummie"))
            {
                // Instantiate blood effect
                if (bloodEffect)
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));

                // Damage the specific pony hit
                PonyAI ponyAI = hit.transform.GetComponent<PonyAI>();
                if (ponyAI != null)
                {
                    ponyAI.TakeDamage(10); // Damage this specific pony
                }
            }
            Destroy(gameObject); // Destroy the bullet
        }
    }

}
