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
    public GameObject loadingPanel;
    [Header("Object Prefabs")]
    public GameObject mechPrefab;
    public GameObject basicEnemyPrefab;

    GameObject collisionParent;
    private Sprite[] spriteTiles;
    private List<GameObject> interactables = new List<GameObject>();

    //Sprite Index for objects
    private const int PLAYERSPAWN = 964;
    private const int WEED_ONE = 592;
    private const int WEED_TWO = 649;
    private const int MECHSPAWN_ONE = 56;
    private const int MECHSPAWN_TWO = 113;
    private const int ENEMYBASIC = 1352;

    private void Awake()
    {
        loadingPanel.SetActive(true);
    }
    private void Start()
    {
        collisionParent = new GameObject("CollisionGridLayer");
        LoadLevel();
    }

    public void LoadLevel()
    {
        StartCoroutine("LoadMap");
    }

    IEnumerator LoadMap()
    {
        yield return new WaitForEndOfFrame();
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
                    tempSprite.isStatic = true;

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
                                GameObject.FindGameObjectWithTag("Player").transform.position = tempSprite.transform.position;
                                Destroy(tempSprite);
                                break;
                            case WEED_ONE:
                            case WEED_TWO:
                                BoxCollider2D boxCol = tempSprite.AddComponent<BoxCollider2D>();
                                boxCol.isTrigger = true;
                                tempSprite.tag = "weed";
                                break;
                            case MECHSPAWN_ONE://Needed two sprites to fit mech so just delete the extra one.
                                Destroy(tempSprite);
                                break;
                            case MECHSPAWN_TWO://Spawns mech on this sprite
                                GameObject tempMech = Instantiate(mechPrefab, tempSprite.transform.position, Quaternion.identity);
                                interactables.Add(tempMech);
                                Destroy(tempSprite);
                                break;
                            case ENEMYBASIC:
                                Instantiate(basicEnemyPrefab, tempSprite.transform.position, Quaternion.identity);
                                Destroy(tempSprite);
                                break;
                            default:
                                BoxCollider2D defaultBoxCol = tempSprite.AddComponent<BoxCollider2D>();
                                break;

                        }
                    }
                    
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

        loadingPanel.SetActive(false);
        GameObject.FindObjectOfType<PlayerInteraction>().SetInteractables(interactables);

        yield break;
    }
}
