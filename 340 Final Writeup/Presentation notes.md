- # TO FIX
	- [x] enemy not stopping 
		- [ ] maybe?
	- [x] fix weapon trails
	- [ ] controls/help page
	- [ ] add more particles
		- [x] player dash
		- [ ] kill
		- [ ] enemy spawn
	- [ ] wave popup
		- [ ] kill count
		- [ ] increasing HP
	- [ ] add sounds and music
		- [ ] woosh
		- [ ] kill
		- [ ] take damage
		- [ ] music?
		- [ ] settings
	- [x] add text for hp values




- explain extension functions
- explain operator overriding
- explain `var`
- Scripts to show
	- `WeaponController`
		- Spawning & cirlcing
	- `ContextSteering`
	- `EnemyAI`
	- `WeaponProjectile`
	- `GameManager`
		- enemy spawning
	- how AI works
		- explain dot product
	- show old dungeon gen script?
	- [GameAIPro2_Chapter18_Context_Steering_Behavior-Driven_Steering_at_the_Macro_Scale.pdf](http://www.gameaipro.com/GameAIPro2/GameAIPro2_Chapter18_Context_Steering_Behavior-Driven_Steering_at_the_Macro_Scale.pdf)




- ## Todo
	- more interesting combat mechanics
		- weapon switching
		- more elements
		- unique weapon behaviors
		- parrying
		- more interesting enemy designs and attacks
	- Optimazation
	- other platforms
	- better sprites
- # Future Ideas
	- turn it into a 3d FPS roguelike
	- Elemental dmg system
	- multiple equipable weapons and abilities
	- crafting
	- multiplayer?

# 305 Notes
- Issues
	- Slightly laggy server
		- esp when generating dims
		- need more cpu?
	- showing the console when running the svc
	- Handling crashing?
	- Finishing the google doc with info
	- Finish/Optamize signup form 
- Solutions
	- Restarting the server
		- using task scheduler
			- updated to allow the cmds to run even when signed out
		- using MCRcon to send cmds to the server
	- Removing the port from the DNS addr
- Compare to Azure and GPortal