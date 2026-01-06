import React, { useState, useEffect } from 'react';
import { RefreshCw, Download, MapPin } from 'lucide-react';

const API_BASE_URL = 'http://localhost:5008/play-world';

export default function PlayWorld() {
  const [items, setItems] = useState([]);
  const [zones, setZones] = useState([]);
  const [loading, setLoading] = useState(false);
  const [geoJson, setGeoJson] = useState(null);

  const fetchMapData = async () => {
    setLoading(true);
    try {
      const response = await fetch(`${API_BASE_URL}/itemLocations`);
      const data = await response.json();
      setItems(data.items || []);
      setZones(data.zones || []);

      // Generate GeoJSON
      const geojson = {
        type: 'FeatureCollection',
        features: [
          // Items as points
          ...data.items.map(item => ({
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
          // Zones as polygons (if available)
          ...data.zones.map(zone => ({
            type: 'Feature',
            geometry: {
              type: 'Polygon',
              coordinates: zone.boundary.coordinates
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
    } catch (err) {
      console.error('Error fetching map data:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchMapData();
  }, []);

  const downloadGeoJSON = () => {
    const blob = new Blob([JSON.stringify(geoJson, null, 2)], { type: 'application/json' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'play-world-map.geojson';
    a.click();
    URL.revokeObjectURL(url);
  };

  return (
      <div>
        {/* Controls */}
        <div className="bg-gray-900 rounded-lg p-6 mb-6 border border-gray-800">
          <div className="flex justify-between items-center">
            <div className="text-sm text-gray-400">
              Items on map: <span className="text-white font-semibold">{items.length}</span>
              <span className="ml-4">Zones: <span className="text-white font-semibold">{zones.length}</span></span>
            </div>
            <div className="flex gap-3">
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

        {/* Map Visualization */}
        <div className="bg-gray-900 rounded-lg border border-gray-800 overflow-hidden mb-6">
          <div className="bg-gray-800 px-4 py-2 border-b border-gray-700">
            <h3 className="font-semibold">Interactive World Map</h3>
            <p className="text-xs text-gray-400 mt-1">
              Showing item locations with coordinates (Lat, Lon)
            </p>
          </div>

          {/* Simple Map Visualization */}
          <div className="relative h-96 bg-gradient-to-br from-gray-800 to-gray-900">
            <svg className="absolute inset-0 w-full h-full">
              {/* Grid */}
              <defs>
                <pattern id="grid" width="40" height="40" patternUnits="userSpaceOnUse">
                  <path d="M 40 0 L 0 0 0 40" fill="none" stroke="rgba(255,255,255,0.05)" strokeWidth="1"/>
                </pattern>
              </defs>
              <rect width="100%" height="100%" fill="url(#grid)" />

              {/* Items as circles */}
              {items.map((item) => {
                // Convert lat/lon to percentage positions
                const x = ((item.position.longitude + 180) / 360) * 100;
                const y = ((90 - item.position.latitude) / 180) * 100;
                return (
                    <g key={item.itemId}>
                      <circle
                          cx={`${x}%`}
                          cy={`${y}%`}
                          r="6"
                          fill={item.isCollected ? '#666' : '#10b981'}
                          stroke="#fff"
                          strokeWidth="2"
                      />
                      <text
                          x={`${x}%`}
                          y={`${y - 2}%`}
                          fontSize="11"
                          fill="#fff"
                          textAnchor="middle"
                          fontWeight="bold"
                      >
                        {item.itemName}
                      </text>
                    </g>
                );
              })}
            </svg>

            {/* Legend */}
            <div className="absolute bottom-4 left-4 bg-black/80 px-3 py-2 rounded text-xs">
              <div className="flex items-center gap-2 mb-1">
                <div className="w-3 h-3 rounded-full bg-green-500"></div>
                <span>Available Item</span>
              </div>
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-gray-600"></div>
                <span>Collected Item</span>
              </div>
            </div>

            {/* Info */}
            <div className="absolute top-4 right-4 bg-black/80 px-3 py-2 rounded text-xs">
              <MapPin className="w-4 h-4 inline mr-1" />
              <span>{items.length} items tracked</span>
            </div>
          </div>
        </div>

        {/* Items List */}
        <div className="bg-gray-900 rounded-lg border border-gray-800 p-6 mb-6">
          <h3 className="font-semibold mb-4">Item Locations</h3>
          <div className="space-y-2 max-h-64 overflow-y-auto">
            {items.length === 0 && (
                <p className="text-gray-500 text-center py-4">
                  No items on map yet. Create items in Play.Items to see them here!
                </p>
            )}
            {items.map(item => (
                <div
                    key={item.itemId}
                    className="flex justify-between items-center p-3 bg-gray-800 rounded hover:bg-gray-750"
                >
                  <div>
                    <div className="font-medium">{item.itemName}</div>
                    <div className="text-xs text-gray-400">
                      Lat: {item.position.latitude.toFixed(4)},
                      Lon: {item.position.longitude.toFixed(4)}
                    </div>
                    <div className="text-xs text-gray-500 mt-1">
                      Dropped: {new Date(item.droppedAt).toLocaleString()}
                    </div>
                  </div>
                  <div className="text-sm">
                    {item.isCollected ? (
                        <span className="text-gray-500 text-xs">● Collected</span>
                    ) : (
                        <span className="text-green-400 text-xs">● Available</span>
                    )}
                  </div>
                </div>
            ))}
          </div>
        </div>

        {/* GeoJSON Preview */}
        {geoJson && (
            <div className="bg-gray-900 rounded-lg border border-gray-800 p-6">
              <div className="flex justify-between items-center mb-4">
                <h3 className="font-semibold">GeoJSON Data</h3>
                <span className="text-xs text-gray-500">
              {geoJson.features.length} features
            </span>
              </div>
              <pre className="bg-black p-4 rounded text-xs overflow-x-auto max-h-64 overflow-y-auto text-gray-300">
            {JSON.stringify(geoJson, null, 2)}
          </pre>
              <p className="text-xs text-gray-500 mt-2">
                💡 This GeoJSON can be imported into QGIS, ArcGIS, or any GIS tool
              </p>
            </div>
        )}
      </div>
  );
}