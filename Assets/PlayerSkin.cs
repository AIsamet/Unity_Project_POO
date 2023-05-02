using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public Texture2D newTexture;

    public void ChangeTexture()
    {
        SkinnedMeshRenderer prefabRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        Material[] prefabMaterials = prefabRenderer.materials;

        for (int i = 0; i < prefabMaterials.Length; i++)
        {
            prefabMaterials[i].mainTexture = newTexture;
        }

        prefabRenderer.materials = prefabMaterials;
    }
}
