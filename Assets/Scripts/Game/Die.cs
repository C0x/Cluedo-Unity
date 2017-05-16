using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour {

	public Vector3 beginPosition;

	[HideInInspector]
	public bool Rolling = false;

	private Rigidbody rb;

	private float duration;
	private float torqueForceMultiplier;
	private float force;
	private float sidewaysForce;

	void Start()
	{
		rb = this.gameObject.GetComponent<Rigidbody>();
		ResetDie();
	}

	void Update () 
	{

		if (!Rolling && Input.GetKeyUp("space"))
		{
			Debug.Log("Rolling");
			StartRollDie();
		}

	}

	///<summary>
	/// Start roll animation
	///</summary>
	public void StartRollDie()
	{
		//randomize some values to randomize the diceroll
		duration = 3f;
		force = Random.Range(100, 300f);
		torqueForceMultiplier = 5f;
		sidewaysForce = Random.Range(-2f, 5f);

		Rolling = true;
		rb.useGravity = true;
		StartCoroutine("RollDie");
	}

	///<summary>
	/// Actual rolling of dice (coroutine)
	///</summary>
	private IEnumerator RollDie()
	{
		float time = 0f;
		
		//dice movement for <duration> seconds
		while (time < duration)
		{
			if (this.transform.position.z > 28f)
				force = -force;

			time += Time.deltaTime;
			rb.AddTorque(new Vector3(force * torqueForceMultiplier, 0f, sidewaysForce), ForceMode.Impulse);
			rb.AddForce(0f, 0f, force * Time.deltaTime); 
			yield return null;
		}

		//check if die stopped moving during <duration> * 2 seconds
		//if die stopped moving, continue
		time = duration * 2;
		while (time > 0)
		{
			time -= Time.deltaTime;

			if (rb.velocity.magnitude < 0.1)
				break;

			yield return null;
		}

		Debug.Log(this.name + ": " + GetUpside());
	}

	///<summary>
	/// Get the value of the dice after roll animation
	///</summary>
	public int GetUpside()
	{
		float dotFwd = Vector3.Dot(this.transform.forward, Vector3.up);
		if (dotFwd > .99f) return 3; //front side
		if (dotFwd < -.99f) return 4; //back side

		float dotRight = Vector3.Dot(this.transform.right, Vector3.up);
		if (dotRight > .99f) return 5; //right side
		if (dotRight < -.99f) return 2; //left side

		float dotUp = Vector3.Dot(this.transform.up, Vector3.up);
		if (dotUp > .99f) return 1;	//top side
 		if (dotUp < -.99f) return 6; //bottom side

		return 0; //should never be reached, but IF it's reached -> handle in call
	}

	///<summary>
	/// Reset die to beginposition
	///</summary>
	private void ResetDie()
	{
		this.transform.position = beginPosition;
		this.transform.rotation = Quaternion.Euler( Random.Range(-90f, 90f),
						  							Random.Range(-90f, 90f),
													Random.Range(-90f, 90f) );		
		rb.useGravity = false;
		Rolling = false;
	}
}
