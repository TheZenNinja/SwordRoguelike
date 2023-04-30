- ## Stats
	- damage
	- element
	- status chance
	- attack speed
	- attack range (how far projectile reaches)
	- peircing (how many enemies it can go through)


- ## Parts
	- Core - determines the weapon type
	- Blade
		- Determines:
			1. Damage
			2. Element? (or should aux do that?)
			3. peircing
	- Handle
		- Dermines:
			1. attack speed
			2. status chance
	- Aux
		- gives the weapon a skill/augments function
			- skill examples: 
				- teleport
			- Augment examples:
				1. -50% damage, 2x projectiles shot

## Part Stat Distr Table
x|Type|Damage|Element|Status Chance|Attack Speed|Attack Range|Piercing
-|-|-|-|-|-|-|-
Core|X||||||P
Blade||P|P|P|S
Handle||S|||P|P
Aux||S|S|S|S|S|S
- P = Primary Stat
- S = Secondary Stat


- ## Weapon Types
	- sword
		- baseline weapon
		- Projectile: decent size
		- Skill: ???
	- dagger
		- faster, weaker, shorter range weapon
		- Projectile: multishot?
		- Skill: ???
	- spear
		- slower, longer range
		- projectile: thinner more precise projectile
		- Skill: ???
	- claymore
		- slow, high damage
		- Projectile: Wider slash
		- Shatter blade (Charge up): the blade breaks apart, does wide AoE damage, and shoots linear projectiles in a spread (like HSR Dan Heng burst)
	- hammer
		- slow, high damage
		- Projectile: AoE slam instead of a projectile
			- piercing does an extra instance of lower damage (1/10-1/4)
	- Chainblade
		- Projectile: slightly worse than normal sword
		- Skill: weapon change
			- Lower damage, slower projectile, way larger projectile