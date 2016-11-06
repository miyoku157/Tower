using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class Explosion : MonoBehaviour
	{
		private IAPon pon;
		public double damage = 70;
		public float expandingRate = 1.2f;

		void Update ()
		{
			transform.localScale = new Vector3(transform.localScale.x * expandingRate,
			                                   transform.localScale.y * expandingRate,
			                                   transform.localScale.z * expandingRate);
		}

		void OnCollisionEnter(Collision collision)
		{
			pon = collision.gameObject.GetComponent<IAPon>();
			
			if(pon != null)
				pon.Damage(damage);
		}
	}
}