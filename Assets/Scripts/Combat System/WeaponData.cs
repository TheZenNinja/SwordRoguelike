using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CombatSystem
{
	[CreateAssetMenu(menuName = "Create Weapon Data")]
	public class WeaponData : ScriptableObject
	{
		public int dmg;
		[Range(0,1)]
		public float statusChance;

		public float speed;
		[Range(1,5)]
		public float returnDmgMulti;
		public int ammo;

		public bool piercing;
		public bool bouncing;

		public WeaponProjectile prefab;
	}
}
