using UnityEngine;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour
{
    // List of sprites to manage
    public List<Sprite> spriteList;

    // Dictionary to store sprites with their keys
    private Dictionary<string, Sprite> rocketSprites = new Dictionary<string, Sprite>();

    private void Start()
    {
        // Populate the dictionary with sprites from the list
        foreach (Sprite sprite in spriteList)
        {
            if (sprite == null) continue; // Skip null sprites

            // Generate the key by removing "_0" from the sprite name
            string key = sprite.name.Replace("_0", "");

            // Add the sprite to the dictionary if the key is valid
            if (!string.IsNullOrEmpty(key))
            {
                rocketSprites[key] = sprite;
            }
        }
    }

    // Method to get a sprite based on design and color
    public Sprite GetRocketSprite(string design, string color)
    {
        // Generate the key using design and color
        string key = $"{design}_{color}";

        // Return the sprite if it exists in the dictionary
        if (rocketSprites.TryGetValue(key, out Sprite sprite))
        {
            return sprite;
        }

        // Log a warning if the sprite is not found
        Debug.LogWarning($"Sprite not found for key: {key}");
        return null;
    }

    // Method to get a random sprite from the list
    public Sprite GetRandomSprite()
    {
        if (spriteList == null || spriteList.Count == 0)
        {
            Debug.LogWarning("Sprite list is empty or not assigned.");
            return null;
        }

        // Return a random sprite from the list
        return spriteList[Random.Range(0, spriteList.Count)];
    }
}