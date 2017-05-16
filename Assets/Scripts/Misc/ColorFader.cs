using UnityEngine;

///<summary>
/// Script that allows to fade between 2 colors
/// Used to highlight all tiles a use can reach after diceroll
///</summary>
public class ColorFader : MonoBehaviour 
{	
	public bool IsEnabled = true;

	public float FadeDuration = 1f;
	public Color Color1;
	public Color Color2;

	private Color startColor;
	private Color endColor;
	private float lastColorChangeTime;

	private Material material;
	

	void Start () {
		material = GetComponent<Renderer>().material;
		startColor = Color1;
		endColor = Color2;
	}
	
	void Update () {

		if (!IsEnabled) return;

		var ratio = (Time.time - lastColorChangeTime) / FadeDuration;
		ratio = Mathf.Clamp01(ratio);
		//material.color = Color.Lerp(startColor, endColor, ratio); //normal
		material.color = Color.Lerp(startColor, endColor, Mathf.Sqrt(ratio)); //effect 1
        //material.color = Color.Lerp(startColor, endColor, ratio * ratio); //effect 2

		if (ratio == 1f)
		{
			lastColorChangeTime = Time.time;

			var temp = startColor;
			startColor = endColor;
			endColor = temp;
		}
	}
}
