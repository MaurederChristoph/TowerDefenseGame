using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// Manages the Tilemaps
/// </summary>
public class HighlightManager : MonoBehaviour {
    /// <summary>
    /// The tilmap that shows the highlights
    /// </summary>
    [SerializeField] private Tilemap _highlightTilemap;
    /// <summary>
    /// The tilemp that represents the placable area
    /// </summary>
    [SerializeField] private Tilemap _placeableTilemap;
    /// <summary>
    /// The tile that is used for highlighting
    /// </summary>
    [SerializeField] private Tile _highlightTile;
    /// <summary>
    /// Shows the radius of the tower
    /// </summary>
    [SerializeField] private GameObject _towerRadius;
    /// <summary>
    /// The tile that is currently hovered over
    /// </summary>
    public Vector3Int CurrentHoveredTile { get; private set; }
    /// <summary>
    /// The old hovered tile
    /// </summary>
    private Vector3Int _oldHoveredTile;
    /// <summary>
    /// The Current instance of the radius
    /// </summary>
    private GameObject _currentRadius;

    /// <summary>
    /// Checks the current mouse hover and highlights the tile and shows the radius if the tower
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public GridPosInfo CheckCurrentMouseHover(Vector3 pos) {
        var towerPos = Camera.main.ScreenToWorldPoint(pos);
        CurrentHoveredTile = _highlightTilemap.WorldToCell(towerPos);
        if(_placeableTilemap.GetTile(CurrentHoveredTile) == null) {
            _highlightTilemap.SetTile(_oldHoveredTile, null);
            return null;
        }
        if(_currentRadius == null) {
            _currentRadius = Instantiate(_towerRadius);
        }
        _highlightTilemap.SetTile(_oldHoveredTile, null);
        _highlightTilemap.SetTile(CurrentHoveredTile, _highlightTile);
        _oldHoveredTile = CurrentHoveredTile;
        var cellCenter = _placeableTilemap.CellToWorld(CurrentHoveredTile) + new Vector3(0.5f, 0.9f);
        _currentRadius.transform.position = cellCenter;
        return new GridPosInfo(cellCenter, CurrentHoveredTile);
    }

    /// <summary>
    /// Deletes tha last highlighted tile
    /// </summary>
    public void DeleteLastHighlight() {
        _highlightTilemap.SetTile(_oldHoveredTile, null);
        Destroy(_currentRadius);
    }
}
