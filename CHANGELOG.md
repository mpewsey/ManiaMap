# Changelog

## Development
* Added support for the creation of randomized layout graph variations. This may be implemented within a generation pipline using the new `LayoutGraphRandomizer` generation step.
* Added support for selecting a random layout graph from a list of multiple within a generation pipeline. This may be implemented using the new `LayoutGraphSelector` generation step.
* `RandomSeed` now implements its own randomization algorithm, rather than relying on the .NET `Random` class. This allows for consistency between .NET versions, as well as for the serialization of the random number generator.

## v1.0.0
* Initial release.
