# ManiaMap

[![Tests](https://github.com/mpewsey/ManiaMap/actions/workflows/tests.yml/badge.svg?event=push)](https://github.com/mpewsey/ManiaMap/actions/workflows/tests.yml)

## Purpose

This package allows for the creation of procedurally generated metroidvania style maps, such as that shown below, from user defined level layout graphs and room templates.

![Map](https://user-images.githubusercontent.com/23442063/153345310-25def719-c5a7-43c5-95ca-9e2e09493e54.png)

## Usage

### Step 1: Create a Layout Graph 

The layout generator uses an input graph as the basis for generating layouts. The graph provides input such as:

* How many rooms are there?
* How are rooms connected?
* Do room connections have any constraints, such as one-way doors?
* Which room templates can be used at each location?

The below example creates several nodes (room locations) and connects them by various edges (door connections) in accordance with the shown graph.

```LayoutGraph.cs
var graph = new LayoutGraph(id: 1);

// Define edges between nodes. Nodes not already in graph will automatically be created.
graph.AddEdge(0, 1);
graph.AddEdge(1, 2);
graph.AddEdge(0, 3);
graph.AddEdge(0, 4);
graph.AddEdge(0, 5);

// Add "Default" template group to nodes.
foreach (var node in graph.GetNodes())
{
    node.TemplateGroups.Add("Default");
}
```

![Example Layout Graph](https://user-images.githubusercontent.com/23442063/153694050-f653f3df-8170-4a2e-bd05-8f35083ccfef.png)

### Step 2: Create Room Templates

Room templates consist of a 2D array of cells. Each cell includes the possible door connections that can be made from that cell to other rooms. Null (or unassigned) values for doors indicate one does not exist at that location. Likewise, null values in the cell array indicate that the room does not exist at that location.

An example 3x3 square room template is shown below:

```RoomTemplate.cs
var o = new Cell();
var a = new Cell { LeftDoor = new Door(DoorType.TwoWay), TopDoor = new Door(DoorType.TwoWay) };
var b = new Cell { TopDoor = new Door(DoorType.TwoWay) };
var c = new Cell { TopDoor = new Door(DoorType.TwoWay), RightDoor = new Door(DoorType.TwoWay) };
var d = new Cell { LeftDoor = new Door(DoorType.TwoWay) };
var e = new Cell { RightDoor = new Door(DoorType.TwoWay) };
var f = new Cell { LeftDoor = new Door(DoorType.TwoWay), BottomDoor = new Door(DoorType.TwoWay) };
var g = new Cell { BottomDoor = new Door(DoorType.TwoWay) };
var h = new Cell { BottomDoor = new Door(DoorType.TwoWay), RightDoor = new Door(DoorType.TwoWay) };

var cells = new Cell[,]
{
    { a, b, c },
    { d, o, e },
    { f, g, h },
};

var roomTemplate = new RoomTemplate(id: 1, cells);
```

### Step 3: Assign Room Templates to Template Groups

Once some room templates have been defined, they must be added to one or more room template groups for lookup based on group(s) assigned to the layout graph nodes. This is done by simply adding them to a `TemplateGroups` object:

```TemplateGroups.cs
var templateGroups = new TemplateGroups();
templateGroups.Add("Default", roomTemplate);
```

### Step 4: Run the Layout Generator

Finally, to generate a room layout, pick a random seed and pass your layout graph and room template groups to the `LayoutGenerator`. A map of the layout can also be rendered using either the default tiles (shown in the layout below) or your own custom map tiles.

```LayoutGenerator.cs
var generator = new LayoutGenerator(seed: 0, graph, templateGroups);
var layout = generator.GenerateLayout();

// Render map and save it to file
var map = new LayoutMap(layout);
map.SaveImage("Map.png");
```

![Map](https://user-images.githubusercontent.com/23442063/153345310-25def719-c5a7-43c5-95ca-9e2e09493e54.png)

## References

* GeeksforGeeks. (2021, July 2). Print all the cycles in an undirected graph. Retrieved February 8, 2022, from https://www.geeksforgeeks.org/print-all-the-cycles-in-an-undirected-graph/
* Nepožitek, Ondřej. (2019, January 13). Dungeon Generator (Part 2) – Implementation. Retrieved February 8, 2022, from https://ondra.nepozitek.cz/blog/238/dungeon-generator-part-2-implementation/
