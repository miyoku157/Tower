using UnityEngine;
using System.Collections;
namespace AssemblyCSharp
{
	public class IAPon : MonoBehaviour
	{
		public static double upgradeLife = 0;

		public double PV;
		public int targeted;

		private float speed = 2f;
		private Transform target;
		private int rand;
		private double damage;

		public double GetDamage()
		{
			return damage;
		}

		void Start()
		{
			if(target == null)
			{
				if(GameObject.FindGameObjectWithTag("Tower") != null)
				{
					target = GameObject.FindGameObjectWithTag("Tower").transform;
				}
			}
		}

		void Awake()
		{
			targeted = 0;
			rand = Random.Range(0, 100);

			if(rand <= 90)
			{
				PV = 100 + upgradeLife;
				damage = 10;
			}
			else
			{
				PV = 300 + upgradeLife * 3;
				transform.localScale = new Vector3(transform.localScale.x * 2,
					                                   transform.localScale.y * 2,
					                                   transform.localScale.z * 2);
				speed = speed / 2;
				damage = 20;
			}
		}

		void Update()
		{
			if(target != null)
			{
				transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
			}
		}

		public void Damage(double damage)
		{
			PV -= damage;

			if(PV <= 0)
				Destroy(gameObject);
		}

		public static void Initialize()
		{
			upgradeLife = 0;
		}
	}
}
