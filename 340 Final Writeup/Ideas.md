[[Requirements]]

- ## Player
	- Standard elemental damage idea (import)
	- have different classes
	- 1-3? selectectable weapons
		- will have to change the part storage pool if thats the case
	- craft melee weapons from parts found during a run
		- have an inventory where you can hold (20?) parts
		- each part is curated? (`scriptableObj`)
		- drag and drop modifcation (when the floor is cleared)
	- different melee weapons shoot different projectiles
		- have a puncture stat that determines how many enemies it can go thru


- ## AI
	- #pathfinding 
		- Use waypoints for patrol paths
		- steering AI vs A*?
			- https://www.youtube.com/watch?v=tIfC00BE6z8
	- #difficulty 
		- Difficulty settings change not only damage/health, but attack speed/patterns
		- Have a `ScriptableObject` hold difficulty settings? #dataDesign 
	- #AI 
		- have elite enemies be able to predict/dodge attacks (diffuclty determines how accurate/punishable the dodge is)

