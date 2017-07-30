using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

/// <summary>
/// Author: Andrew Seba
/// Description: Imports tile maps from xml tiled data
/// </summary>
public class TileImport : MonoBehaviour {

    [Tooltip("Xml File to load map of")]
    public TextAsset mapInformation;
    [HideInInspector]
    public int layerWidth;
    [HideInInspector]
    public int layerHeight;

    GameObject collisionParent;
    private Sprite[] spriteTiles;

    private const int PLAYERSPAWN = 964;
    private const int WEED_ONE = 592;
    private const int WEED_TWO = 649;

    private void Awake()
    {
        //Try loading asset from folder
        //Texture2D dynamicAssetTexture = null;
        //byte[] imageBytes;
        //string imagePath = Application.dataPath + "/";
        //foreach(string file in Directory.GetFiles(imagePath))
        //{
        //    Debug.Log(Application.dataPath + "/" + file);

        //    if (file.EndsWith(".png"))
        //    {
        //        imageBytes = File.ReadAllBytes(Application.dataPath + "/" + file);
        //        Debug.Log("SUCCESS!");
        //        System.Drawing.Image img
        //        dynamicAssetTexture = new Texture2D(Syste)
        //    }
        //}

        collisionParent = new GameObject("CollisionGridLayer");
        StartCoroutine("LoadMap");
    }

    IEnumerator LoadMap()
    {
        try
        {
            spriteTiles = Resources.LoadAll<Sprite>("roguelikeSheet");
        }
        catch
        {
            Debug.LogWarning("Couldn't load in sprite sheet.");
        }

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.LoadXml(mapInformation.text);

        //I think layer is a "Tiled" element in the xml
        XmlNodeList layerNames = xmlDoc.GetElementsByTagName("layer");

        XmlNode tilesetInfo = xmlDoc.SelectSingleNode("map").SelectSingleNode("tileset");
        float tileWidth = (float.Parse(tilesetInfo.Attributes["tilewidth"].Value) / (float)16);
        float tileHeight = (float.Parse(tilesetInfo.Attributes["tileheight"].Value) / (float)16);

        //Generate Collision grid for mouse input.
        //int width = int.Parse(xmlDoc.SelectSingleNode("map").Attributes["width"].Value);
        //int height = int.Parse(xmlDoc.SelectSingleNode("map").Attributes["height"].Value);

        //Tile[,] allTiles = new Tile[width, height];



        //for (int i = 0; i < height; i++)
        //{
        //    for (int j = 0; j < width; j++)
        //    {
        //        GameObject tempSprite = new GameObject("gid(" + i + "," + j + ")");
        //        tempSprite.AddComponent<BoxCollider2D>();
        //        //set position
        //        tempSprite.transform.position = new Vector3((tileWidth * i), (tileHeight * j));
        //        tempSprite.tag = "Tile";
        //        tempSprite.transform.parent = collisionParent.transform;
        //    }
        //}

        //Build Basic Connections
        //foreach (Tile tile in allTiles)
        //{
        //    //Left
        //    if (tile.x - 1 >= 0)
        //    {
        //        tile.Connections.Add(new ScriptConnection(tile.gameObject, allTiles[tile.x - 1, tile.y].gameObject, 1));
        //    }
        //    if (tile.x + 1 < width)
        //    {
        //        tile.Connections.Add(new ScriptConnection(tile.gameObject, allTiles[tile.x + 1, tile.y].gameObject, 1));
        //    }
        //    if (tile.y - 1 >= 0)
        //    {
        //        tile.Connections.Add(new ScriptConnection(tile.gameObject, allTiles[tile.x, tile.y - 1].gameObject, 1));
        //    }
        //    if (tile.y + 1 < height)
        //    {
        //        tile.Connections.Add(new ScriptConnection(tile.gameObject, allTiles[tile.x, tile.y + 1].gameObject, 1));
        //    }
        //}



        //for each layer that exists
        foreach (XmlNode layerInfo in layerNames)
        {
            layerWidth = int.Parse(layerInfo.Attributes["width"].Value);
            layerHeight = int.Parse(layerInfo.Attributes["height"].Value);

            //Pull out of the data node
            XmlNode tempNode = layerInfo.SelectSingleNode("data");

            int verticalIndex = layerHeight - 1;
            int horizontalIndex = 0;

            foreach (XmlNode tile in tempNode.SelectNodes("tile"))
            {
                int spriteValue = int.Parse(tile.Attributes["gid"].Value);


                //if not empty
                if (spriteValue > 0)
                {
                    Sprite[] currentSpriteSheet = spriteTiles;
                    //Create a sprite
                    GameObject tempSprite = new GameObject(layerInfo.Attributes["name"].Value + " <" + horizontalIndex + ", " + verticalIndex + ">");


                    //Make a sprite renderer.
                    SpriteRenderer spriteRend = tempSprite.AddComponent<SpriteRenderer>();
                    //get sprite from sheet.
                    spriteRend.sprite = currentSpriteSheet[spriteValue - 1];
                    //set position
                    tempSprite.transform.position = new Vector3((tileWidth * horizontalIndex), (tileHeight * verticalIndex));
                    //set sorting layer
                    spriteRend.sortingLayerName = layerInfo.Attributes["name"].Value;

                    //set parent
                    GameObject parent = GameObject.Find(layerInfo.Attributes["name"].Value + "Layer");
                    if (parent == null)
                    {
                        parent = new GameObject();
                        parent.name = layerInfo.Attributes["name"].Value + "Layer";
                    }
                    tempSprite.transform.parent = GameObject.Find(layerInfo.Attributes["name"].Value + "Layer").transform;
                    tempSprite.tag = "Tile";


                    if (layerInfo.Attributes["name"].Value == "Roof object")
                    {

                    }
                    else if (layerInfo.Attributes["name"].Value == "Doors/windows/roof")
                    {

                    }
                    else if(layerInfo.Attributes["name"].Value == "Objects")
                    {
                        switch (spriteValue - 1)
                        {
                            case PLAYERSPAWN:
                                Debug.Log("Player found at " + spriteValue);
                                break;
                            case WEED_ONE:
                            case WEED_TWO:
                                BoxCollider2D boxCol = tempSprite.AddComponent<BoxCollider2D>();
                                boxCol.isTrigger = true;
                                tempSprite.tag = "weed";
                                break;
                            default:
                                tempSprite.AddComponent<BoxCollider2D>();
                                break;

                        }
                    }
                    tempSprite.isStatic = true;
                    #region old
                    //else if (layerInfo.Attributes["name"].Value == "Terrain")
                    //{
                    //    switch (spriteValue - 1)
                    //    {
                    //        case FOREST:
                    //            tempSprite.name = "Forest";
                    //            Terrain tempForest = tempSprite.AddComponent<Terrain>();
                    //            tempForest.x = horizontalIndex;
                    //            tempForest.y = verticalIndex;
                    //            tempForest.terrainType = TerrainType.Forest;
                    //            FindParent(tempForest);
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}

                    //tempSprite.tag = "Entity";

                    //int value = spriteValue - 1;
                    //switch (value)
                    //{
                    //    case VILLAGER_BLU_M:
                    //    case VILLAGER_M:
                    //        tempSprite.name = "Villager";
                    //        Human tempVillager = tempSprite.AddComponent<Human>();
                    //        if (value == VILLAGER_BLU_M)
                    //        {
                    //            tempVillager.SetupHuman(Class.Villager, Faction.Blue, horizontalIndex, verticalIndex);
                    //        }
                    //        else if (value == VILLAGER_RED_M)
                    //        {
                    //            tempVillager.SetupHuman(Class.Villager, Faction.Red, horizontalIndex, verticalIndex);
                    //        }
                    //        FindParent(tempVillager);
                    //        break;
                    //    case KNIGHT:
                    //        tempSprite.name = "Knight";
                    //        Human knight = tempSprite.AddComponent<Human>();
                    //        knight.x = horizontalIndex;
                    //        knight.y = verticalIndex;
                    //        knight.ClassType = Class.Knight;
                    //        FindParent(knight);
                    //        break;
                    //    default:
                    //        Debug.Log("Unknown Entity placed in map(no info loaded for sprite index: " + (spriteValue - 1) + ").");
                    //        break;
                    //}
                    #endregion
                }
                horizontalIndex++;
                if (horizontalIndex % layerWidth == 0)
                {
                    //Increase our vertical location
                    verticalIndex--;
                    //reset our horizontal location
                    horizontalIndex = 0;
                }
            }

        }//End of placing sprites
        //rebuild costs in the connections

        yield break;
    }

    //void FindParent(Tile tile)
    //{
    //    foreach (Transform tilObj in collisionParent.transform)
    //    {
    //        if (tilObj.GetComponent<Tile>().x == tile.x && tilObj.GetComponent<Tile>().y == tile.y)
    //        {
    //            Tile tileScript = tilObj.GetComponent<Tile>();
    //            if (tile is Human)
    //            {
    //                Human tempHuman = (Human)tile;
    //                tempHuman.tileOccuping = tileScript;
    //                tileScript.occupiedBy = tempHuman;
    //            }
    //            else if (tile is Terrain)
    //            {
    //                Terrain tempTerrain = (Terrain)tile;
    //                int terrainCost = 0;
    //                switch (tempTerrain.terrainType)
    //                {
    //                    case TerrainType.Forest:
    //                        terrainCost = 2;
    //                        break;
    //                    default:
    //                        terrainCost = 0;
    //                        break;
    //                }
    //                foreach (ScriptConnection conn in tileScript.Connections)
    //                {
    //                    Tile surroundingForest = conn.goingTo.GetComponent<Tile>();

    //                    foreach (ScriptConnection goingToForest in surroundingForest.Connections)
    //                    {
    //                        if (goingToForest.goingTo == tileScript.gameObject)
    //                        {
    //                            goingToForest.cost = terrainCost;
    //                            break;
    //                        }
    //                    }
    //                }

    //            }
    //        }
    //    }
    //}
}
