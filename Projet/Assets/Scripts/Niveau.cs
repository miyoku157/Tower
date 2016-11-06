using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Niveau : MonoBehaviour
	{
		public static List<GameObject> pons;
		public GameObject pon;
		public static bool generate = false;
		public float timer;
		public int number;
		public static bool start=true;
		private bool running;
		private bool generating;
		private Population pop;
		private int nbtour = 6;
		private int touractu = 0;
		void Start()
		{
			pop = new Population(nbtour);
			for (int i=0; i<nbtour; i++) {

				pop.indiv[i].transform.GetChild(0).GetComponent<Tower>().Aleatower();			
			}
			start = false;
			foreach(GameObject tow in pop.indiv){
				tow.transform.GetChild(0).GetComponent<Tower>().pv=100;
				tow.transform.GetChild(0).GetComponent<Tower>().fitness=0;
			}
			this.gameObject.AddComponent<Timer>();
			StartCoroutine("SpawnPons");
			StartCoroutine("UpgradeLife");
			Camera.main.GetComponent<CameraControl>().target = pop.indiv[0].transform.GetChild(0);
			pop.genereTower (0);
			touractu++;

		}
		void Update(){
			if (!generate) {
				if(touractu!=nbtour){
					pop.genereTower(touractu);
				}
				touractu++;
			}
			if (touractu > nbtour) {
				touractu=0;

				pop.selection();
				Camera.main.GetComponent<CameraControl>().target = pop.indiv[0].transform;



			}
		}
		void Awake()
		{
			pons = new List<GameObject>();
			generating = true;
			running = true;
		}



		IEnumerator SpawnPons()
		{
			int direction;

			while(generating)
			{
				for(int i = 0; i < number; i++)
				{
					pon = Instantiate(Resources.Load("Prefab/Pon")) as GameObject;
					pon.transform.parent = transform;

					direction = Random.Range(0, 4);

					switch(direction)
					{
						case 0 :
							pon.transform.position = new Vector3(20f, 0.35f, Random.Range(-20f, 20f));
						break;

						case 1 :
							pon.transform.position = new Vector3(-20f, 0.35f, Random.Range(-20f, 20f));
						break;

						case 2 :
							pon.transform.position = new Vector3(Random.Range(-20f, 20f), 0.35f, 20f);
						break;

						case 3 :
							pon.transform.position = new Vector3(Random.Range(-20f, 20f), 0.35f, -20f);
						break;
					}

					pons.Add(pon);
				}
				yield return new WaitForSeconds(timer);
			}
		}

		IEnumerator UpgradeLife()
		{
			while(true)
			{
				//IAPon.upgradeLife += 10;
				yield return new WaitForSeconds(1.0f);
			}
		}
	}
}
