# Boid simulation
![Image of simulation](/media/simulation.png)

## Simulation rules
- Seperation
- Alignment
- Cohesion
- Target point
- Attraction to other boid types

## Performance implementations
- Nearest neighbor evaluation ist done by a grid system, no use of unity physics
- Boid data gets aggregated per grid chunk.
- Position based heuristic to evaluate relevant neighbor chunks.
- Avoidance of memory allocations. (As far as possible)
