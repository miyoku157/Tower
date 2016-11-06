using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace AssemblyCSharp
{
public class Tower : MonoBehaviour
{
		//face: 1=West 2=est 3= nord 4=sud
		public static Vector3[][,] position_fixe;
		public bool[][,] position;
		public List<Arme> weapons= new List<Arme>();
		public double pv;
		public const int score = 100;
		public double fitness;
		public enum taille{ petit=1,grand=2}
		public enum type{ minigun=1,laser=2,missile=3}
	// Use this for initialization
	void Awake ()
	{
			if (Niveau.start) {

				position_fixe = new Vector3[4][,];
								for (int i=0; i<4; i++) {
										position_fixe [i] = new Vector3[3, 4];			
								}
								position = new bool[4][,];
								for (int i=0; i<4; i++) {
										position [i] = new bool[3, 4];			
								}

				double x = -0.4 * this.transform.localScale.x;
								double y = 3.8 * this.transform.localScale.y;
								double z = 0.287 * this.transform.localScale.z;
								for (int i=0; i<2; i++) {
										y = 3.8 * this.transform.localScale.y;
										for (int j=0; j<4; j++) {
												x = -0.4 * this.transform.localScale.x;
												for (int k=0; k<3; k++) {
														position_fixe [i] [k, j] = new Vector3 ((float)x, (float)y, (float)z);
														x += 0.6 * this.transform.localScale.x;
												}
												y -= 0.7 * this.transform.localScale.y;
										}
										z -= 1.746 * this.transform.localScale.z;
								}
								x = -0.653;
								y = 0;
								z = 0;

								for (int i=2; i<4; i++) {
										y = 3.8 * this.transform.localScale.y;
										for (int j=0; j<4; j++) {
												z = 0 * this.transform.localScale.z;
												for (int k=0; k<3; k++) {
														position_fixe [i] [k, j] = new Vector3 ((float)x, (float)y, (float)z);
														z -= 0.6 * this.transform.localScale.z;
												}
												y -= 0.7 * this.transform.localScale.y;
										}
										x += 1.753 * this.transform.localScale.x;
								}


						}
	}
	
	
		public void Aleatower(){
			int compteur = score;
			int ite = 0;
			do {
				Random .seed = Random.Range (0, 10000);
				
				int sous = 0;
				Arme weap = new Arme ();
				int face = UnityEngine.Random.Range (0, 4);
				int x_co = UnityEngine.Random.Range (0, 3);
				int y_co = UnityEngine.Random.Range (0, 4);
				while (position[face][x_co,y_co]) {
					face = UnityEngine.Random.Range (0, 4);
					x_co = UnityEngine.Random.Range (0, 3);
					y_co = UnityEngine.Random.Range (0, 4);
				}
				if (UnityEngine.Random.Range (1, 3) == 1) {
					weap.taille = 1;
					sous += 5;
				} else {
					weap.taille = 2;
					sous += 10;
				}
				position [face] [x_co, y_co] = true;
				int rand = UnityEngine.Random.Range (1, 4);
				//ajout du score des types
				if (rand == 1) {
					weap.Type = 1;
				} else if (rand == 2) {
					weap.Type = 2;
				} else {
					weap.Type = 3;
				}
				switch (face) {
				case 0:
					weap.rotation = Quaternion.Euler (0, 0, 0);
					break;
				case 1:
					weap.rotation = Quaternion.Euler (0, 180, 0);
					break;
				case 2:
					weap.rotation = Quaternion.Euler (0, -90, 0);
					break;
				case 3:
					weap.rotation = Quaternion.Euler (0, 90, 0);
					break;
				}
				
				weap.pos = position_fixe [face] [x_co, y_co];
				weapons.Add (weap);
				compteur -= sous;
				if (compteur < 0) {
					weapons.Remove (weap);
					compteur += sous;
					ite++;
				}
			} while(compteur>0&&ite<=5);		
		}
		public static Tower ConstructTower1(Tower tow){
			Tower retour=(Tower)tow.MemberwiseClone ();
			retour.fitness = 0;
			mutation (retour);
			return retour;
		}
		public static Tower ConstructTower2(Tower parent1,Tower parent2){
			int total = 50;
			int i = 0;
			Tower retour=(Tower)parent1.MemberwiseClone ();
			List<Arme> temp = new List<Arme> (parent1.weapons);
			parent1.weapons.Clear ();
			int random = 0;
			while (total>0&&temp.Count>0) {
				int decrement=0;
				random=UnityEngine.Random.Range(0,temp.Count);
					if(temp[random].taille==(int)taille.grand){
					decrement+=10;
				}else{
					decrement+=5;
				}
				if(temp[random].Type==(int)type.laser){
				}else if (temp[random].Type==(int)type.minigun){
				}else{
				}
				total-=decrement;

				if(total>=0){
					retour.weapons.Add(temp[random]);
				}
				temp.RemoveAt(random);

				i++;
			}

			total = 50;				

			temp = new List<Arme> (parent2.weapons);
			while (total>0&&temp.Count>0) {
				int decrement=0;

				random=UnityEngine.Random.Range(0,temp.Count);
				if(temp[random].taille==(int)taille.grand){
					decrement+=10;
				}else{
					decrement+=5;
				}
				if(temp[random].Type==(int)type.laser){
				}else if (temp[random].Type==(int)type.minigun){
				}else{
				}

			
				if(retour.weapons.Exists(ar=>ar.pos.x==temp[random].pos.x&&ar.pos.y==temp[random].pos.y&&ar.pos.z==temp[random].pos.z)){
						decrement=0;
				}
				total-=decrement;
				if(total>=0&&decrement!=0){
					retour.weapons.Add(temp[random]);
				}
				temp.RemoveAt(random);

			}
			
		
			retour.fitness = 0;
			mutation (retour);
			return retour;
		}
		public static void mutation(Tower tow){
			foreach (Arme ar in tow.weapons) {
				if (UnityEngine.Random.Range (0f, 1f) < Population.mutation) {
					int face=0;
					if(ar.rotation.Equals(Quaternion.Euler(0,0,0))){
						face=0;
					}else if(ar.rotation.Equals(Quaternion.Euler(0,180,0))){
						face=1;
					}else if(ar.rotation.Equals(Quaternion.Euler(0,-90,0))){
						face=2;
					}else if(ar.rotation.Equals(Quaternion.Euler(0,90,0))){
						face=3;
					}

					int x=0;
					int y=0;
					for(int i=0;i<3;i++){
						for (int j=0;j<4;j++){
							if(ar.pos.x==position_fixe[face][i,j].x&&ar.pos.y==position_fixe[face][i,j].y&&ar.pos.z==position_fixe[face][i,j].z){
								x=i;
								y=j;
							}
						}
					}
					int count=0;
					do{
					int rand=UnityEngine.Random.Range(0,4);
					if(rand==0){
						x-=1;
						if(x<0){
							x=2;
							face+=2;
							if(face>3){
								face=face%3;
							}
							
						}
					}else if(rand==1){
						x+=1;
						if(x>2){
							x=0;
							face -=2;
							if(face<0){
								face+=4;
							}
						}
					}else if(rand==2){
							y+=1;
						if(y>3){
							y=0;
						}

					}else if (rand ==3){
							y-=1;
							if(y<0){
							y=3;
						}
					}
						count++;
					}while(tow.position[face][x,y]&& count<10);
				}
				if(UnityEngine.Random.Range(0f,1f)<Population.mutation){
					int rand;
					if((int)ar.Type==1){
						rand=UnityEngine.Random.Range(2,4);

					}else if((int)ar.Type==3){
						rand=UnityEngine.Random.Range (1,3);
					}else{
						double test=UnityEngine.Random.Range(0f,1f);
						if(test<0.5f){
							rand=1;
						}else{
							rand=3;
						}
					}
					ar.Type=rand;
				}
				if(UnityEngine.Random.Range(0f,1f)<Population.mutation){
					if(ar.taille==(int)taille.petit){
						ar.taille=(int)taille.grand;
					}else{
						ar.taille=(int)taille.petit;
					}
				}
						}
		}

		public void generation()
		{
			GameObject tow = this.gameObject;
			GameObject tourelle;

			foreach (Arme ar in weapons)
			{
				if(ar.taille == 1)
				{
					tourelle = (GameObject)Instantiate(Resources.Load("Prefab/BaseTurret"),ar.pos,ar.rotation);
				}
				else
				{
					tourelle = (GameObject)Instantiate(Resources.Load("Prefab/HeavyTurret"),ar.pos,ar.rotation);
				}
				tourelle.GetComponent<WeaponControl>().type=ar.Type;
				tourelle.GetComponent<WeaponControl>().taille=ar.taille;
				tourelle.GetComponent<WeaponControl>().Initialize();
				tourelle.transform.parent=tow.transform.parent;

			}
		}

		public void Damage(double damage)
		{
			pv -= damage;
			
			if(pv <= 0)
			{
				foreach(GameObject shot in GameObject.FindGameObjectsWithTag("Shot"))
				{
					if(shot != null)
						Destroy(shot);
				}

				for(int i = 1; i < transform.parent.childCount; i++)
				{
					Destroy(transform.parent.GetChild(i).gameObject);
				}

				this.fitness = GameObject.Find("GameController").GetComponent<Timer>().getTime ();
				this.gameObject.tag = "Untagged";
				this.gameObject.GetComponent<MeshRenderer> ().enabled = false;
				this.gameObject.GetComponent<MeshCollider> ().enabled = false;
				this.enabled = false;
				StopCoroutine ("SpawnPons");
				Niveau.generate=false;

			}
		}

		void OnCollisionEnter(Collision collision)
		{
			IAPon pon = collision.gameObject.GetComponent<IAPon>();

			if(pon != null)
			{
				Damage(pon.GetDamage());
				Destroy(pon.gameObject);
			}
		}
}

}