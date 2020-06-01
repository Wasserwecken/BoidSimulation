# Boid simulation Master
![Image of simulation](/gitMedia/boids.png)

## Requirements
- Unity 2019.3

## Simulation rules
- Seperation
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
This simulation uses unity physics to determine its neighbours and a clear, not optimized, code style for the simulation rules for good readablility and understanding.