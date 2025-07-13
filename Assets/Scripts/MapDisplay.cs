using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

    public Renderer textureRender;			// Referencia al renderer del plano en el que se dibujará la textura	
    public MeshFilter meshFilter;			// Referencias a componentes del mesh
    public MeshRenderer meshRenderer;
    public MeshCollider meshCollider;

    /// <summary>
    /// Aplica la textura al mapa 2D dada al plano a través de su renderer
    /// </summary>
    /// <param name="texture">La textura a aplicar</param>
    public void DrawTexture(Texture2D texture) {
        //Ocultamos el mesh y activamos el plano
        meshRenderer.gameObject.SetActive(false);
        textureRender.gameObject.SetActive(true);
        //Aplica la textura al material del plano (colores)
        textureRender.sharedMaterial.mainTexture = texture;
        //Ajusta la escala de la textura para que no esté distorsionada
        textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
    }

    /// <summary>
    /// Método que asigna los datos de meshData a meshFilter y aplica una textura para el mapa 3D
    /// </summary>
    /// <param name="meshData">Struct que contiene la información relevante del mesh</param>
    /// <param name="texture">Textura a aplicar</param>
    public void DrawMesh(MeshData meshData, Texture2D texture) {
        //Ocultamos el plano y activamos el mesh
        textureRender.gameObject.SetActive(false);
        meshRenderer.gameObject.SetActive(true);
        
        meshFilter.sharedMesh = meshData.CreateMesh ();		// Aplicamos el mesh definidio por los triángulos
        meshCollider.sharedMesh = meshFilter.sharedMesh;    // Actualizamos el detector de colisiones del mesh para que se comporte como debería
        meshRenderer.sharedMaterial.mainTexture = texture;	// Aplicamos la textura (colores)
    }

}   