import React, { useState, useEffect } from 'react';
import { MapContainer, TileLayer, Marker, Popup, Polygon, useMapEvents } from 'react-leaflet';
import { RefreshCw, Download, MapPin, Plus, X, Layers } from 'lucide-react';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';

// Fix Leaflet icon issue
delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png',
  iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
});

const availableIcon = new L.Icon({
  iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41]
});

const collectedIcon = new L.Icon({
  iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-grey.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  shadowSize: [41, 41]
});

// Component for drawing zones
function DrawZone({ onComplete, isDrawing }) {
  const [points, setPoints] = useState([]);

  useMapEvents({
    click(e) {
      if (isDrawing) {
        const newPoint = [e.latlng.lat, e.latlng.lng];
        setPoints([...points, newPoint]);
      }
    },
  });

  useEffect(() => {
    if (!isDrawing && points.length > 0) {
      onComplete(points);
      setPoints([]);
    }
  }, [isDrawing, points, onComplete]);

  if (points.length < 2) return null;

  return (
      <Polygon
          positions={points}
          pathOptions={{ color: 'yellow', fillColor: 'yellow', fillOpacity: 0.2 }}
      />
  );
}

const API_BASE_URL = 'http://localhost:5008/play-world';

export default function PlayWorld() {
  const [items, setItems] = useState([]);
  const [zones, setZones] = useState([]);
  const [loading, setLoading] = useState(false);
  const [geoJson, setGeoJson] = useState(null);
  const [showCreateZoneModal, setShowCreateZoneModal] = useState(false);
  const [isDrawingZone, setIsDrawingZone] = useState(false);
  const [drawnPoints, setDrawnPoints] = useState([]);
  const [createZoneLoading, setCreateZoneLoading] = useState(false);
  const [createZoneError, setCreateZoneError] = useState(null);
  const [selectedZone, setSelectedZone] = useState(null);
  const [zoneItemsLoading, setZoneItemsLoading] = useState(false);
  const [zoneItems, setZoneItems] = useState([]);
  const [zoneFormData, setZoneFormData] = useState({
    name: '',
    type: 'Forest'
  });

  const fetchMapData = async () => {
    setLoading(true);
    try {
      const response = await fetch(`${API_BASE_URL}/itemLocations`);
      const data = await response.json();

      const itemsData = data.itemLocations || data.items || data || [];
      setItems(itemsData);

      await fetchZones();
    } catch (err) {
      console.error('Error fetching map data:', err);
    } finally {
      setLoading(false);
    }
  };

  const fetchZones = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/zones`);
      if (response.ok) {
        const data = await response.json();
        setZones(data.zones || data || []);
      }
    } catch (err) {
      console.error('Error fetching zones:', err);
    }
  };

  const fetchItemsInZone = async (zoneName) => {
    setZoneItemsLoading(true);
    try {
      const response = await fetch(`${API_BASE_URL}/zones/${encodeURIComponent(zoneName)}/items`);
      if (response.ok) {
        const data = await response.json();
        setZoneItems(data.items || data || []);
      }
    } catch (err) {
      console.error('Error fetching zone items:', err);
    } finally {
      setZoneItemsLoading(false);
    }
  };

  useEffect(() => {
    fetchMapData();
  }, []);

  useEffect(() => {
    if (items.length > 0 || zones.length > 0) {
      const geojson = {
        type: 'FeatureCollection',
        features: [
          ...items.map(item => ({
            type: 'Feature',
            geometry: {
              type: 'Point',
              coordinates: [item.position.longitude, item.position.latitude]
            },
            properties: {
              type: 'item',
              id: item.itemId,
              name: item.itemName,
              isCollected: item.isCollected,
              droppedAt: item.droppedAt
            }
          })),
          ...zones.map(zone => ({
            type: 'Feature',
            geometry: {
              type: 'Polygon',
              coordinates: [zone.boundary.points.map(p => [p.longitude, p.latitude])]
            },
            properties: {
              type: 'zone',
              name: zone.name,
              zoneType: zone.type
            }
          }))
        ]
      };
      setGeoJson(geojson);
    }
  }, [items, zones]);

  const downloadGeoJSON = () => {
    const blob = new Blob([JSON.stringify(geoJson, null, 2)], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'play-world-map.geojson';
    a.click();
    URL.revokeObjectURL(url);
  };

  const startDrawingZone = () => {
    setIsDrawingZone(true);
    setDrawnPoints([]);
    setZoneFormData({ name: '', type: 'Forest' });
    setCreateZoneError(null);
  };

  const handleDrawingComplete = (points) => {
    setDrawnPoints(points);
  };

  const handleCreateZone = async () => {
    if (drawnPoints.length < 3) {
      setCreateZoneError('Zone must have at least 3 points');
      return;
    }

    setCreateZoneLoading(true);
    setCreateZoneError(null);

    try {
      const boundary = {
        points: drawnPoints.map(p => ({
          latitude: p[0],
          longitude: p[1]
        }))
      };

      const response = await fetch(`${API_BASE_URL}/zones`, {
        method: 'POST',
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: zoneFormData.name,
          type: zoneFormData.type,
          boundary: boundary
        })
      });

      if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      }

      setZoneFormData({ name: '', type: 'Forest' });
      setDrawnPoints([]);
      setShowCreateZoneModal(false);
      setIsDrawingZone(false);

      await fetchZones();
    } catch (err) {
      console.error('Create zone error:', err);
      setCreateZoneError(err.message);
    } finally {
      setCreateZoneLoading(false);
    }
  };

  const cancelDrawing = () => {
    setIsDrawingZone(false);
    setShowCreateZoneModal(false);
    setDrawnPoints([]);
    setZoneFormData({ name: '', type: 'Forest' });
    setCreateZoneError(null);
  };

  const getZoneColor = (zoneType) => {
    const colors = {
      Forest: '#22c55e',
      Desert: '#fbbf24',
      City: '#3b82f6',
      Mountain: '#8b5cf6',
      Water: '#06b6d4',
      Dungeon: '#ef4444'
    };
    return colors[zoneType] || '#6b7280';
  };

  return (
      <div>
        {/* Controls */}
        <div className="bg-gray-900 rounded-lg p-6 mb-6 border border-gray-800">
          <div className="flex justify-between items-center">
            <div className="text-sm text-gray-400">
              Items: <span className="text-white font-semibold">{items.length}</span>
              <span className="ml-4">Zones: <span className="text-white font-semibold">{zones.length}</span></span>
            </div>
            <div className="flex gap-3">
              <button
                  onClick={startDrawingZone}
                  className="flex items-center gap-2 px-4 py-2 bg-purple-600 hover:bg-purple-700 text-white rounded-lg transition-colors"
              >
                <Plus className="w-4 h-4" />
                Create Zone
              </button>
              <button
                  onClick={downloadGeoJSON}
                  disabled={!geoJson}
                  className="flex items-center gap-2 px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded-lg transition-colors disabled:opacity-50"
              >
                <Download className="w-4 h-4" />
                Export GeoJSON
              </button>
              <button
                  onClick={fetchMapData}
                  disabled={loading}
                  className="flex items-center gap-2 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg transition-colors"
              >
                <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
                Refresh
              </button>
            </div>
          </div>
        </div>

        {/* Leaflet Map */}
        <div className="bg-gray-900 rounded-lg border border-gray-800 overflow-hidden mb-6">
          <div className="bg-gray-800 px-4 py-2 border-b border-gray-700">
            <h3 className="font-semibold">Interactive World Map</h3>
            <p className="text-xs text-gray-400 mt-1">
              {isDrawingZone
                  ? '🖱️ Click on the map to draw zone boundaries. Click "Complete Drawing" when done.'
                  : 'Click markers for item details. Click zones to see items inside.'}
            </p>
          </div>

          <div className="h-[600px]">
            <MapContainer
                center={[20, 0]}
                zoom={2}
                style={{ height: '100%', width: '100%' }}
                className="z-0"
            >
              <TileLayer
                  attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>'
                  url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
              />

              {/* Draw new zone */}
              {isDrawingZone && (
                  <DrawZone onComplete={handleDrawingComplete} isDrawing={isDrawingZone} />
              )}

              {/* Display existing zones */}
              {zones.map((zone) => {
                const positions = zone.boundary.points.map(p => [p.latitude, p.longitude]);
                return (
                    <Polygon
                        key={zone.id}
                        positions={positions}
                        pathOptions={{
                          color: getZoneColor(zone.type),
                          fillColor: getZoneColor(zone.type),
                          fillOpacity: 0.2,
                          weight: 2
                        }}
                        eventHandlers={{
                          click: () => {
                            setSelectedZone(zone);
                            fetchItemsInZone(zone.name);
                          }
                        }}
                    >
                      <Popup>
                        <div className="text-gray-900">
                          <h3 className="font-bold text-lg mb-2">{zone.name}</h3>
                          <div className="space-y-1 text-sm">
                            <div><strong>Type:</strong> {zone.type}</div>
                            <div><strong>Points:</strong> {zone.boundary.points.length}</div>
                          </div>
                        </div>
                      </Popup>
                    </Polygon>
                );
              })}

              {/* Display items */}
              {items.map((item) => (
                  <Marker
                      key={item.itemId}
                      position={[item.position.latitude, item.position.longitude]}
                      icon={item.isCollected ? collectedIcon : availableIcon}
                  >
                    <Popup>
                      <div className="text-gray-900">
                        <h3 className="font-bold text-lg mb-2">{item.itemName}</h3>
                        <div className="space-y-1 text-sm">
                          <div>
                            <strong>Status:</strong>{' '}
                            <span className={item.isCollected ? 'text-gray-600' : 'text-green-600'}>
                          {item.isCollected ? 'Collected' : 'Available'}
                        </span>
                          </div>
                          <div>
                            <strong>Position:</strong><br/>
                            Lat: {item.position.latitude.toFixed(4)}<br/>
                            Lon: {item.position.longitude.toFixed(4)}
                          </div>
                        </div>
                      </div>
                    </Popup>
                  </Marker>
              ))}
            </MapContainer>
          </div>

          {/* Legend */}
          <div className="bg-gray-800 px-4 py-2 border-t border-gray-700">
            <div className="flex items-center gap-4 text-xs flex-wrap">
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-green-500"></div>
                <span>Available Item</span>
              </div>
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-gray-600"></div>
                <span>Collected Item</span>
              </div>
              <div className="flex items-center gap-2">
                <Layers className="w-3 h-3 text-purple-400" />
                <span>Zone (click to see items)</span>
              </div>
              <div className="ml-auto text-gray-400">
                <MapPin className="w-4 h-4 inline mr-1" />
                {items.length} items • {zones.length} zones
              </div>
            </div>
          </div>
        </div>

        {/* Zones List */}
        <div className="bg-gray-900 rounded-lg border border-gray-800 p-6 mb-6">
          <h3 className="font-semibold mb-4">Zones</h3>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {zones.length === 0 && (
                <p className="text-gray-500 text-center py-4 col-span-full">
                  No zones created yet. Click "Create Zone" to draw one!
                </p>
            )}
            {zones.map(zone => (
                <div
                    key={zone.id}
                    className="p-4 bg-gray-800 rounded hover:bg-gray-750 cursor-pointer"
                    onClick={() => {
                      setSelectedZone(zone);
                      fetchItemsInZone(zone.name);
                    }}
                >
                  <div className="flex items-start justify-between mb-2">
                    <h4 className="font-medium">{zone.name}</h4>
                    <div
                        className="w-4 h-4 rounded"
                        style={{ backgroundColor: getZoneColor(zone.type) }}
                    />
                  </div>
                  <div className="text-sm text-gray-400">
                    Type: {zone.type}
                  </div>
                  <div className="text-xs text-gray-500 mt-1">
                    {zone.boundary.points.length} boundary points
                  </div>
                </div>
            ))}
          </div>
        </div>

        {/* Create Zone Modal */}
        {showCreateZoneModal && (
            <div className="fixed inset-0 bg-black/90 flex items-center justify-center p-4 z-50">
              <div className="bg-gray-900 rounded-lg max-w-md w-full border border-gray-800">
                <div className="p-6">
                  <div className="flex justify-between items-start mb-6">
                    <h2 className="text-2xl font-bold text-white">Create Zone</h2>
                    <button onClick={cancelDrawing} className="text-gray-400 hover:text-gray-300">
                      <X className="w-6 h-6" />
                    </button>
                  </div>

                  {createZoneError && (
                      <div className="mb-4 bg-red-900/50 border border-red-500 rounded-lg p-3">
                        <p className="text-red-200 text-sm">{createZoneError}</p>
                      </div>
                  )}

                  <div className="space-y-4">
                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Zone Name *</label>
                      <input
                          type="text"
                          required
                          value={zoneFormData.name}
                          onChange={(e) => setZoneFormData({...zoneFormData, name: e.target.value})}
                          className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800"
                          placeholder="Dark Forest"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Zone Type *</label>
                      <select
                          value={zoneFormData.type}
                          onChange={(e) => setZoneFormData({...zoneFormData, type: e.target.value})}
                          className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800"
                      >
                        <option value="Forest">Forest</option>
                        <option value="Desert">Desert</option>
                        <option value="City">City</option>
                        <option value="Mountain">Mountain</option>
                        <option value="Water">Water</option>
                        <option value="Dungeon">Dungeon</option>
                      </select>
                    </div>

                    <div className="bg-gray-800 p-3 rounded">
                      <p className="text-sm text-gray-400">
                        {drawnPoints.length === 0
                            ? '👆 Click on the map to add points'
                            : `✓ ${drawnPoints.length} points drawn`}
                      </p>
                    </div>

                    <div className="flex gap-3 pt-4">
                      {isDrawingZone && drawnPoints.length > 0 && (
                          <button
                              onClick={() => setIsDrawingZone(false)}
                              className="flex-1 px-4 py-2 bg-yellow-600 hover:bg-yellow-700 text-white rounded-lg"
                          >
                            Complete Drawing
                          </button>
                      )}

                      {!isDrawingZone && drawnPoints.length >= 3 && (
                          <>
                            <button
                                onClick={cancelDrawing}
                                className="flex-1 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg"
                            >
                              Cancel
                            </button>
                            <button
                                onClick={handleCreateZone}
                                disabled={createZoneLoading}
                                className="flex-1 px-4 py-2 bg-purple-600 hover:bg-purple-700 text-white rounded-lg disabled:opacity-50"
                            >
                              {createZoneLoading ? 'Creating...' : 'Create Zone'}
                            </button>
                          </>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            </div>
        )}

        {/* Zone Items Modal */}
        {selectedZone && (
            <div className="fixed inset-0 bg-black/90 flex items-center justify-center p-4 z-50">
              <div className="bg-gray-900 rounded-lg max-w-2xl w-full border border-gray-800">
                <div className="p-6">
                  <div className="flex justify-between items-start mb-6">
                    <div>
                      <h2 className="text-2xl font-bold text-white">{selectedZone.name}</h2>
                      <p className="text-gray-400 text-sm mt-1">Items in this zone</p>
                    </div>
                    <button
                        onClick={() => {
                          setSelectedZone(null);
                          setZoneItems([]);
                        }}
                        className="text-gray-400 hover:text-gray-300"
                    >
                      <X className="w-6 h-6" />
                    </button>
                  </div>

                  {zoneItemsLoading && (
                      <div className="text-center py-8 text-gray-400">
                        <RefreshCw className="w-6 h-6 animate-spin mx-auto mb-2" />
                        Loading items...
                      </div>
                  )}

                  {!zoneItemsLoading && zoneItems.length === 0 && (
                      <div className="text-center py-8 text-gray-400">
                        No items found in this zone
                      </div>
                  )}

                  {!zoneItemsLoading && zoneItems.length > 0 && (
                      <div className="space-y-2 max-h-96 overflow-y-auto">
                        {zoneItems.map(item => (
                            <div
                                key={item.itemId}
                                className="p-4 bg-gray-800 rounded hover:bg-gray-750"
                            >
                              <div className="flex justify-between items-start">
                                <div>
                                  <h4 className="font-medium">{item.itemName}</h4>
                                  <div className="text-xs text-gray-400 mt-1">
                                    Lat: {item.position.latitude.toFixed(4)},
                                    Lon: {item.position.longitude.toFixed(4)}
                                  </div>
                                </div>
                                <span className={`text-xs ${item.isCollected ? 'text-gray-500' : 'text-green-400'}`}>
                          {item.isCollected ? 'Collected' : 'Available'}
                        </span>
                              </div>
                            </div>
                        ))}
                      </div>
                  )}

                  <div className="mt-6">
                    <button
                        onClick={() => {
                          setSelectedZone(null);
                          setZoneItems([]);
                        }}
                        className="w-full px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg"
                    >
                      Close
                    </button>
                  </div>
                </div>
              </div>
            </div>
        )}
      </div>
  );
}