using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
namespace AssemblyCSharp{

	public class WeaponControl : MonoBehaviour
	{
		public float range;
		public int type;
		public int taille;

		private float trackingSpeed = 5.0f;
		private bool isOn;
		private float dot;
		private float norm;
		private bool reloading;
		private AudioSource AS;
		private GameObject gun;
		private Transform gunTr;
		private GameObject target = null;
		private Quaternion targetRotation;
		private float str;
		private List<GameObject> targets = new List<GameObject>();
		private MeshFilter mesh;
		
		void Start()
		{
			isOn = true;
			StartCoroutine("CheckRange");
		}

		public void Initialize()
		{
			mesh = GetComponent<MeshFilter>();
			
			switch(type)
			{
			case 2 :
				trackingSpeed = 4f;
				range = 13;
				gun = Instantiate(Resources.Load("Prefab/TurretLaser")) as GameObject;
				gunTr = gun.transform;
				gunTr.position = transform.position;
				gunTr.localScale = transform.localScale;
				gunTr.rotation = transform.rotation;
				gunTr.parent = transform;
				transform.GetChild(0).GetChild(0).GetComponent<LineRenderer>().enabled = false;
			break;
				
			case 1 :
				trackingSpeed = 7f;
				range = 20;
				gun = Instantiate(Resources.Load("Prefab/TurretMinigun")) as GameObject;
				gunTr = gun.transform;
				gunTr.position = transform.position;
				gunTr.localScale = transform.localScale;
				gunTr.rotation = transform.rotation;
				gunTr.parent = transform;
			break;
				
			case 3 :
				range = 17;
				trackingSpeed = 3f;
				gun = Instantiate(Resources.Load("Prefab/TurretPlasma")) as GameObject;
				gunTr = gun.transform;
				gunTr.position = transform.position;
				gunTr.localScale = transform.localScale;
				gunTr.rotation = transform.rotation;
				gunTr.parent = transform;
			break;
			}

			AS = transform.GetChild(0).GetComponent<AudioSource>();
			AS.Stop();
		}

		void Update()
		{
			float distance = 1500f;

			if(targets.Count() > 0 && target == null)
			{
				foreach(GameObject pon in targets)
				{
					if(pon && Vector3.Distance(transform.position, pon.transform.position) < distance && pon.GetComponent<IAPon>().targeted < 2)
					{
						target = pon;
						distance = Vector3.Distance(transform.position, pon.transform.position);
					}
				}

				if(target != null)
					target.GetComponent<IAPon>().targeted++;
			}

			if(!reloading && target != null)
			{
				dot = Vector3.Dot((target.transform.position - transform.position).normalized, transform.GetChild(0).forward);
				//Debug.Log(dot);
				if(Vector3.Distance(transform.position, target.transform.position) < range &&
				   dot > 0.98)
				{
					StartCoroutine("Fire", target);
				}
				else if(type == 2)
				{
					transform.GetChild(0).GetChild(0).GetComponent<LineRenderer>().enabled = false;
					transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Stop();
				}
			}

			if(target != null)
			{
				targetRotation = Quaternion.LookRotation(target.transform.position - gunTr.position);
				str = Mathf.Min(trackingSpeed * Time.deltaTime, 1);
				gunTr.rotation = Quaternion.Lerp(gunTr.rotation, targetRotation, str);
			}
			else if(type == 2)
			{
				transform.GetChild(0).GetChild(0).GetComponent<LineRenderer>().enabled = false;
			}
		}

		IEnumerator CheckRange()
		{


			while(isOn)
			{
				for(int i = 0; i < Niveau.pons.Count(); i++)
				{
					if(Niveau.pons[i] != null)
					{
						dot = Vector3.Dot((Niveau.pons[i].transform.position - transform.position).normalized, transform.forward);

						if(Vector3.Distance(transform.position, Niveau.pons[i].transform.position) < range &&
						   dot > 0.3)
						{
							//Debug.DrawLine(gunTr.GetChild(0).position, Niveau.pons[i].transform.position);
							targets.Add(Niveau.pons[i]);
						}
					}
				}
				yield return new WaitForSeconds(0.1f);
			}
		}

		IEnumerator Fire(GameObject target)
		{
			GameObject projectile;

			switch(type)
			{
				case 2 :
					transform.GetChild(0).GetChild(0).GetComponent<LineRenderer>().enabled = false;
				break;
			}

			switch(type)
			{
				case 2 :
					transform.GetChild(0).GetChild(0).GetComponent<LineRenderer>().enabled = true;
				break;

				case 1 :
					AS.Play();
					reloading = true;
					
					projectile = Instantiate(Resources.Load("Prefab/LaserShot")) as GameObject;
					if(target != null)
					{
						projectile.transform.position = gunTr.transform.GetChild(0).position;
						projectile.transform.rotation = Quaternion.LookRotation(target.transform.position - gunTr.position);
						projectile.transform.parent = GameObject.FindGameObjectWithTag("GameController").transform;
						HandleShooting(0, projectile.GetComponent<Projectile>());
					}
					
					yield return new WaitForSeconds(0.2f);
					reloading = false;
				break;

				case 3 :
					AS.Play();
					reloading = true;
					
					projectile = Instantiate(Resources.Load("Prefab/PlasmaShot")) as GameObject;
					if(target != null)
					{
						projectile.transform.position = gunTr.transform.GetChild(0).position;
						projectile.transform.rotation = Quaternion.LookRotation(target.transform.position - gunTr.position);
						projectile.transform.parent = GameObject.FindGameObjectWithTag("GameController").transform;
						HandleShooting(1, projectile.GetComponent<Projectile>());
					}
					
					yield return new WaitForSeconds(2.5f);
					reloading = false;
				break;
			}
			yield return new WaitForSeconds(0.1f);
		}

		private void HandleShooting(int _type, Projectile shot)
		{
			switch(_type)
			{
				case 0 :
					shot.Initialize(1.1f, 10, "minigun");
				break;

				case 1 :
					shot.Initialize(0.25f, 1, "plasma");
				break;
			}
		}
	}
}

