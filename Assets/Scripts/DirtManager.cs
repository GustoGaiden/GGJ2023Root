using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class DirtManager : MonoBehaviour
{
    // Dirt Manager populates the game with tiers of dirt
    // There are tiers of Dirt : progressively more resistent material, that costs more and more juice to travel through.
    // There are tiers of Gravel : areas of additional difficulty, rendered as blobby circles.
    // Each Dirt Tier contains Caverns : Caverns increase the player's drilling ability, once connected to the Vine Network

    public BoxCollider2D GameWorldBounds;

    public GameObject DirtContainer;

    public GameObject GravelContainer;

    public GameObject CavernContainer;

    private List<DirtTier> dirtTiers = new List<DirtTier>();

    private List<GameObject> gravelBlobs = new List<GameObject>();

    private List<GameObject> caverns = new List<GameObject>();

    public List<float> DirtTiersJuicePerSecond = new List<float> {0.1f, 1.0f, 5.0f, 15.0f};
    public List<float> GravelTierMultipliers = new List<float> {1.1f, 1.5f, 3.0f};

    public DirtTier DirtPrefab;
    
    public List<float> gravelTierModifiers;
    private int numCavernsPerTier = 2;
    
    private Vector2 tierSize;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void regenerateDirt()
    {
        // Destroy existing Tiers
        foreach (DirtTier obj in dirtTiers)
        {
            obj.transform.SetParent(null);
            Destroy(obj.gameObject);            
        }
        dirtTiers.RemoveRange(0, dirtTiers.Count);

        // Start generating new tiers
        for (int i = 0; i < DirtTiersJuicePerSecond.Count; i++)
        {
            DirtTier NewDirtTier = Instantiate(DirtPrefab);
            NewDirtTier.TierLevel = i;
            NewDirtTier.JuiceCostPerSecond = DirtTiersJuicePerSecond[i];
            NewDirtTier.Collider.size = tierSize;
            switch (i)
            {
                case 0:
                    NewDirtTier.sprite.color = new Color(1,1,1,0.3f);
                    break;
                case 1:
                    NewDirtTier.sprite.color = new Color(1,0,1,0.3f);
                    break;
                case 2:
                    NewDirtTier.sprite.color = new Color(0,1,1,0.3f);
                    break;
                case 3:
                    NewDirtTier.sprite.color = new Color(0,0,1,0.3f);;
                    break;
                default:
                    NewDirtTier.sprite.color = new Color(1,0,0,0.3f);;
                    break;
            }

            NewDirtTier.sprite.size = tierSize;
            
            NewDirtTier.transform.SetParent(DirtContainer.transform);
            NewDirtTier.transform.position = new Vector3(0f, tierSize.y * -i - (tierSize.y/2) , 0f);
            dirtTiers.Add(NewDirtTier);
        }
        
    }
    
    public void RegenerateWorld()
    {
        tierSize = new Vector2(GameWorldBounds.size.x, GameWorldBounds.size.y / 4);
        regenerateDirt();
    }

    private float GetCurrentDirtCostPerSecond()
    {
        // Start with lowest tier value.
        float highestValue = dirtTiers[0].JuiceCostPerSecond;
        
        foreach (DirtTier tier in dirtTiers)
        {
            if (tier.isPlayerTouchingThisTier())
            {
                highestValue = Mathf.Max(highestValue, tier.JuiceCostPerSecond);
            }
        }
        return highestValue;
    }

    private float GetCurrentGravelCostModifier()
    {
        // TO DO : ADD GRAVEL
        return 1f;
    }

    public float GetCurrentJuiceCostPerSecond()
    {
        float dirtCost = GetCurrentDirtCostPerSecond();
        float gravelMod = GetCurrentGravelCostModifier();
        return dirtCost * gravelMod;
    }
}
