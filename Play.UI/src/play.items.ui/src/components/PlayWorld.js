import React, { useState, useEffect } from 'react';
import { MapContainer, TileLayer, Marker, Popup, useMap } from 'react-leaflet';
import { RefreshCw, Download, MapPin } from 'lucide-react';
import 'leaflet/dist/leaflet.css';
import L from 'leaflet';

// Fix Leaflet icon issue with React
delete L.Icon.Default.prototype._getIconUrl;
L.Icon.Default.mergeOptions({
  iconRetinaUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon-2x.png',
  iconUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-icon.png',
  shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.7.1/images/marker-shadow.png',
});

// Custom icons for available and collected items
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

// Component to fit map bounds to all markers
function FitBounds({ items }) {
  const map = useMap();

  useEffect(() => {
    if (items.length > 0) {
      const bounds = items.map(item => [
        item.position.latitude,
        item.position.longitude
      ]);
      map.fitBounds(bounds, { padding: [50, 50] });
    }
  }, [items, map]);

  return null;
}

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

      // Handle the response structure
      const itemsData = data.itemLocations || data.items || data || [];
      setItems(itemsData);
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

        {/* Leaflet Map */}
        <div className="bg-gray-900 rounded-lg border border-gray-800 overflow-hidden mb-6">
          <div className="bg-gray-800 px-4 py-2 border-b border-gray-700">
            <h3 className="font-semibold">Interactive World Map</h3>
            <p className="text-xs text-gray-400 mt-1">
              Click on markers to see item details
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
                  attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                  url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
              />

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
                          <div>
                            <strong>Dropped:</strong><br/>
                            {new Date(item.droppedAt).toLocaleString()}
                          </div>
                          <div className="text-xs text-gray-500 mt-2">
                            ID: {item.itemId}
                          </div>
                        </div>
                      </div>
                    </Popup>
                  </Marker>
              ))}

              {items.length > 0 && <FitBounds items={items} />}
            </MapContainer>
          </div>

          {/* Legend */}
          <div className="bg-gray-800 px-4 py-2 border-t border-gray-700">
            <div className="flex items-center gap-4 text-xs">
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-green-500"></div>
                <span>Available Item</span>
              </div>
              <div className="flex items-center gap-2">
                <div className="w-3 h-3 rounded-full bg-gray-600"></div>
                <span>Collected Item</span>
              </div>
              <div className="ml-auto text-gray-400">
                <MapPin className="w-4 h-4 inline mr-1" />
                {items.length} items on map
              </div>
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