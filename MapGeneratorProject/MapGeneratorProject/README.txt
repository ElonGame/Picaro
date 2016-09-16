HOW TO ADD A TILE SET:
1) Add .txt file in MapGeneratorProjectContent\Maps\TileSets in the following format:

	START
	TILE_SET_NAME							(single entry)
	(name of tile set)
	TEXTURE_FILE_NAME						(single entry)
	(name of texture file used in TileSet)
	BG_SOURCES								(must be in x, y pairs)
	(x pixel coordinate of tile)
	(y pixel coordinate of tile)
	BG_ANIMATED_TILES
	(index in BG_SOURCES of first frame of animated tile)
	(frame count of animated tile)
	(milliseconds between frame updates)
	FG_SOURCES
	(x pixel coordinate of tile)
	(y pixel coordinate of tile)
	FG_ANIMATED_TILES
	(index in BG_SOURCES of first frame of animated tile)
	(frame count of animated tile)
	(milliseconds between frame updates)
	STOP

2) Set "Build Action" under properties to "none", and "Copy if Newer"

3) Add texture file name used in TileSet .txt file under TEXTURE_FILE_NAME to Dictionary that links texture names to texture filepaths:

	In LibraryProject\Map\TextureManager, add new to TextureManager.textureNameToPathDictionary 
	in function "static private void LoadTextureNameDictionary()" to create an association between a texture name its filepath:
	e.g. textureNameToPathDictionary.Add("testTileSet", textureContentManager.Load<Texture2D>(@"MapTextures\\testTileSet"));

4) Add filepath of TileSet .txt file to List<string> that contains all TileSet .txt filepaths:
	
	In LibraryProject\Map\TileSetManager, add filepath of new .txt file to tileSetsNames in 
	function TileSetManager.Initialize():
	e.g. tileSetsNames.Add(@"Content\\Maps\\TileSets\\testTileSet.txt");

5) Add enum identifier that points to the index of TileSetManager.tileSetNames the .txt filepath was added as:

	In LibraryProject\EnumDefinitions, add the identifier in TileSetsNamesIndices at the index that matches the 
	index the .txt filepath was added to TileSetManager.tileSetNames in step 4:
	e.g. TileSetsNamesIndices { TEST_TILE_SET }

HOW TO USE A TILE SET IN PROJECT

1) Use MapGenerator.MakeMap()