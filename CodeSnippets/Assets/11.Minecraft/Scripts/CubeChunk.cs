using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeChunk : MonoBehaviour 
{
	/// <summary>
	/// Properties size cube chunk
	/// </summary>
	public const int Width = 16;
	public const int Length = 16;
	// Max height of each chuck is 64 cubes
	public const int Height = 64;

	private byte[] cubeChunkInfo;
	private Mesh   meshChunk;
	public Mesh MeshChunk
	{
		set { this.meshChunk = value; }
		get { return this.meshChunk; }
	}

	private Material materialChunk;
	public Material MaterialChunk
	{
		set { this.materialChunk = value; }
		get { return this.materialChunk; }
	}

	private Vector2 positionChunk = Vector2.zero;
	public Vector2 PositionChunk
	{
		set { this.positionChunk = value; }
		get { return this.positionChunk; }
	}

	void Start () 
	{
		// Data info for a cube chunk
		this.cubeChunkInfo = new byte[Width * Length * Height];

		// Create cube chunk, and sets the info for uvs
		for( int x=0; x<Width; ++x)
		{
			for( int z=0; z<Length; ++z)
			{
				// Generate heights pseudo-random through perlin noise
				float heightPerlin = Mathf.PerlinNoise( positionChunk.x + x / 16.0f, positionChunk.y + z / 16.0f) * 16;
				Debug.Log("heightPerlin: " + heightPerlin);
				for( int y=0; y<Height; ++y)
				{
					// Gets index from a 3D space into 1D space
					int indexInfo = x + y * (Width * Length) + z *  Length;
					byte value =(byte)0;
					// Check heigh, every height under semi random heightPerlin is drawn
					if (y < heightPerlin)
					{
						value =(byte)2;
						//value =(byte)1;
					}
					this.cubeChunkInfo[indexInfo] = value;
				}
			}
		}

		setupBoxChunk();
		// Add mesh renderer and mesh colider
		// MeshFilter allows to generate the mesh of the game object in procedural way
		this.gameObject.AddComponent<MeshFilter>().mesh = this.meshChunk;
		this.gameObject.AddComponent<MeshRenderer>().material = this.materialChunk;
		this.gameObject.AddComponent<MeshCollider>().sharedMesh = this.meshChunk;
		this.transform.position = new Vector3(this.positionChunk.x * Width, 0, this.positionChunk.y * Length);
	}

	/// <summary>
	/// Get the info
	/// </summary>
	/// <returns>The face.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	private byte getInfo( int x, int y, int z )
	{
		// Check boundaries
		if( x < 0 || x >= Width || y < 0 || y>=Height || z < 0 || z>=Length ) return 0;
		// Returns chunk info
		int indexInfo = x + y * (Width * Length) + z *  Length;
		return cubeChunkInfo[indexInfo];
	}

	/// <summary>
	/// Assign the uvs from info
	/// </summary>
	/// <param name="listUvs">List uvs.</param>
	/// <param name="currentInfo">Current info.</param>
	private void addUVs(List<Vector2> listUvs, byte currentInfo)
	{
		int xCenter = currentInfo%16;
		int yCenter = currentInfo/16;
		float size = 1.0f / 16.0f;
		
		float min = size*0.5f;
		float max = 1.0f - min;
		
		listUvs.Add( new Vector2( (xCenter+max) * size, 1.0f- (yCenter+max) * size ) );
		listUvs.Add( new Vector2( (xCenter+min) * size, 1.0f- (yCenter+max) * size ) );
		listUvs.Add( new Vector2( (xCenter+min) * size, 1.0f- (yCenter+min) * size ) );
		listUvs.Add( new Vector2( (xCenter+max) * size, 1.0f- (yCenter+min) * size ) );
	}

	/// <summary>
	/// Updates the indices.
	/// </summary>
	/// <param name="lIndex">Current list indices.</param>
	/// <param name="id">Current index.</param>
	private void updateIndices(List<int> lIndex, int id)
	{
		if (lIndex !=null)
		{
			lIndex.Add(id);
			lIndex.Add(id+1);
			lIndex.Add(id+3);
			lIndex.Add(id+2);
			lIndex.Add(id+3);
			lIndex.Add(id+1);
		}
	}
	
	/// <summary>
	/// Sets the visual elements of the cube chunk
	/// </summary>
	private void setupBoxChunk()
	{
		// List of vertex
		List<Vector3> listVertices = new List<Vector3>();
		// List uvs
		List<Vector2> listUvs = new List<Vector2>();
		// List index of the chunk
		List<int> 	  listIndex = new List<int>();

		// Mesh
		this.meshChunk = new Mesh();

		int auxIndex = 0;
		for( int y=0; y<Height; ++y)
		{
			for( int x=0; x<Width; ++x)
			{
				for( int z=0; z<Length; ++z)
				{
					// Gets current info
					byte currentInfo = getInfo(x,y,z);
					if (currentInfo != 0)
					{
						// Generate left face
						if (getInfo(x-1,y,z) == 0)
						{
							listVertices.Add( new Vector3( x-0.5f, y+0.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+0.0f, z+0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+1.0f, z+0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+1.0f, z-0.5f ) );

							// Generate the uvs
							addUVs(listUvs,currentInfo);
							// index for triangles
							updateIndices(listIndex,auxIndex);
							auxIndex +=4;
						}
						// Rigth face
						if (getInfo(x+1,y,z) == 0)
						{
							listVertices.Add( new Vector3( x+0.5f, y+0.0f, z+0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+0.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+1.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+1.0f, z+0.5f ) );
							
							// Generate the uvs
							addUVs(listUvs,currentInfo);
							// index for triangles
							updateIndices(listIndex,auxIndex);
							auxIndex +=4;
						}

						// Top face
						if (getInfo(x,y-1,z) == 0)
						{
							listVertices.Add( new Vector3( x-0.5f, y+0.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+0.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+0.0f, z+0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+0.0f, z+0.5f ) );							
							// Generate the uvs
							addUVs(listUvs,currentInfo);
							// index for triangles
							updateIndices(listIndex,auxIndex);
							auxIndex +=4;
						}

						// Bottom face
						if (getInfo(x,y+1,z) == 0)
						{
							listVertices.Add( new Vector3( x+0.5f, y+1.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+1.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+1.0f, z+0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+1.0f, z+0.5f ) );							
							// Generate the uvs
							addUVs(listUvs,currentInfo);
							// index for triangles
							updateIndices(listIndex,auxIndex);
							auxIndex +=4;
						}

						// Front face
						if (getInfo(x,y,z-1) == 0)
						{
							listVertices.Add( new Vector3( x+0.5f, y+0.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+0.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+1.0f, z-0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+1.0f, z-0.5f ) );					
							// Generate the uvs
							addUVs(listUvs,currentInfo);
							// index for triangles
							updateIndices(listIndex,auxIndex);
							auxIndex +=4;
						}

						// Back face
						if (getInfo(x,y, z+1 ) == 0)
						{
							listVertices.Add( new Vector3( x-0.5f, y+0.0f, z+0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+0.0f, z+0.5f ) );
							listVertices.Add( new Vector3( x+0.5f, y+1.0f, z+0.5f ) );
							listVertices.Add( new Vector3( x-0.5f, y+1.0f, z+0.5f ) );						
							// Generate the uvs
							addUVs(listUvs,currentInfo);
							// index for triangles
							updateIndices(listIndex,auxIndex);
							auxIndex +=4;
						}
					}
				}
			}
		}

		// Assign elements and normals
		this.meshChunk.vertices = listVertices.ToArray();
		this.meshChunk.uv = listUvs.ToArray();
		this.meshChunk.triangles = listIndex.ToArray();
		this.meshChunk.RecalculateNormals();
	}

	void Update () {}
}
