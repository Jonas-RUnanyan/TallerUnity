
using UnityEngine;
using System.Collections;

public static class MeshGenerator {

	/// <summary>
	/// Método que genera los datos del mesh dado un mapa de altura y otros parámetros para personalizarlo
	/// </summary>
	/// <param name="heightMap">Mapa de altura expresado como un array bidimensional</param>
	/// <param name="heightMultiplier">Multiplicador de altura, determina cuánto se "exagera" la altura en el mesh</param>
	/// <param name="heightCurve">Curva de animación, mapea los valores de altura en determinadas alturas reales en el mesh</param>
	/// <returns></returns>
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve) {
   		int width = heightMap.GetLength (0);
		int height = heightMap.GetLength (1);

   		//Coordenada inicial para recorrer la matriz de coordenadas (La esquina superior izquierda del mapa) 
	    float topLeftX = (width - 1) / -2f;
	    float topLeftZ = (height - 1) / 2f;
		
		//Inicializamos un meshData para guardar la información del Mesh que queremos aplicar
		MeshData meshData = new MeshData(width, height);
		
		//Variable para llevar la cuenta del vértice por el que vamos
		int vertexIndex = 0;
		
		//Bucle anidado para recorrer las coordenadas, asignarles un vértice y añadir los triangulos que formen al mesh
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				meshData.vertices[vertexIndex] = new Vector3(topLeftX + x,
					heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);

				meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1)
				{
					meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
					meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
				}

				vertexIndex++;
			}
		}

		//devolvemos el MeshData
		return meshData;

	}
}

/// <summary>
/// Struct que contiene los datos de un Mesh
/// </summary>
public class MeshData {
	public Vector3[] vertices;		// Vértices del mesh
	public int[] triangles;			// Triángulos del mesh
	public Vector2[] uvs;			// Los UVs son vectores bidimensionales que se usan para mapear su textura
									// en cada una de los triángulos de un mesh

	int triangleIndex;				// Usaremos esta variable para llevar la cuenta del número de vértices del mesh

	/// <summary>
	/// Constructor de MeshData, devuelve los datos de un mesh de ancho meshWidth y largo meshHeight
	/// </summary>
	/// <param name="meshWidth">Anchura del mesh (medida en número de vértices)</param>
	/// <param name="meshHeight">Altura del mesh (medida en número de vértices)</param>
	public MeshData(int meshWidth, int meshHeight) {
		//Inicializamos los arrays que contendrán los datos del mesh
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
	}

	/// <summary>
	/// Define el siguiente triángulo en MeshData a partir de sus vértices
	/// </summary>
	/// <param name="a">Vértice 1</param>
	/// <param name="b">Vértice 2</param>
	/// <param name="c">Vértice 3</param>
	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	/// <summary>
	/// Crea el Mesh correspondiente a nuestros datos del mapa
	/// </summary>
	/// <returns>El mesh correspondiente a los datos guardados</returns>
	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();		// Inicializamos el mesh
		mesh.vertices = vertices;		// Asignamos los datos guardados a su componente correspondiente
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();		// Recalculamos los vectores normales a la superficie despúes de los cambios hechos.
										// Esto se hace para que la iluminación y las sombras del mesh sean correctas
		return mesh;
	}

}
