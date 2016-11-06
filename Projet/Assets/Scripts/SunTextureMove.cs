using UnityEngine;
using System.Collections;
namespace AssemblyCSharp{
public class SunTextureMove : MonoBehaviour
{
	public float scrollSpeedX=0.015f;
	public float scrollSpeedY=0.015f;
	public float scrollXSpeedMaterial2=0.015f;
	public float scrollYSpeedMaterial2=0.015f;

	void Start ()
	{

	}
	
	void Update ()
	{
		float offsetX = Time.time * scrollSpeedX % 1;
		float offsetY = Time.time * scrollSpeedY % 1;
		float offset2X = Time.time * scrollXSpeedMaterial2 % 1;
		float offset2Y = Time.time * scrollYSpeedMaterial2 % 1;

		GetComponent<Renderer>().materials[0].SetTextureOffset("_BumpMap",new Vector2(offsetX,offsetY));
		GetComponent<Renderer>().materials[0].SetTextureOffset("_MainTex",new Vector2(offsetX,offsetY));

		if (GetComponent<Renderer>().materials.Length > 1)
		{
			GetComponent<Renderer>().materials[1].SetTextureOffset("_MainTex",new Vector2(offset2X,offset2Y));
			GetComponent<Renderer>().materials[1].SetTextureOffset("_BumpMap",new Vector2(offset2X,offset2Y));
		}
	}
}

}