using UnityEngine;
using System.Collections;

public class BoxMovementScript : MonoBehaviour
{

		public float speed = 0.1f;
		private Vector3 positionVector3;

		void Update ()
		{
				InitializePosition ();
				if (Input.GetKey (KeyCode.LeftArrow)) {
						GoLeft ();
				}
		
				if (Input.GetKey (KeyCode.RightArrow)) {
						GoRight ();
				}
		
				if (Input.GetKey (KeyCode.UpArrow)) {
						GoTop ();
				}
		
				if (Input.GetKey (KeyCode.DownArrow)) {
						GoDown ();
				}
				RotateNow ();
		}
	
	
		private void InitializePosition ()
		{
				positionVector3 = transform.position;
		}
	
		private void RotateNow ()
		{
				Quaternion targetRotation = Quaternion.LookRotation (transform.position - positionVector3);
				transform.rotation = targetRotation;
		}
	
		private void GoLeft ()
		{
				transform.position = transform.position + new Vector3 (-speed, 0, 0);	
		}
	
		private void GoRight ()
		{
				transform.position = transform.position + new Vector3 (speed, 0, 0);
		}
	
		private void GoTop ()
		{
				transform.position = transform.position + new Vector3 (0, 0, speed);
				
		}
	
		private void GoDown ()
		{
				transform.position = transform.position + new Vector3 (0, 0, -speed);
				
		}
}
