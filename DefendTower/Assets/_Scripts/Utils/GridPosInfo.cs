using UnityEngine;
public class GridPosInfo {
    public Vector3Int CurrentHoveredTile { get; private set; }
    public Vector3 CurrentHoveredTilePos { get; private set; }

    public GridPosInfo(Vector3 currentHoveredTilePos, Vector3Int currentHoveredTile) {
        CurrentHoveredTilePos = currentHoveredTilePos;
        CurrentHoveredTile = currentHoveredTile;
    }
    public GridPosInfo() {
        CurrentHoveredTilePos = new Vector3(999, 999);
        CurrentHoveredTile = new Vector3Int(999, 999);
    }
}
