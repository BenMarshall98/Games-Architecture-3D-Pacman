using Game_Engine.ArtificialIntelligence;
using Game_Engine.Component;
using Game_Engine.Managers;
using Game_Engine.Maps;
using Game_Engine.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenGL_Game.Maps
{
    struct PlayerData
    {
        public Vector3 startPosition;
        public Entity entity;
    }

    class MapLoader : MapData
    {
        List<Vector3> previousLocations = new List<Vector3>();

        Vector3 ghostDeadZone;

        PlayerData player;
        PlayerData inky;
        PlayerData blinky;
        PlayerData pinky;
        PlayerData clyde;

        int replaceNext = 0;

        public MapLoader() : base(3, 20) { }

        public override void LoadMap(string filename)
        {
            string line;

            try
            {
                FileStream fin = File.OpenRead(filename);
                StreamReader reader = new StreamReader(fin);

                List<string> lines = new List<string>();

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    lines.Add(line);
                }

                width = lines[0].Length;
                height = lines.Count;

                startX = -(float)width / 2 + 0.5f;
                startY = -(float)height / 2 + 0.5f;

                map = new int[height, width];

                EntityManager entityManager = EntityManager.Instance();
                Entity newEntity;

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        float XPosition = startX + j;
                        float ZPosition = startY + i;
                        if (lines[i][j] == 'w')
                        {
                            newEntity = new Entity("Wall" + i.ToString() + "," + j.ToString());
                            newEntity.AddComponent(new ComponentPosition(XPosition, 0.0f, ZPosition));
                            newEntity.AddComponent(new ComponentGeometry("Geometry/Wall.obj"));
                            newEntity.AddComponent(new ComponentTexture("Textures/wall.png"));
                            newEntity.AddComponent(new ComponentShader("Shaders/vs.glsl", "Shaders/fs.glsl"));
                            newEntity.AddComponent(new ComponentCollisionSquare(1.0f, 1.0f));
                            newEntity.AddComponent(new ComponentDirection(0.0f, 0.0f, 1.0f, 1));
                            entityManager.AddEntity(newEntity);
                            map[i, j] = 0;
                        }
                        else
                        {
                            map[i, j] = 1;
                            newEntity = new Entity("Floor" + i.ToString() + "," + j.ToString());
                            newEntity.AddComponent(new ComponentPosition(XPosition, 0.0f, ZPosition));
                            newEntity.AddComponent(new ComponentGeometry("Geometry/Floor.obj"));
                            newEntity.AddComponent(new ComponentDirection(0.0f, 0.0f, 1.0f, 1));

                            if (lines[i + 1][j] == 'w' && lines[i][j - 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/TopLeft.png"));
                            }
                            else if (lines[i + 1][j] == 'w' && lines[i][j + 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/TopRight.png"));
                            }
                            else if (lines[i - 1][j] == 'w' && lines[i][j - 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/BottomLeft.png"));
                            }
                            else if (lines[i - 1][j] == 'w' && lines[i][j + 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/BottomRight.png"));
                            }
                            else if (lines[i - 1][j] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Bottom.png"));
                            }
                            else if (lines[i + 1][j] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Top.png"));
                            }
                            else if (lines[i][j - 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Left.png"));
                            }
                            else if (lines[i][j + 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Right.png"));
                            }
                            else if (lines[i - 1][j - 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/BottomLeftCorner.png"));
                            }
                            else if (lines[i + 1][j + 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/TopRightCorner.png"));
                            }
                            else if (lines[i + 1][j - 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/TopLeftCorner.png"));
                            }
                            else if (lines[i - 1][j + 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/BottomRightCorner.png"));
                            }
                            else if (lines[i - 2][j] == 'w' && lines[i + 2][j] != 'w' && lines[i][j - 2] == 'w' && lines[i][j + 2] != 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/TopRightTurn.png"));
                            }
                            else if (lines[i - 2][j] == 'w' && lines[i + 2][j] != 'w' && lines[i][j - 2] != 'w' && lines[i][j + 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/TopLeftTurn.png"));
                            }
                            else if (lines[i - 2][j] != 'w' && lines[i + 2][j] == 'w' && lines[i][j - 2] == 'w' && lines[i][j + 2] != 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/BottomRightTurn.png"));
                            }
                            else if (lines[i - 2][j] != 'w' && lines[i + 2][j] == 'w' && lines[i][j - 2] != 'w' && lines[i][j + 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/BottomLeftTurn.png"));
                            }
                            else if (lines[i - 2][j] == 'w' && lines[i + 2][j] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Horizontal.png"));
                            }
                            else if (lines[i][j - 2] == 'w' && lines[i][j + 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Vertical.png"));
                            }
                            else if (lines[i][j - 2] != 'w' && lines[i + 1][j - 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Vertical.png"));
                            }
                            else if (lines[i][j - 2] != 'w' && lines[i - 1][j - 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Vertical.png"));
                            }
                            else if (lines[i][j + 2] != 'w' && lines[i + 1][j + 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Vertical.png"));
                            }
                            else if (lines[i][j + 2] != 'w' && lines[i - 1][j + 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Vertical.png"));
                            }
                            else if (lines[i + 2][j] != 'w' && lines[i + 2][j + 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Horizontal.png"));
                            }
                            else if (lines[i + 2][j] != 'w' && lines[i + 2][j - 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Horizontal.png"));
                            }
                            else if (lines[i - 2][j] != 'w' && lines[i - 2][j + 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Horizontal.png"));
                            }
                            else if (lines[i - 2][j] != 'w' && lines[i - 2][j - 1] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/Horizontal.png"));
                            }
                            else if (lines[i - 2][j] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/BottomWall.png"));
                            }
                            else if (lines[i + 2][j] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/TopWall.png"));
                            }
                            else if (lines[i][j + 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/RightWall.png"));
                            }
                            else if (lines[i][j - 2] == 'w')
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/LeftWall.png"));
                            }
                            else
                            {
                                newEntity.AddComponent(new ComponentTexture("Textures/NoWall.png"));
                            }
                            newEntity.AddComponent(new ComponentShader("Shaders/vs.glsl", "Shaders/fs.glsl"));
                            entityManager.AddEntity(newEntity);

                            if (lines[i][j] == 'c')
                            {
                                //Add Collectable
                                newEntity = new Entity("Collectable" + i.ToString() + "," + j.ToString());
                                newEntity.AddComponent(new ComponentPosition(XPosition, 0.5f, ZPosition));
                                newEntity.AddComponent(new ComponentGeometry("Geometry/Collectable.obj"));
                                newEntity.AddComponent(new ComponentTexture("Textures/testCoin.png"));
                                newEntity.AddComponent(new ComponentShader("Shaders/vs.glsl", "Shaders/fs.glsl"));
                                newEntity.AddComponent(new ComponentCollisionCircle(0.1f));
                                newEntity.AddComponent(new ComponentEntityType(EntityType.COLLECTABLE));
                                newEntity.AddComponent(new ComponentDirection(0.0f, 0.0f, 1.0f, 1));
                                newEntity.AddComponent(new ComponentAnimation(AnimationType.ROTATION_Y, 5.0f));
                                entityManager.AddEntity(newEntity);
                                coinsToCollect++;
                            }
                            else if (lines[i][j] == 'p')
                            {
                                //Add Player
                                newEntity = new Entity("Player");
                                newEntity.AddComponent(new ComponentPosition(XPosition, 1.0f, ZPosition));
                                newEntity.AddComponent(new ComponentTexture("Textures/testCoin.png"));
                                newEntity.AddComponent(new ComponentGeometry("Geometry/Pacman.obj"));
                                newEntity.AddComponent(new ComponentShader("Shaders/vs.glsl", "Shaders/fs.glsl"));
                                newEntity.AddComponent(new ComponentDirection(0.0f, 0.0f, 1.0f, 45f));
                                newEntity.AddComponent(new ComponentVelocity(5));
                                newEntity.AddComponent(new ComponentInput());
                                newEntity.AddComponent(new ComponentCollisionCircle(1f));
                                newEntity.AddComponent(new ComponentEntityType(EntityType.PLAYER));
                                entityManager.AddEntity(newEntity);
                                player.entity = newEntity;
                                player.startPosition = new Vector3(XPosition, 1.0f, ZPosition);
                            }
                            else if (lines[i][j] == 'g')
                            {
                                ghostDeadZone = new Vector3(XPosition, 0.0f, ZPosition);
                            }
                            else if (lines[i][j] == '1')
                            {
                                //Add Inky
                                newEntity = CreateGhost("Inky", "Textures/Inky.png", XPosition, ZPosition);
                                newEntity.AddComponent(new ComponentArtificalInput(Directions.Down, 10f, new AStarAlgorithm()));
                                entityManager.AddEntity(newEntity);
                                inky.entity = newEntity;
                                inky.startPosition = new Vector3(XPosition, 0.0f, ZPosition);

                                AddPowerUp(entityManager, i, j, XPosition, ZPosition);
                            }
                            else if (lines[i][j] == '2')
                            {
                                //Add Blinky
                                newEntity = CreateGhost("Blinky", "Textures/Blinky.png", XPosition, ZPosition);
                                newEntity.AddComponent(new ComponentArtificalInput(Directions.Up, 20f, new AStarAlgorithm()));
                                entityManager.AddEntity(newEntity);
                                blinky.entity = newEntity;
                                blinky.startPosition = new Vector3(XPosition, 0.0f, ZPosition);

                                AddPowerUp(entityManager, i, j, XPosition, ZPosition);
                            }
                            else if (lines[i][j] == '3')
                            {
                                //Add Pinky
                                newEntity = CreateGhost("Pinky", "Textures/Pinky.png", XPosition, ZPosition);
                                newEntity.AddComponent(new ComponentArtificalInput(Directions.Left, 30f, new AStarAlgorithm()));
                                entityManager.AddEntity(newEntity);
                                pinky.entity = newEntity;
                                pinky.startPosition = new Vector3(XPosition, 0.0f, ZPosition);

                                AddPowerUp(entityManager, i, j, XPosition, ZPosition);
                            }
                            else if (lines[i][j] == '4')
                            {
                                //Add Clyde
                                newEntity = CreateGhost("Clyde", "Textures/Clyde.png", XPosition, ZPosition);
                                newEntity.AddComponent(new ComponentArtificalInput(Directions.Right, 40f, new AStarAlgorithm()));
                                entityManager.AddEntity(newEntity);
                                clyde.entity = newEntity;
                                clyde.startPosition = new Vector3(XPosition, 0.0f, ZPosition);

                                AddPowerUp(entityManager, i, j, XPosition, ZPosition);
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public override Entity GetPlayer()
        {
            return player.entity;
        }

        public override void LoseLive()
        {
            ResetPlayer(player);
            ResetPlayer(inky);
            ResetPlayer(blinky);
            ResetPlayer(pinky);
            ResetPlayer(clyde);
            lives--;
        }

        private void ResetPlayer(PlayerData player)
        {
            List<IComponent> components = player.entity.Components;

            IComponent positionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });

            IComponent directionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_DIRECTION;
            });

            //Done Twice to prevent the swap back method from moving back to death position
            ((ComponentPosition)positionComponent).Position = player.startPosition;
            ((ComponentPosition)positionComponent).Position = player.startPosition;

            ((ComponentDirection)directionComponent).Direction = new Vector3(0.0f, 0.0f, 1.0f);
        }

        private void AddPowerUp(EntityManager entityManager, int mapX, int mapY, float posX, float posY)
        {
            Entity newEntity = new Entity("Powerup" + mapX.ToString() + "," + mapY.ToString());
            newEntity.AddComponent(new ComponentPosition(posX, 0.5f, posY));
            newEntity.AddComponent(new ComponentGeometry("Geometry/Powerup.obj"));
            newEntity.AddComponent(new ComponentTexture("Textures/Fruit.png"));
            newEntity.AddComponent(new ComponentShader("Shaders/vs.glsl", "Shaders/fs.glsl"));
            newEntity.AddComponent(new ComponentAudio("Audio/Sonar.wav", true));
            newEntity.AddComponent(new ComponentCollisionCircle(0.1f));
            newEntity.AddComponent(new ComponentEntityType(EntityType.POWERUP));
            newEntity.AddComponent(new ComponentDirection(0.0f, 0.0f, 1.0f, 1));
            newEntity.AddComponent(new ComponentAnimation(AnimationType.ROTATION_Y, 5.0f));
            entityManager.AddEntity(newEntity);
        }

        private Entity CreateGhost(string name, string texture, float XPosition, float ZPosition)
        {
            Entity newEntity = new Entity(name);
            newEntity.AddComponent(new ComponentPosition(XPosition, 0.0f, ZPosition));
            newEntity.AddComponent(new ComponentDirection(0.0f, 0.0f, 1.0f, 30f));
            newEntity.AddComponent(new ComponentTexture(texture));
            newEntity.AddComponent(new ComponentGeometry("Geometry/PacmanGhost.obj"));
            newEntity.AddComponent(new ComponentShader("Shaders/vs.glsl", "Shaders/fs.glsl"));
            newEntity.AddComponent(new ComponentVelocity(4));
            newEntity.AddComponent(new ComponentCollisionCircle(0.5f));
            newEntity.AddComponent(new ComponentEntityType(EntityType.GHOST));
            newEntity.AddComponent(new ComponentAnimation(AnimationType.OSCILLATION_Y, 2.5f, 0.25f));
            return newEntity;
        }

        public override void KillGhost(Entity entity)
        {
            List<IComponent> components = entity.Components;

            IComponent positionComponent = components.Find(delegate (IComponent component)
            {
                return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
            });

            Vector3 position;
            
            while(true)
            {
                Random random = new Random();

                int XDiff = (int)(random.NextDouble() * 3) - 1;
                int ZDiff = (int)(random.NextDouble() * 3) - 1;

                position = new Vector3(ghostDeadZone.X + XDiff, 0.0f, ghostDeadZone.Z + ZDiff);

                bool usedBefore = false;

                for (int i = 0; i < previousLocations.Count; i++)
                {
                    if(previousLocations[i] == position)
                    {
                        usedBefore = true;
                    }
                }

                if (!usedBefore)
                {
                    if (previousLocations.Count != 4)
                    {
                        previousLocations.Add(position);
                    }
                    else if (replaceNext < 4)
                    {
                        previousLocations[replaceNext] = position;
                        replaceNext++;
                    }
                    else
                    {
                        previousLocations[0] = position;
                        replaceNext = 1;
                    }
                    break;
                }
            }
            

            ((ComponentPosition)positionComponent).Position = position;
        }
    }
}
