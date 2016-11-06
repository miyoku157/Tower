using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class Projectile : MonoBehaviour
	{
		private double damage;
		private float speed;
		private string type;

		private GameObject FX;
		private IAPon pon;

		void Start ()
		{
			StartCoroutine("Move");

			if(type == "plasma")
			{
				StartCoroutine("Particle");
			}
		}

		public void Initialize(float _speed, double _damage, string _type)
		{
			speed = _speed;
			damage = _damage;
			type = _type;
		}

		void OnCollisionEnter(Collision collision)
		{
			pon = collision.gameObject.GetComponent<IAPon>();

			if(pon != null)
				pon.Damage(damage);
			if(type == "minigun")
				Destroy(gameObject);

			if(type == "plasma")
			{
				FX = Instantiate(Resources.Load("Prefab/PlasmaFX")) as GameObject;
				FX.transform.position = transform.position;
				FX.transform.parent = GameObject.FindGameObjectWithTag("GameController").transform;
				Destroy(FX, 0.25f);
				Destroy(gameObject);
			}
		}

		IEnumerator Move()
		{
			while(true)
			{
				transform.position += transform.forward * speed;
				yield return new WaitForSeconds(0.02f);
			}
		}

		IEnumerator Particle()
		{
			GameObject ParticleSys;

			while(true)
			{
				ParticleSys = Instantiate(Resources.Load("Prefab/PlasmaParticule")) as GameObject;
				ParticleSys.transform.parent = GameObject.FindGameObjectWithTag("GameController").transform;
				ParticleSys.transform.position = transform.position;
				Destroy(ParticleSys, 3.0f);
				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}