
using UnityEngine;
using System.Collections;

public static class Noise {
/// <summary>
/// Función que genera un array con los valores del ruido para cada coordenada
/// </summary>
/// <param name="mapWidth">Longitud del mapa</param>
/// <param name="mapHeight">Altura del mapa</param>
/// <param name="seed">Semilla del mapa para la generación aleatoria (como en Minecraft)</param>
/// <param name="scale">Escala del ruido</param>
/// <param name="octaves">Número de octavas del ruido (más octavas = más detalle)</param>
/// <param name="persistance">Disminución de la amplitud en octavas sucesivas</param>
/// <param name="lacunarity">Aumento de la frecuencia en octavas sucesivas</param>
/// <param name="offset">Desplazamiento del ruido</param>
/// <returns></returns>
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
		 
		//Creamos la matriz en el que almacenaremos el ruido
		float[,] noiseMap = new float[mapWidth, mapHeight];
		
		//Declaramos un generador de números aleatorios
		System.Random prng = new System.Random (seed);						
		
		//Añadimos un offset a cada una de las octavas para aportar variedad
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++) {
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) + offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}
		
		// Nos aseguramos de que scale es mayor que 0 para luego evitar divisiones entre 0
		if (scale <= 0) {
			scale = 0.0001f;
		}
		
		//Valores máximo y mínimo entre los que tendrán que estar acotados todos los valores del ruido
		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		//Variables para centrar el mapa de ruido (coordenadas centrales), permitiendo que el offset mueva el centro del mapa
		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		//Dos bucles anidados para recorrer la matriz de ruido
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++)
				{
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;

					if (noiseHeight > maxNoiseHeight)
					{
						maxNoiseHeight = noiseHeight;
					} else if (noiseHeight < minNoiseHeight)
					{
						minNoiseHeight = noiseHeight;
					}
					noiseMap[x,y] = noiseHeight;

				}

			}
			
		}


    
		//Con estos dos bucles nos aseguramos de que el valor devuelto esté entre 0 y 1
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
			
		}
		

		
		//Devolver el mapa de ruido
		return noiseMap;
	}

}
