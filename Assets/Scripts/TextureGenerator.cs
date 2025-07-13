using UnityEngine;
using System.Collections;

public static class TextureGenerator {
    /// <summary>
    /// Dada una matriz como la generada por Noise.GenerateNoisemap crea una textura en escala de grises
    /// </summary>
    /// <param name="heightMap">La matriz de ruido</param>
    /// <returns></returns>
    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colorMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        //Le pasamos la matriz de escala de grises a TextureFromColorMap para que cree la textura a partir de ella
        return TextureFromColourMap(colorMap, width, height);
    }

    /// <summary>
    /// Dada un array unidimensional de colores y las dimensiones del mapa definido por este, devuelve la textura definida por este array
    /// </summary>
    /// <param name="colourMap">El array de colores a partir del cual se creará la textura</param>
    /// <param name="width">Anchura del mapa</param>
    /// <param name="height">Altura mapa</param>
    /// <returns></returns>
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height) {
        Texture2D texture = new Texture2D (width, height);	// Creamos una textura
        texture.filterMode = FilterMode.Point;				// Con esto los píxeles quedarán definidos y no con los bordes borrosos
        texture.wrapMode = TextureWrapMode.Clamp;			// Con esto Unity interpreta que la textura no se va a loopear
        texture.SetPixels (colourMap);						// Aplicamos a la textura los colores de nuestro array
        texture.Apply ();
        return texture;
    }

}