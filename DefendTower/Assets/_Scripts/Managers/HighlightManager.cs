using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightManager : MonoBehaviour {
    [SerializeField] private Tilemap _highlightTilemap;
    [SerializeField] private Tilemap _placeableTilemap;
    [SerializeField] private Tile _highlightTile;
    [SerializeField] private GameObject _towerRadius;

    public Vector3Int CurrentHoveredTile { get; private set; }
    private Vector3Int _oldHoveredTile;
    private GameObject _currentRadius;

    private Camera _cam;
    private void Start() {
        _cam = Camera.main;
    }

    public GridPosInfo CheckCurrentMouseHover(Vector3 pos) {
        var towerPos = Camera.main.ScreenToWorldPoint(pos);
        CurrentHoveredTile = _highlightTilemap.WorldToCell(towerPos);
        if(_placeableTilemap.GetTile(CurrentHoveredTile) == null) {
            _highlightTilemap.SetTile(_oldHoveredTile, null);
            return null;
        }
        if (_currentRadius == null) {
            _currentRadius = Instantiate(_towerRadius);
        }
        _highlightTilemap.SetTile(_oldHoveredTile, null);
        _highlightTilemap.SetTile(CurrentHoveredTile, _highlightTile);
        _oldHoveredTile = CurrentHoveredTile;
        var cellCenter = _placeableTilemap.CellToWorld(CurrentHoveredTile) + new Vector3(0.5f, 0.9f);
        _currentRadius.transform.position = cellCenter;
        return new GridPosInfo(cellCenter, CurrentHoveredTile);
    }

    public void DeleteLastHighlight() {
        _highlightTilemap.SetTile(_oldHoveredTile, null);
        Destroy(_currentRadius);
    }
}
