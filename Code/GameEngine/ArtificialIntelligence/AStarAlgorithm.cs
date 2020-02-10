using Game_Engine.ArtificialIntelligence;
using Game_Engine.Maps;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Engine.ArtificialIntelligence
{
    public class Path
    {
        public Directions direction;
        public int mapLocationX;
        public int mapLocationY;
        public int weight;
    }

    public class AStarAlgorithm : ArtificialIntelligence
    {
        private List<Path> previousPaths = null;
        private MapData map;
        private Location previousPlayerLocation;
        private Directions preferedDirection;

        private List<Path> CreatePaths(Location pGhostLocation, Location pTargetLocation)
        {
            List<Path> paths = new List<Path>();
            Path ghost = new Path();
            ghost.weight = 0;
            ghost.mapLocationX = pGhostLocation.mapXPosition;
            ghost.mapLocationY = pGhostLocation.mapYPosition;
            ghost.direction = Directions.None;
            paths.Add(ghost);

            Path middleCell = new Path();
            int mapX = pGhostLocation.mapXPosition / 4;
            int mapY = pGhostLocation.mapYPosition / 4;

            middleCell.weight = 0;
            middleCell.mapLocationX = 2 + (mapX * 4);
            middleCell.mapLocationY = 2 + (mapY * 4);
            middleCell.direction = Directions.None;

            paths.Add(middleCell);

            bool foundPath = false;
            FindPath(paths, pTargetLocation, ref foundPath);
            paths.Remove(middleCell);

            SimplifyPathStart(paths);

            return paths;
        }

        private float DegreeToRadians(float angle)
        {
            return angle * (float)(Math.PI / 180.0);
        }

        private Path ExplorePath(int currentX, int currentY, int changeX, int changeY, Location pTargetLocation)
        {
            Path path = new Path();

            path.direction = Directions.None;

            int weight = 0;
            int newX;
            int newY;

            bool breakNext = false;

            while (true)
            {
                newX = currentX + (changeX * weight);
                newY = currentY + (changeY * weight);

                if ((newX == pTargetLocation.mapXPosition && newY == pTargetLocation.mapYPosition) ||
                    //Check up and down of ghost for player including walls
                    (changeX != 0 && newX == pTargetLocation.mapXPosition && newY + 1 == pTargetLocation.mapYPosition) ||
                    (changeX != 0 && newX == pTargetLocation.mapXPosition && newY + 2 == pTargetLocation.mapYPosition) ||
                    (changeX != 0 && newX == pTargetLocation.mapXPosition && newY - 1 == pTargetLocation.mapYPosition) ||
                    (changeX != 0 && newX == pTargetLocation.mapXPosition && newY - 2 == pTargetLocation.mapYPosition) ||
                    //Check left and right of ghost for player including walls
                    (changeY != 0 && newX + 1 == pTargetLocation.mapXPosition && newY == pTargetLocation.mapYPosition) ||
                    (changeY != 0 && newX + 2 == pTargetLocation.mapXPosition && newY == pTargetLocation.mapYPosition) ||
                    (changeY != 0 && newX - 1 == pTargetLocation.mapXPosition && newY == pTargetLocation.mapYPosition) ||
                    (changeY != 0 && newX - 2 == pTargetLocation.mapXPosition && newY == pTargetLocation.mapYPosition))
                {
                    path.direction = Directions.Player;
                    break;
                }

                if (map.GetMap()[newY, newX] == 0)
                {
                    newX = newX - (2 * changeX);
                    newY = newY - (2 * changeY);
                    weight -= 2;
                    break;
                }

                if (breakNext)
                {
                    break;
                }

                if (weight >= 2 && ((changeX != 0 && map.GetMap()[newY + 2, newX] == 1) ||
                    (changeX != 0 && map.GetMap()[newY - 2, newX] == 1) ||
                    (changeY != 0 && map.GetMap()[newY, newX + 2] == 1) ||
                    (changeY != 0 && map.GetMap()[newY, newX - 2] == 1)))
                {
                    breakNext = true;
                }
                weight++;
            }

            if (weight < 2 && path.direction == Directions.None)
            {
                path.direction = Directions.Wall;
            }

            path.mapLocationX = newX;
            path.mapLocationY = newY;

            return path;
        }

        private Location FindFurthestPointFromPlayer(int playerXMapCoord, int playerYMapCoord, int ghostXMapCoord, int ghostYMapCoord)
        {
            int furthestX = 0;
            int furthestY = 0;
            double closestDistanceToGhost = Math.Sqrt(Math.Pow(map.GetWidth(), 2) + Math.Pow(map.GetHeight(), 2)); ;
            double furthestDistanceFromPlayer = 0;

            for (int i = 0; i < map.GetHeight(); i++)
            {
                for (int j = 0; j < map.GetWidth(); j++)
                {
                    if (map.GetMap()[i, j] == 0)
                    {
                        continue;
                    }

                    int diffX = playerXMapCoord - j;
                    int diffY = playerYMapCoord - i;

                    double distanceFromPlayer = Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));

                    diffX = ghostXMapCoord - j;
                    diffY = ghostYMapCoord - i;
                    double distanceFromGhost = Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));

                    if (distanceFromPlayer > furthestDistanceFromPlayer && distanceFromGhost < closestDistanceToGhost)
                    {
                        furthestX = j;
                        furthestY = i;
                        furthestDistanceFromPlayer = distanceFromPlayer;
                        closestDistanceToGhost = distanceFromGhost;
                    }
                }
            }
            Location target = new Location();
            int mapX = furthestX / 4;
            int mapY = furthestY / 4;
            target.mapXPosition = 2 + (mapX * 4);
            target.mapYPosition = 2 + (mapY * 4);
            return target;
        }

        private void FindPath(List<Path> currentPaths, Location pTargetLocation, ref bool foundPath)
        {
            int currentX = currentPaths[currentPaths.Count - 1].mapLocationX;
            int currentY = currentPaths[currentPaths.Count - 1].mapLocationY;
            Directions currentDir = currentPaths[currentPaths.Count - 1].direction;

            List<Path> newPaths = new List<Path>();

            if (currentDir != Directions.Left)
            {
                //Go Right
                Path path = ExplorePath(currentX, currentY, 1, 0, pTargetLocation);
                if (path.direction == Directions.Player)
                {
                    currentPaths.Add(path);
                    Path player = new Path();
                    player.mapLocationX = pTargetLocation.mapXPosition;
                    player.mapLocationY = pTargetLocation.mapYPosition;
                    currentPaths.Add(player);
                    foundPath = true;
                    return;
                }
                else if (path.direction == Directions.None)
                {
                    path.direction = Directions.Right;
                    newPaths.Add(path);
                }
            }
            if (currentDir != Directions.Right)
            {
                //Go Left
                Path path = ExplorePath(currentX, currentY, -1, 0, pTargetLocation);
                if (path.direction == Directions.Player)
                {
                    currentPaths.Add(path);
                    Path player = new Path();
                    player.mapLocationX = pTargetLocation.mapXPosition;
                    player.mapLocationY = pTargetLocation.mapYPosition;
                    currentPaths.Add(player);
                    foundPath = true;
                    return;
                }
                else if (path.direction == Directions.None)
                {
                    path.direction = Directions.Left;
                    newPaths.Add(path);
                }
            }
            if (currentDir != Directions.Up)
            {
                //Go Down
                Path path = ExplorePath(currentX, currentY, 0, -1, pTargetLocation);
                if (path.direction == Directions.Player)
                {
                    currentPaths.Add(path);
                    Path player = new Path();
                    player.mapLocationX = pTargetLocation.mapXPosition;
                    player.mapLocationY = pTargetLocation.mapYPosition;
                    currentPaths.Add(player);
                    foundPath = true;
                    return;
                }
                else if (path.direction == Directions.None)
                {
                    path.direction = Directions.Down;
                    newPaths.Add(path);
                }
            }
            if (currentDir != Directions.Down)
            {
                //Go Up
                Path path = ExplorePath(currentX, currentY, 0, 1, pTargetLocation);
                if (path.direction == Directions.Player)
                {
                    currentPaths.Add(path);
                    Path player = new Path();
                    player.mapLocationX = pTargetLocation.mapXPosition;
                    player.mapLocationY = pTargetLocation.mapYPosition;
                    currentPaths.Add(player);
                    foundPath = true;
                    return;
                }
                else if (path.direction == Directions.None)
                {
                    path.direction = Directions.Up;
                    newPaths.Add(path);
                }
            }

            //Check if the path has already been checked

            for (int i = 0; i < newPaths.Count; i++)
            {
                for (int j = 0; j < currentPaths.Count; j++)
                {
                    if (newPaths[i].mapLocationX == currentPaths[j].mapLocationX && newPaths[i].mapLocationY == currentPaths[j].mapLocationY)
                    {
                        newPaths.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            while (newPaths.Count > 0)
            {
                int shortestPath = 0;
                int diffX = Math.Abs(newPaths[0].mapLocationX - pTargetLocation.mapXPosition);
                int diffY = Math.Abs(newPaths[0].mapLocationY - pTargetLocation.mapYPosition);

                int lowestWeight = newPaths[0].weight + diffX + diffY;

                for (int i = 1; i < newPaths.Count; i++)
                {
                    diffX = Math.Abs(newPaths[i].mapLocationX - pTargetLocation.mapXPosition);
                    diffY = Math.Abs(newPaths[i].mapLocationY - pTargetLocation.mapYPosition);

                    int weight = newPaths[0].weight + diffX + diffY;

                    if (weight < lowestWeight)
                    {
                        shortestPath = i;
                        lowestWeight = weight;
                    }
                    else if (weight == lowestWeight && newPaths[0].direction == preferedDirection)
                    {
                        shortestPath = i;
                    }
                    else if (weight == lowestWeight && newPaths[shortestPath].direction != preferedDirection && newPaths[0].direction == currentDir)
                    {
                        shortestPath = i;
                    }
                }

                currentPaths.Add(newPaths[shortestPath]);
                FindPath(currentPaths, pTargetLocation, ref foundPath);

                if (foundPath)
                {
                    return;
                }
                else
                {
                    currentPaths.RemoveAt(currentPaths.Count - 1);
                    newPaths.RemoveAt(shortestPath);
                }
            }
        }

        private void SimplifyPathStart(List<Path> paths)
        {
            if (paths.Count > 1)
            {
                int diffX = paths[1].mapLocationX - paths[0].mapLocationX;
                int diffY = paths[1].mapLocationY - paths[0].mapLocationY;

                float XLeft;
                float YLeft;
                float XRight;
                float YRight;

                Vector3 changeDirection = new Vector3(diffX, diffY, 0.0f);
                changeDirection = Vector3.Normalize(changeDirection);
                changeDirection *= 0.75f;
                Matrix3 rot = Matrix3.CreateRotationZ(DegreeToRadians(90f));
                Vector3 left = changeDirection * rot;
                XLeft = paths[0].mapLocationX + map.GetStartY() + left.X;
                YLeft = paths[0].mapLocationY + map.GetStartY() + left.Y;

                rot = Matrix3.CreateRotationZ(DegreeToRadians(-90f));
                Vector3 right = changeDirection * rot;
                XRight = paths[0].mapLocationX + map.GetStartX() + right.X;
                YRight = paths[0].mapLocationY + map.GetStartY() + right.Y;

                float distance = (float)Math.Sqrt(Math.Pow(diffX, 2) + Math.Pow(diffY, 2));
                int steps = (int)distance * 10;

                bool simplify = true;
                for (int k = 0; k < steps; k++)
                {
                    float XMapCoord = (paths[0].mapLocationX + map.GetStartX() + (k * (float)diffX / steps));
                    float YMapCoord = (paths[0].mapLocationY + map.GetStartY() + (k * (float)diffY / steps));

                    int targetXMapCoord = (int)Math.Abs((map.GetStartX() - 0.5f) - XMapCoord);
                    int targetYMapCoord = (int)Math.Abs((map.GetStartY() - 0.5f) - YMapCoord);

                    if (map.GetMap()[targetXMapCoord, targetYMapCoord] == 0)
                    {
                        simplify = false;
                        break;
                    }
                }

                for (int k = 0; simplify && k < steps; k++)
                {
                    float XMapCoord = (XLeft + (k * (float)diffX / steps));
                    float YMapCoord = (YLeft + (k * (float)diffY / steps));

                    int targetXMapCoord = (int)Math.Abs((map.GetStartX() - 0.5f) - XMapCoord);
                    int targetYMapCoord = (int)Math.Abs((map.GetStartY() - 0.5f) - YMapCoord);

                    if (map.GetMap()[targetXMapCoord, targetYMapCoord] == 0)
                    {
                        simplify = false;
                        break;
                    }
                }

                for (int k = 0; simplify && k < steps; k++)
                {
                    float XMapCoord = (XRight + (k * (float)diffX / steps));
                    float YMapCoord = (YRight + (k * (float)diffY / steps));

                    int targetXMapCoord = (int)Math.Abs((map.GetStartX() - 0.5f) - XMapCoord);
                    int targetYMapCoord = (int)Math.Abs((map.GetStartY() - 0.5f) - YMapCoord);

                    if (map.GetMap()[targetXMapCoord, targetYMapCoord] == 0)
                    {
                        simplify = false;
                        break;
                    }
                }

                if (simplify)
                {
                    paths.RemoveAt(0);
                }
            }
        }

        public override Vector3 BuildPath(Vector2 pGhostLocation, Location pGhostMapLocation, MapData pMap, Location playerLocation, Directions pPreferedDirection)
        {
            map = pMap;
            preferedDirection = pPreferedDirection;

            Location target = playerLocation;
            if (map.HasPowerup())
            {
                //Target at this point means furthest point from player
                target = FindFurthestPointFromPlayer(playerLocation.mapXPosition, playerLocation.mapYPosition, pGhostMapLocation.mapXPosition, pGhostMapLocation.mapYPosition);
            }

            if (previousPaths == null || previousPlayerLocation.mapXPosition != target.mapXPosition || previousPlayerLocation.mapYPosition != target.mapYPosition)
            {
                previousPaths = CreatePaths(pGhostMapLocation, target);
                previousPlayerLocation = target;
            }

            for (int i = 0; i < previousPaths.Count - 1; i++)
            {
                Vector2 ghostMapLocation = new Vector2(previousPaths[i].mapLocationX + map.GetStartX(), previousPaths[i].mapLocationY + map.GetStartY());

                if ((ghostMapLocation - pGhostLocation).Length < 0.05f)
                {
                    previousPaths.RemoveAt(0);
                    i--;
                }
            }



            return new Vector3(previousPaths[0].mapLocationX + map.GetStartX(), 0.0f, previousPaths[0].mapLocationY + map.GetStartY());
        }
    }
}
