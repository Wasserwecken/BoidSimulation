# Boid simulation Master
![Image of simulation](/.doc/boids.png)

## Release binaries
A build of the project can be found in the [release tab](https://github.com/Wasserwecken/BoidSimulation/releases)

## Simulation rules
- Separation
- Alignment
- Cohesion
- Target point
- Attraction / distraction to other boid types

## Performance implementations
- Nearest neighbor evaluation ist done by a grid system, no use of unity physics
- Boid data gets aggregated in chunks.
- Position based heuristic to evaluate relevant neighbor chunks.
- Memory allocations optimized code.

# Branch seminar
Branch that was used to teach students in a workshop how boids are working.
This simulation uses unity physics to determine its neighbors and a clear, not optimized, code style for the simulation rules for good readability and understanding.