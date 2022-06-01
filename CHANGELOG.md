# Changelog

## v2.0.0
* Change cell implementation.

## v1.5.0
* Changed the way that collectable spots are implemented. Unique ID's must now be unique to the room, not to the cell. This breaks compatibility with previous versions.

## v1.4.1
* Remove None enum from `DoorDirection` and `DoorType`.

## v1.4.0
* Added validation to layout graphs, room templates, collectable groups, and template groups.

## v1.3.0
* Added support for multiple collectables within a single cell. This breaks compatibility with previous versions.
* Added new exceptions namespace with named exceptions.

## v1.2.0
* Added rebase decay rate to layout generator. This addition generally improves layout generation times and increases the success rate of layout generation.
* Add results container to generation pipeline.

## v1.1.0
* Added support for the creation of randomized layout graph variations. This may be implemented within a generation pipeline using the new `LayoutGraphRandomizer` generation step.
* Added support for selecting a random layout graph from a list of multiple within a generation pipeline. This may be implemented using the new `LayoutGraphSelector` generation step.
* `RandomSeed` now implements its own randomization algorithm, rather than relying on the .NET `Random` class. This allows for consistency between .NET versions, as well as for the serialization of the random number generator.

## v1.0.0
* Initial release.
