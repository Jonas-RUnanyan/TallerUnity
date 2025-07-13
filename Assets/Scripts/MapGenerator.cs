using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour {

	//Definimos una variable de tipo enumerable para seleccionar el modo del programa
	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;		// Declaramos la variable drawMode del tipo que acabamos de difinir

	public int mapChunkSize = 241;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]	// Con esto capamos la persistencia para que solo pueda tomar valores entre 0 y 1, y hacemos que en el inspector de Unity aparezca como un slider
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;		// Booleano para decidir si el mapa se actualiza automáticamente o solo cuando pulsemos el botón de generar

	public TerrainType[] regions;	// Array con las distintas regiones del terreno

	public void GenerateMap() {

    //Crear un array para almacenar los valores de ruido para el mapa
    float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves,
	    persistance, lacunarity, offset);

		//Inicializamos un array que guardará los colores de cada una de las coordenadas de nuestro mapa
		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		
		//Bucles anidados para recorrer a la vez la matriz de ruido y la de color e ir asignando colores a cada coordenada según la región a la que pertenezca
		for (int y = 0; y < mapChunkSize; y++)
		{
			for (int x = 0; x < mapChunkSize; x++)
			{
				float currentHeight = noiseMap[x, y];

				for (int i = 0; i < regions.Length; i++)
				{
					if (currentHeight <= regions[i].height)
					{
						colourMap[y * mapChunkSize + x] = regions[i].colour;
						break;
					}
				}
			}
		}
		
		//Obtenemos una referencia a MapDisplay, que a su vez contiene referencias a los componentes del renderer del
		//plano y del mesh
		MapDisplay display = FindObjectOfType<MapDisplay> ();

		//Comprobamos el modo seleccionado de visualización en la interfaz y ejecutamos la visualización correspondiente
		if (drawMode == DrawMode.NoiseMap) {
      //Añadimos la textura en escala de grises usando MapDisplay
			display.DrawTexture((TextureGenerator.TextureFromHeightMap(noiseMap)));
		} else if (drawMode == DrawMode.ColourMap) {
      //Añadimos la textura a color usando MapDisplay
			display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
		}
	}

	//La función OnValidate es llamada cada vez que se cambia un valor en el inspector de Unity.
	//Es muy útil para mantener determinados valores dentro de un cierto rango
	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}
}

/// <summary>
/// Este struct guarda la información relacionada con cada tipo de región que podemos encontrar en el mapa según su altura
/// </summary>
[System.Serializable]	// Ponemos esto para que el struct pueda salir y ser modificado en el inspector
public struct TerrainType
{
	public string name; // Nombre de la región

	public float height; // Altura máxima de la región
	public Color colour; // Color de la región
}