# Boid simulation Master
![Image of simulation](/.doc/boids.png)

## Releases
A binary for windows be found in the [release tab](https://github.com/Wasserwecken/BoidSimulation/releases)

Or try the web version right now: [webGL build](https://wasserwecken.github.io/BoidSimulation/)

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