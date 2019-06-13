# Course Project at Design and Analysis of Algorithms

## About project
 This is my (DAA) courser project [ year-II , semester- I] . It is about  A* (pathfinding algorithm) . The project was made in **unity** ,code was written in **C#** .

## How to run
- You have to make a new project in unity 
- Download Assets
- Replace this Assets with the Assets you have in the new created project
- It's done , now just run the project in unity .

## Unity part
In unity you will see 4 scenes.

<p align="center">
<img  align="center" width="750" height="400" src="https://user-images.githubusercontent.com/29525730/59407661-a567fd80-8dba-11e9-862d-0a29edd0d5b7.png">
<img  align="center" width="400" height="200" src="https://user-images.githubusercontent.com/29525730/59407674-adc03880-8dba-11e9-82a3-e21407fd0f15.png">
</p>



 The first scene **1)ShowPath** is jut for finding the best path from **seeker** (blue sphere) to **target** (green sphere).

<p align="center">
<img  align="center" width="350" height="300" src="https://user-images.githubusercontent.com/29525730/59389433-f0f8b800-8d76-11e9-9d51-a1644041292b.png">
<img  align="center" width="350" height="300" src="https://user-images.githubusercontent.com/29525730/59389081-218c2200-8d76-11e9-98b9-85959e899f76.png">
</p>


- white squares is free nodes, seeker can pass through them.
- red squares is obstacles, seeker can't pas through them, it has to go around them.
- gray squares is all nodes which were taken in consideration during execution of A* algorithm.
- black squares is the best path from seeker to target.

To have the possibility to see just nodes, without obstacles and seeker, or target, you have to go to Scene window and to disable the objects visibility.

Scenes 2,3 and 4 is for visualizing how seeker moves to target. There you will find the different labyrinths and will have the possibility to see how A* works. For that just run the project and press **SPACE** button.

<p align="center">
<img  align="center" width="350" height="300" src="https://user-images.githubusercontent.com/29525730/59389335-b98a0b80-8d76-11e9-8433-1b7d9cbb6930.png">
<img  align="center" width="350" height="300" src="https://user-images.githubusercontent.com/29525730/59389710-b3e0f580-8d77-11e9-8bef-2c9812fdc367.png">
</p>

## A* pseudo code
```
OPEN //the set of nodes to be evaluated
CLOSED //the set of nodes already evaluated
add the start node to OPEN
loop
		current = node in OPEN with the lowest f_cost
		remove current from OPEN
		add current to CLOSED
		
	if current is the target node //path has been found
		return
	foreach neighbour of the current node
		if neighbour is not traversable or neighbour is in CLOSED
			skip to the next neighbour
		if new path to neighbour is shorter OR neighbour is not in OPEN
			set f_cost of neighbour
			set parent of neighbour to current
			if neighbour is not in OPEN
				add neighbour to OPEN

```

