# Roguelike_Tane_Practice

Unity 2D roguelike practice project.

## Current Playable State

- Grid-based movement (WASD / Arrow Keys)
- Turn-based flow (player action -> enemy action)
- Basic combat (adjacent attack, HP, defeat)
- Auto-generated simple test map

## Project Setup

1. Open the project in Unity Hub.
2. Open `Assets/Scenes/SampleScene.unity`.
3. Create an empty GameObject in the scene.
4. Attach `GameBootstrap` component.
5. Press Play.

## Development Workflow

- Keep `main` stable.
- Create feature branches per task:
  - `feature/map-generation`
  - `feature/inventory`
  - `feature/ui-hud`
- Commit in small units with clear intent.
- Merge to `main` only after manual play test.

## Next Recommended Steps

1. Replace generated tiles with Tilemap-based rendering.
2. Add multiple enemies and simple spawn rules.
3. Add floor transitions (stairs).
4. Add minimal HUD (HP + combat log).
5. Add item pickup and inventory.
