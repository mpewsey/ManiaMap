# ManiaMap

[![Tests](https://github.com/mpewsey/ManiaMap/actions/workflows/tests.yml/badge.svg?event=push)](https://github.com/mpewsey/ManiaMap/actions/workflows/tests.yml)
[![codecov](https://codecov.io/gh/mpewsey/ManiaMap/branch/main/graph/badge.svg?token=Q1LDU83FAQ)](https://codecov.io/gh/mpewsey/ManiaMap)

## About

This package allows for the creation of procedurally generated metroidvania style layouts from user-defined level layout graphs and room templates. The resuling layouts can be used in games, such as those of the rouge-like genre, or to render maps, such as that shown below. The generation algorithm is based on [[2]](#2) but does not include a simulated annealing evolver. 

![Map](https://user-images.githubusercontent.com/23442063/153345310-25def719-c5a7-43c5-95ca-9e2e09493e54.png)

## Features

* Single and multi-layer 2D room layout generation.
* Graph-based procedural generation.
* Specification of room connection constraints by defining door directions and matching door codes.
* Rendering of generated layouts to image files using built-in or custom map tiles.
* Supports .NET Standard 2.0.
* Works with Windows, Mac, and Linux.

## Dependencies

The package modules have the below dependencies. External dependencies can be acquired through NuGet.

| Module             | Description                         | Dependencies                       |
|--------------------|-------------------------------------|------------------------------------|
| `ManiaMap`         | Contains core generator components. | None                               |
| `ManiaMap.Drawing` | Contains map drawing components.    | `ManiaMap`, `SixLabors.ImageSharp` |

## Usage

### Step 1: Create Room Templates

The generator creates rooms by pulling from user-defined room templates. Room templates consist of a 2D array of cells, that may be empty (a null value) or filled. Each cell includes the possible door connections (north, south, east, west, top, and/or bottom) that can be made from that cell to other rooms. Furthermore, each door includes a door code, which requires a match in another template for a connection to be made, as well as a traversal type, such the door being one-way or two-way.

Depending on how much variety you want to be included in your layout, one or more templates are required by the generator. The following code provides an example for the creation of a simple 3x3 square room template. In it, a series of cells are created and have doors set to them via directional characters ("N" = North, "S" = South, etc.). The cells are then assigned to a 2D array to create the geometry of the layout. Finally, these cells are passed to the `RoomTemplate` initializer, along with a unique ID, to create the room template.

```RoomTemplate.cs
var o = Cell.New;
var a = Cell.New.SetDoors("WN", Door.TwoWay);
var b = Cell.New.SetDoors("N", Door.TwoWay);
var c = Cell.New.SetDoors("NE", Door.TwoWay);
var d = Cell.New.SetDoors("W", Door.TwoWay);
var e = Cell.New.SetDoors("E", Door.TwoWay);
var f = Cell.New.SetDoors("WS", Door.TwoWay);
var g = Cell.New.SetDoors("S", Door.TwoWay);
var h = Cell.New.SetDoors("SE", Door.TwoWay);

var cells = new Cell[,]
{
    { a, b, c },
    { d, o, e },
    { f, g, h },
};

var roomTemplate = new RoomTemplate(id: 1, name: "Square", cells);
```

### Step 2: Assign Room Templates to Template Groups

Once some room template have been defined, they must be added to one or more room template groups, which can be referenced by name later. This is accomplished by simply adding them to a `TemplateGroups` object:

```TemplateGroups.cs
var templateGroups = new TemplateGroups();
templateGroups.Add("Default", roomTemplate);
```

### Step 3: Create a Layout Graph 

To provide a designed feel to generated layouts, the generator uses a room layout graph as the basis for generating layouts. The layout graph consists of nodes, representing rooms, and edges, representing door connections between rooms. Ultimately, the layout graph contains information to help guide the features of a room layout. Each graph element can be assigned one or more room template groups (created in Step 2) from which room teplates can be drawn for that location. Z (layer) values can also be assigned to elements to create a multi-level room layout. In addition, properties such as names and colors can be assigned that will be passed on to the generated rooms, allowing for their use elsewhere in a game or other application.

In the below example, the graph shown in the image is created by adding edges to a graph. In the process, the nodes, representing rooms, are automatically created. Afterwards, the code loops over all of the created nodes and assigns a "Default" template group to them, from which room templates will be drawn by the generator.

```LayoutGraph.cs
var graph = new LayoutGraph(id: 1, name: "ExampleGraph");

// Define edges between nodes. Nodes not already in graph will automatically be created.
graph.AddEdge(0, 1);
graph.AddEdge(1, 2);
graph.AddEdge(0, 3);
graph.AddEdge(0, 4);
graph.AddEdge(0, 5);

// Add "Default" template group to nodes.
foreach (var node in graph.GetNodes())
{
    node.AddTemplateGroups("Default");
}
```

![Example Layout Graph](https://user-images.githubusercontent.com/23442063/153694050-f653f3df-8170-4a2e-bd05-8f35083ccfef.png)

### Step 4: Run the Layout Generator

To create repeatable layouts, a random seed is used by the layout generator. To generate a room layout, simply select a seed and pass your layout graph and room template groups to the `LayoutGenerator`, as shown below. In the example, a map of the layout is also rendered and saved using the `LayoutMap` class. For this instance, the default built-in tiles are used. However, custom tiles can also be specified by the user.

```LayoutGenerator.cs
var generator = new LayoutGenerator(seed: 12345, graph, templateGroups);
var layout = generator.GenerateLayout();

// Render map and save it to file
var map = new LayoutMap(layout);
map.SaveImage("Map.png");
```

![Map](https://user-images.githubusercontent.com/23442063/153345310-25def719-c5a7-43c5-95ca-9e2e09493e54.png)

## References

* <a id="1">[1]</a> GeeksforGeeks. (2021, July 2). Print all the cycles in an undirected graph. Retrieved February 8, 2022, from https://www.geeksforgeeks.org/print-all-the-cycles-in-an-undirected-graph/
 
* <a id="2">[2]</a> Nepožitek, Ondřej. (2019, January 13). Dungeon Generator (Part 2) – Implementation. Retrieved February 8, 2022, from https://ondra.nepozitek.cz/blog/238/dungeon-generator-part-2-implementation/
