- LMB to shoot weapon
	- slow down over time
- RMB to retrieve


- Stats
	- dmg
	- status chnc
	- speed
	- bool: piercing
	- int: ammo
	- float: returnDmgMulti
	- bool: bouncing
		- if piercing and bouncing are active at the same time: pierce enemy, bounce walls

- Weapons
	- sword - basic
	- spear - pierces, faster projectile
	- dagger - more ammo
	- claymore - slower, pierces, large projectile, increased return dmg multi
	- hammer - like claymore but bounces, has an even higher return dmg multi


- Visuals
	- trail/line following weapon
	- spark on hit/bounce
	- have symbolic ui in bottom of screen
		- show a mini sword icon for each ammo
		- dont render weapon on player if out of ammo
