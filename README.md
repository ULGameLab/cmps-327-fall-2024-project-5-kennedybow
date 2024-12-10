[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/_44qlOG2)
# CMPS327-AStar-Game-Starter-Code
CMPS 327 A Star Game Starter Code
Project Title: A* Pathfinding Game

Description:
This Unity game implements an enemy AI system where enemies use the A* pathfinding algorithm to chase the player. The game includes dynamic behavior for the enemies and allows for tile-based movement. The player controls a character in a maze environment, while enemies navigate through the maze and try to track the player based on their assigned behaviors.

What Works:
Basic Game Mechanics: The player can move freely through the maze, and the enemies can attempt to move toward their target.
Pathfinding Structure: The structure for pathfinding is in place using the A* algorithm, and the enemies attempt to find paths to specific goals.
Enemy Behavior Handling: Basic handling of enemy states, such as moving, resting, and attempting to chase the player.
What Does Not Work:
Pathfinder Namespace Issue: The PathFinder class cannot be used as intended due to a CS0118 error, where PathFinder is being incorrectly identified as a namespace instead of a type. This issue is preventing the enemies from finding paths and functioning properly.
Enemy Movement: Because the pathfinding system isn't working due to the above error, enemies are unable to navigate the maze as intended, making it impossible for them to chase the player.
Pathfinding Transitions: Since the error exists, the enemies cannot transition properly between behaviors or dynamically adjust their paths when the player moves.
Known Bugs:
Pathfinding Initialization: Enemies do not perform pathfinding correctly due to the namespace error. They are unable to find or follow paths.
Behavior State Issues: The enemies' states (chasing, resting, etc.) are dependent on pathfinding, and with the error present, these states do not transition properly.
Collision Detection: Some enemies may clip or get stuck on walls due to a lack of movement updates, though this is secondary to the pathfinding issue.
Problems Encountered:
Namespace Conflicts: I ran into the CS0118 error where the PathFinder class is being treated as a namespace rather than a type, which is preventing the pathfinding system from functioning. I tried fixing this by ensuring proper imports, but the issue persists and requires further debugging.
Enemy AI Behavior: Without functioning pathfinding, the enemies are stuck in their default states, and the logic for moving between tiles is broken. The enemies fail to chase or move properly, which impacts gameplay.
