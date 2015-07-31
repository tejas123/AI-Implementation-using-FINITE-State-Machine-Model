using UnityEngine;
using System.Collections;

public class BoxFSM : FSM
{
		public enum FSMState
		{
				None,
				Patrol,
				Chase,
		}
		//Current state that the Box is in
		public FSMState curState;
		//Speed of the Box
		private float curSpeed;
		//Box Rotation Speed
		private float curRotSpeed;

		//Initialize the Finite state machine for the Box
		protected override void Initialize ()
		{
				curState = FSMState.Patrol;
				curSpeed = 5.0f;
				curRotSpeed = 1.5f;				
				
				//Get the list of points
				pointList = GameObject.FindGameObjectsWithTag ("WandarPoint");
				
				//Set Random destination point first
				FindNextPoint ();
				
				//Get the target enemy(Player)
				GameObject objPlayer = GameObject.FindGameObjectWithTag ("Player");
				playerTransform = objPlayer.transform;
				
				if (!playerTransform)
						print ("Player doesn't exist.. Please add one " + "with Tag named 'Player'");
				
		}

		//Update each frame
		protected override void FSMUpdate ()
		{
				switch (curState) {
				case FSMState.Patrol:
						UpdatePatrolState ();
						break;
				case FSMState.Chase:
						UpdateChaseState ();
						break;
				}
		}

		protected void UpdatePatrolState ()
		{
				//Find another random patrol point if the current
				//point is reached
				if (Vector3.Distance (transform.position, destPos) <= 2.5f) {
						print ("Reached to the destination point\n" + "calculating the next point");
						FindNextPoint ();
				}

				//Check the distance with player Box
				//When the distance is near, transition to chase state
				else if (Vector3.Distance (transform.position, playerTransform.position) <= 15.0f) {
						print ("Switch to Chase Position");
						curState = FSMState.Chase;
				}
				
				//Rotate to the target point
				Quaternion targetRotation = Quaternion.LookRotation (destPos - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
				
				//Go Forward
				transform.Translate (Vector3.forward * Time.deltaTime * curSpeed);
		}

		protected void FindNextPoint ()
		{
				print ("Finding next point");
				int rndIndex = Random.Range (0, pointList.Length);
				float rndRadius = 5.0f;
				Vector3 rndPosition = Vector3.zero;
				destPos = pointList [rndIndex].transform.position + rndPosition;
				
				//Check Range to decide the random point
				//as the same as before
				if (IsInCurrentRange (destPos)) {
						rndPosition = new Vector3 (Random.Range (-rndRadius, rndRadius), 0.0f, Random.Range (-rndRadius, rndRadius));
						destPos = pointList [rndIndex].transform.position + rndPosition;
				}
		}

		protected bool IsInCurrentRange (Vector3 pos)
		{
				float xPos = Mathf.Abs (pos.x - transform.position.x);
				float zPos = Mathf.Abs (pos.z - transform.position.z);
				if (xPos <= 8 && zPos <= 8)
						return true;
				return false;
		}

		protected void UpdateChaseState ()
		{
				//Set the target position as the player position
				destPos = playerTransform.position;
				//Check the distance with player Box When
				float dist = Vector3.Distance (transform.position, playerTransform.position);
				
				//Go back to patrol as player is now too far
				if (dist >= 15.0f) {
						curState = FSMState.Patrol;
						FindNextPoint ();
				}
				//Rotate to the target point
				Quaternion targetRotation = Quaternion.LookRotation (destPos - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
				//Go Forward
				transform.Translate (Vector3.forward * Time.deltaTime * curSpeed);
		}
}
