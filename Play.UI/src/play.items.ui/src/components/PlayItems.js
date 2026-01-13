import React, { useState, useEffect } from 'react';
import { RefreshCw, Search, Eye, Calendar, DollarSign, AlertCircle, Plus, X } from 'lucide-react';

const API_BASE_URL = 'http://localhost:5008/play-items';

function App() {
  const [items, setItems] = useState([]);
  const [crafters, setCrafters] = useState([]);
  const [elements, setElements] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedItem, setSelectedItem] = useState(null);
  const [autoRefresh, setAutoRefresh] = useState(false);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showSocketModal, setShowSocketModal] = useState(false);
  const [socketItemId, setSocketItemId] = useState(null);
  const [createLoading, setCreateLoading] = useState(false);
  const [socketLoading, setSocketLoading] = useState(false);
  const [createError, setCreateError] = useState(null);
  const [socketError, setSocketError] = useState(null);
  const [hollowType, setHollowType] = useState('Stone');
  const [showArtifactModal, setShowArtifactModal] = useState(false);
  const [artifactItemId, setArtifactItemId] = useState(null);
  const [artifactLoading, setArtifactLoading] = useState(false);
  const [artifactError, setArtifactError] = useState(null);
  const [artifactFormData, setArtifactFormData] = useState({
    artifactName: '',
    stats: [{ key: '', value: '' }]
  });
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    price: '',
    crafterId: '',
    element: ''
  });
  const [detailsLoading, setDetailsLoading] = useState(false);
  const [detailsError, setDetailsError] = useState(null);

  const fetchItems = async () => {
    try {
      setLoading(true);
      setError(null);
      const timestamp = new Date().getTime();
      const url = `${API_BASE_URL}/items?_t=${timestamp}`;
      const response = await fetch(url, {
        method: 'GET',
        mode: 'cors',
        cache: 'no-cache',
        headers: { 'Content-Type': 'application/json' }
      });
      if (!response.ok) throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      const data = await response.json();
      setItems(Array.isArray(data) ? data : []);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const fetchItemDetails = async (id) => {
    try {
      setDetailsLoading(true);
      setDetailsError(null);
      const timestamp = new Date().getTime();
      const url = `${API_BASE_URL}/items/${id}?_t=${timestamp}`;
      const response = await fetch(url, {
        method: 'GET',
        mode: 'cors',
        cache: 'no-cache',
        headers: { 'Content-Type': 'application/json' },
      });
      if (!response.ok) throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      const data = await response.json();
      setSelectedItem(data);
    } catch (err) {
      setDetailsError(err.message);
    } finally {
      setDetailsLoading(false);
    }
  };

  const fetchCrafters = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/crafters`, {
        method: 'GET',
        mode: 'cors',
        headers: { 'Content-Type': 'application/json' }
      });
      if (response.ok) {
        const data = await response.json();
        setCrafters(Array.isArray(data) ? data : []);
      }
    } catch (err) {
      console.error('Error fetching crafters:', err);
    }
  };

  const fetchElements = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/elements`, {
        method: 'GET',
        mode: 'cors',
        headers: { 'Content-Type': 'application/json' }
      });
      if (response.ok) {
        const data = await response.json();
        setElements(Array.isArray(data) ? data : []);
      }
    } catch (err) {
      console.error('Error fetching elements:', err);
    }
  };

  useEffect(() => {
    fetchItems();
    fetchCrafters();
    fetchElements();
  }, []);

  useEffect(() => {
    if (!autoRefresh) return;
    const interval = setInterval(() => {
      fetchItems();
    }, 5000);
    return () => clearInterval(interval);
  }, [autoRefresh]);

  const filteredItems = items.filter(item =>
      item.name?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      item.description?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const formatDate = (date) => new Date(date).toLocaleString();
  const formatPrice = (price) => new Intl.NumberFormat('en-US', { style: 'currency', currency: 'USD' }).format(price);

  const handleCreateItem = async () => {
    setCreateLoading(true);
    setCreateError(null);
    try {
      const response = await fetch(`${API_BASE_URL}/items`, {
        method: 'POST',
        mode: 'cors',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          name: formData.name,
          description: formData.description,
          price: parseFloat(formData.price),
          crafterId: formData.crafterId,
          element: formData.element
        })
      });
      if (!response.ok) throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      setFormData({ name: '', description: '', price: '', crafterId: '', element: '' });
      setShowCreateModal(false);
      await fetchItems();
    } catch (err) {
      setCreateError(err.message);
    } finally {
      setCreateLoading(false);
    }
  };

  const handleAddSocket = async () => {
    setSocketLoading(true);
    setSocketError(null);
    try {
      const response = await fetch(`${API_BASE_URL}/items/${socketItemId}/socket`, {
        method: 'POST',
        mode: 'cors',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ hollowType: hollowType })
      });
      if (!response.ok) throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      setShowSocketModal(false);
      setSocketItemId(null);
      setHollowType('Stone');
      await fetchItems();
    } catch (err) {
      setSocketError(err.message);
    } finally {
      setSocketLoading(false);
    }
  };

  const handleEmbedArtifact = async () => {
    setArtifactLoading(true);
    setArtifactError(null);
    try {
      const statsDict = {};
      artifactFormData.stats.forEach(stat => {
        if (stat.key && stat.value) {
          statsDict[stat.key] = parseInt(stat.value);
        }
      });
      const response = await fetch(`${API_BASE_URL}/items/${artifactItemId}/artifact`, {
        method: 'POST',
        mode: 'cors',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          artifactName: artifactFormData.artifactName,
          stats: statsDict
        })
      });
      if (!response.ok) throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      setShowArtifactModal(false);
      setArtifactItemId(null);
      setArtifactFormData({ artifactName: '', stats: [{ key: '', value: '' }] });
      await fetchItems();
    } catch (err) {
      setArtifactError(err.message);
    } finally {
      setArtifactLoading(false);
    }
  };

  const addStatField = () => {
    setArtifactFormData({
      ...artifactFormData,
      stats: [...artifactFormData.stats, { key: '', value: '' }]
    });
  };

  const removeStatField = (index) => {
    const newStats = artifactFormData.stats.filter((_, i) => i !== index);
    setArtifactFormData({ ...artifactFormData, stats: newStats });
  };

  const updateStatField = (index, field, value) => {
    const newStats = [...artifactFormData.stats];
    newStats[index][field] = value;
    setArtifactFormData({ ...artifactFormData, stats: newStats });
  };

  return (
      <div className="min-h-screen bg-black">
        <div className="container mx-auto px-4 py-8">
          <div className="mb-8">
            <h1 className="text-4xl font-bold text-white mb-2">Play.Items Admin Dashboard</h1>
            <p className="text-gray-400">Manage and monitor game items in real-time</p>
            <div className="mt-2 text-sm text-gray-500">
              API: <code className="bg-gray-900 px-2 py-1 rounded border border-gray-800">{API_BASE_URL}/items</code>
            </div>
          </div>

          <div className="bg-gray-900 rounded-lg p-6 mb-6 border border-gray-800">
            <div className="flex flex-col md:flex-row gap-4 items-center justify-between">
              <div className="relative flex-1 w-full md:max-w-md">
                <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500 w-5 h-5" />
                <input
                    type="text"
                    placeholder="Search items..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="w-full pl-10 pr-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                />
              </div>
              <div className="flex gap-3 items-center">
                <button
                    onClick={() => setShowCreateModal(true)}
                    className="flex items-center gap-2 px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded-lg transition-colors"
                >
                  <Plus className="w-4 h-4" />
                  Create Item
                </button>
                <label className="flex items-center gap-2 text-gray-400 cursor-pointer">
                  <input
                      type="checkbox"
                      checked={autoRefresh}
                      onChange={(e) => setAutoRefresh(e.target.checked)}
                      className="w-4 h-4 rounded border-gray-700 bg-black"
                  />
                  <span className="text-sm">Auto-refresh (5s)</span>
                </label>
                <button
                    onClick={fetchItems}
                    disabled={loading}
                    className="flex items-center gap-2 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
                  Refresh
                </button>
              </div>
            </div>
            <div className="mt-4 flex gap-4 text-sm">
              <div className="text-gray-400">
                Total Items: <span className="text-white font-semibold">{items.length}</span>
              </div>
              <div className="text-gray-400">
                Filtered: <span className="text-white font-semibold">{filteredItems.length}</span>
              </div>
              <div className={`${loading ? 'text-yellow-400' : 'text-green-400'}`}>
                Status: <span className="font-semibold">{loading ? 'Loading...' : 'Connected'}</span>
              </div>
            </div>
          </div>

          {error && (
              <div className="bg-red-900/50 border border-red-500 rounded-lg mb-6 p-4">
                <div className="flex items-start gap-3">
                  <AlertCircle className="w-5 h-5 text-red-400 flex-shrink-0 mt-0.5" />
                  <div>
                    <h3 className="text-red-200 font-semibold mb-1">Connection Error</h3>
                    <p className="text-red-300 text-sm mb-2">{error}</p>
                  </div>
                </div>
              </div>
          )}

          {loading && items.length === 0 && !error && (
              <div className="flex flex-col items-center justify-center py-12 text-gray-400">
                <RefreshCw className="w-8 h-8 mb-3 animate-spin" />
                <p>Loading items...</p>
              </div>
          )}

          {!loading && !error && filteredItems.length === 0 && (
              <div className="text-center py-12">
                <div className="text-gray-400 mb-4">
                  {searchTerm ? '🔍 No items match your search.' : '📦 No items found in database.'}
                </div>
                {!searchTerm && (
                    <p className="text-gray-500 text-sm">Create an item by clicking the "Create Item" button above!</p>
                )}
              </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredItems.map((item) => (
                <div key={item.id} className="bg-gray-900 rounded-lg p-6 border border-gray-800 hover:border-gray-700 transition-all">
                  <div className="flex justify-between items-start mb-4">
                    <h3 className="text-xl font-bold text-white">{item.name || 'Unnamed Item'}</h3>
                    <div className="flex gap-2">
                      {!item.socket && (
                          <button
                              onClick={() => {
                                setSocketItemId(item.id);
                                setShowSocketModal(true);
                              }}
                              className="p-2 hover:bg-gray-800 rounded-lg transition-colors"
                              title="Add Socket"
                          >
                            <Plus className="w-5 h-5 text-gray-400" />
                          </button>
                      )}
                      {item.socket && !item.socket.artifact && (
                          <button
                              onClick={() => {
                                setArtifactItemId(item.id);
                                setShowArtifactModal(true);
                              }}
                              className="p-2 hover:bg-gray-800 rounded-lg transition-colors"
                              title="Embed Artifact"
                          >
                            <Plus className="w-5 h-5 text-purple-400" />
                          </button>
                      )}
                      <button
                          onClick={() => {
                            setSelectedItem(item);
                            fetchItemDetails(item.id);
                          }}
                          className="p-2 hover:bg-gray-800 rounded-lg transition-colors"
                          title="View Details"
                      >
                        <Eye className="w-5 h-5 text-gray-400" />
                      </button>
                    </div>
                  </div>
                  <p className="text-gray-400 text-sm mb-4 line-clamp-2">{item.description || 'No description'}</p>
                  <div className="space-y-2">
                    <div className="flex items-center gap-2 text-sm">
                      <DollarSign className="w-4 h-4 text-green-400" />
                      <span className="text-green-400 font-semibold">{formatPrice(item.price || 0)}</span>
                    </div>
                    <div className="flex items-center gap-2 text-sm text-gray-400">
                      <Calendar className="w-4 h-4" />
                      <span>{formatDate(item.createdDate)}</span>
                    </div>
                  </div>
                  <div className="mt-4 pt-4 border-t border-gray-800">
                    <code className="text-xs text-gray-500 break-all">ID: {item.id}</code>
                  </div>
                </div>
            ))}
          </div>

          {showSocketModal && (
              <div className="fixed inset-0 bg-black/90 flex items-center justify-center p-4 z-50">
                <div className="bg-gray-900 rounded-lg max-w-md w-full border border-gray-800">
                  <div className="p-6">
                    <div className="flex justify-between items-start mb-6">
                      <h2 className="text-2xl font-bold text-white">Add Socket</h2>
                      <button
                          onClick={() => {
                            setShowSocketModal(false);
                            setSocketError(null);
                            setHollowType('Stone');
                          }}
                          className="text-gray-400 hover:text-gray-300"
                      >
                        <X className="w-6 h-6" />
                      </button>
                    </div>
                    {socketError && (
                        <div className="mb-4 bg-red-900/50 border border-red-500 rounded-lg p-3">
                          <p className="text-red-200 text-sm">{socketError}</p>
                        </div>
                    )}
                    <div className="space-y-4">
                      <div>
                        <label className="block text-gray-400 text-sm font-semibold mb-2">Hollow Type *</label>
                        <select
                            value={hollowType}
                            onChange={(e) => setHollowType(e.target.value)}
                            className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                        >
                          <option value="Stone">Stone</option>
                          <option value="Dust">Dust</option>
                          <option value="Liquid">Liquid</option>
                        </select>
                        <p className="text-gray-500 text-xs mt-2">⚠️ Hollow type cannot be changed after creation</p>
                      </div>
                      <div className="flex gap-3 pt-4">
                        <button
                            type="button"
                            onClick={() => {
                              setShowSocketModal(false);
                              setSocketError(null);
                              setHollowType('Stone');
                            }}
                            className="flex-1 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg transition-colors"
                        >
                          Cancel
                        </button>
                        <button
                            type="button"
                            onClick={handleAddSocket}
                            disabled={socketLoading}
                            className="flex-1 px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
                        >
                          {socketLoading ? (
                              <>
                                <RefreshCw className="w-4 h-4 animate-spin" />
                                Adding...
                              </>
                          ) : (
                              <>
                                <Plus className="w-4 h-4" />
                                Add Socket
                              </>
                          )}
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
          )}

          {showArtifactModal && (
              <div className="fixed inset-0 bg-black/90 flex items-center justify-center p-4 z-50">
                <div className="bg-gray-900 rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto border border-gray-800">
                  <div className="p-6">
                    <div className="flex justify-between items-start mb-6">
                      <h2 className="text-2xl font-bold text-purple-400">Embed Artifact</h2>
                      <button
                          onClick={() => {
                            setShowArtifactModal(false);
                            setArtifactError(null);
                            setArtifactFormData({ artifactName: '', stats: [{ key: '', value: '' }] });
                          }}
                          className="text-gray-400 hover:text-gray-300"
                      >
                        <X className="w-6 h-6" />
                      </button>
                    </div>
                    {artifactError && (
                        <div className="mb-4 bg-red-900/50 border border-red-500 rounded-lg p-3">
                          <p className="text-red-200 text-sm">{artifactError}</p>
                        </div>
                    )}
                    <div className="space-y-4">
                      <div className="bg-gray-800/50 border border-gray-700 rounded-lg p-4">
                        <label className="block text-gray-400 text-sm font-semibold mb-2">Existing Socket</label>
                        <div className="flex justify-between items-center">
                          <span className="text-gray-400 text-sm">Hollow Type:</span>
                          <span className="text-white font-semibold px-3 py-1 bg-gray-900 rounded">
                        {items.find(i => i.id === artifactItemId)?.socket?.hollowType || 'N/A'}
                      </span>
                        </div>
                        <p className="text-gray-500 text-xs mt-2">🔒 Socket is locked and cannot be modified</p>
                      </div>

                      <div className="border-t border-gray-800 pt-4">
                        <label className="block text-purple-400 text-sm font-semibold mb-4">New Artifact Details</label>

                        <div className="mb-4">
                          <label className="block text-gray-400 text-sm font-semibold mb-2">Artifact Name *</label>
                          <input
                              type="text"
                              required
                              value={artifactFormData.artifactName}
                              onChange={(e) => setArtifactFormData({...artifactFormData, artifactName: e.target.value})}
                              className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                              placeholder="Enter artifact name"
                          />
                          <p className="text-gray-500 text-xs mt-2">⚠️ Artifact cannot be changed after embedding</p>
                        </div>

                        <div>
                          <label className="block text-gray-400 text-sm font-semibold mb-3">Stats</label>
                          {artifactFormData.stats.map((stat, index) => (
                              <div key={index} className="flex gap-2 mb-2">
                                <input
                                    type="text"
                                    value={stat.key}
                                    onChange={(e) => updateStatField(index, 'key', e.target.value)}
                                    className="flex-1 px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                                    placeholder="Stat name (e.g., Strength)"
                                />
                                <input
                                    type="number"
                                    value={stat.value}
                                    onChange={(e) => updateStatField(index, 'value', e.target.value)}
                                    className="w-24 px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                                    placeholder="Value"
                                />
                                {artifactFormData.stats.length > 1 && (
                                    <button
                                        type="button"
                                        onClick={() => removeStatField(index)}
                                        className="p-2 bg-red-900/50 hover:bg-red-900 text-red-400 rounded-lg transition-colors"
                                    >
                                      <X className="w-5 h-5" />
                                    </button>
                                )}
                              </div>
                          ))}
                          <button
                              type="button"
                              onClick={addStatField}
                              className="mt-2 flex items-center gap-2 px-3 py-2 bg-gray-800 hover:bg-gray-700 text-gray-400 rounded-lg transition-colors text-sm"
                          >
                            <Plus className="w-4 h-4" />
                            Add Stat
                          </button>
                        </div>
                      </div>

                      <div className="flex gap-3 pt-4">
                        <button
                            type="button"
                            onClick={() => {
                              setShowArtifactModal(false);
                              setArtifactError(null);
                              setArtifactFormData({ artifactName: '', stats: [{ key: '', value: '' }] });
                            }}
                            className="flex-1 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg transition-colors"
                        >
                          Cancel
                        </button>
                        <button
                            type="button"
                            onClick={handleEmbedArtifact}
                            disabled={artifactLoading || !artifactFormData.artifactName}
                            className="flex-1 px-4 py-2 bg-purple-700 hover:bg-purple-600 text-white rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
                        >
                          {artifactLoading ? (
                              <>
                                <RefreshCw className="w-4 h-4 animate-spin" />
                                Embedding...
                              </>
                          ) : (
                              <>
                                <Plus className="w-4 h-4" />
                                Embed Artifact
                              </>
                          )}
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
          )}

          {selectedItem && (
              <div className="fixed inset-0 bg-black/90 flex items-center justify-center p-4 z-50">
                <div className="bg-gray-900 rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto border border-gray-800">
                  <div className="p-6">
                    <div className="flex justify-between items-start mb-6">
                      <h2 className="text-2xl font-bold text-white">{selectedItem.name}</h2>
                      <button
                          onClick={() => setSelectedItem(null)}
                          className="text-gray-400 hover:text-gray-300 text-2xl leading-none"
                      >
                        <X className="w-6 h-6" />
                      </button>
                    </div>
                    <div className="space-y-4">
                      <div>
                        <label className="text-gray-400 text-sm font-semibold">Description</label>
                        <p className="text-white mt-1">{selectedItem.description || 'No description'}</p>
                      </div>
                      <div className="grid grid-cols-2 gap-4">
                        <div>
                          <label className="text-gray-400 text-sm font-semibold">Price</label>
                          <p className="text-green-400 text-xl font-bold mt-1">{formatPrice(selectedItem.price || 0)}</p>
                        </div>
                        <div>
                          <label className="text-gray-400 text-sm font-semibold">Created Date</label>
                          <p className="text-white mt-1">{formatDate(selectedItem.createdDate)}</p>
                        </div>
                      </div>
                      {selectedItem.socket && (
                          <div className="bg-black border border-gray-800 rounded-lg p-4">
                            <label className="text-gray-400 text-sm font-semibold block mb-2">Socket</label>
                            <div className="space-y-2">
                              <div className="flex justify-between">
                                <span className="text-gray-400 text-sm">Hollow Type:</span>
                                <span className="text-white font-semibold">{selectedItem.socket.hollowType || 'N/A'}</span>
                              </div>
                              {selectedItem.socket.artifact ? (
                                  <div className="mt-3 pt-3 border-t border-gray-800">
                                    <div className="flex justify-between mb-2">
                                      <span className="text-gray-400 text-sm">Artifact:</span>
                                      <span className="text-purple-400 font-semibold">{selectedItem.socket.artifact.name || 'N/A'}</span>
                                    </div>
                                    {selectedItem.socket.artifact.stats && Object.keys(selectedItem.socket.artifact.stats).length > 0 && (
                                        <div className="mt-2 bg-gray-900 rounded p-3">
                                          <div className="text-gray-400 text-xs font-semibold mb-2">Stats:</div>
                                          <div className="space-y-1">
                                            {Object.entries(selectedItem.socket.artifact.stats).map(([key, value]) => (
                                                <div key={key} className="flex justify-between text-sm">
                                                  <span className="text-gray-400">{key}:</span>
                                                  <span className="text-green-400 font-semibold">+{value}</span>
                                                </div>
                                            ))}
                                          </div>
                                        </div>
                                    )}
                                  </div>
                              ) : (
                                  <div className="mt-3 pt-3 border-t border-gray-700">
                                    <p className="text-gray-500 text-sm italic">No artifact embedded</p>
                                  </div>
                              )}
                            </div>
                          </div>
                      )}
                      {!selectedItem.socket && (
                          <div className="bg-gray-800/50 border border-gray-700 rounded-lg p-4">
                            <p className="text-gray-400 text-sm">No socket added yet</p>
                          </div>
                      )}
                      <div>
                        <label className="text-gray-400 text-sm font-semibold">Item ID</label>
                        <code className="block text-gray-500 text-sm mt-1 break-all bg-black p-2 rounded border border-gray-800">
                          {selectedItem.id}
                        </code>
                      </div>
                    </div>
                    <div className="mt-6">
                      <button
                          onClick={() => setSelectedItem(null)}
                          className="w-full px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg transition-colors"
                      >
                        Close
                      </button>
                    </div>
                  </div>
                </div>
              </div>
          )}

          {showCreateModal && (
              <div className="fixed inset-0 bg-black/90 flex items-center justify-center p-4 z-50">
                <div className="bg-gray-900 rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto border border-gray-800">
                  <div className="p-6">
                    <div className="flex justify-between items-start mb-6">
                      <h2 className="text-2xl font-bold text-white">Create New Item</h2>
                      <button
                          onClick={() => {
                            setShowCreateModal(false);
                            setCreateError(null);
                          }}
                          className="text-gray-400 hover:text-gray-300"
                      >
                        <X className="w-6 h-6" />
                      </button>
                    </div>
                    {createError && (
                        <div className="mb-4 bg-red-900/50 border border-red-500 rounded-lg p-3">
                          <p className="text-red-200 text-sm">{createError}</p>
                        </div>
                    )}
                    <div className="space-y-4">
                      <div>
                        <label className="block text-gray-400 text-sm font-semibold mb-2">Name *</label>
                        <input
                            type="text"
                            required
                            value={formData.name}
                            onChange={(e) => setFormData({...formData, name: e.target.value})}
                            className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                            placeholder="Enter item name"
                        />
                      </div>
                      <div>
                        <label className="block text-gray-400 text-sm font-semibold mb-2">Description *</label>
                        <textarea
                            required
                            value={formData.description}
                            onChange={(e) => setFormData({...formData, description: e.target.value})}
                            rows="3"
                            className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                            placeholder="Enter item description"
                        />
                      </div>
                      <div>
                        <label className="block text-gray-400 text-sm font-semibold mb-2">Price *</label>
                        <input
                            type="number"
                            step="0.01"
                            min="0"
                            required
                            value={formData.price}
                            onChange={(e) => setFormData({...formData, price: e.target.value})}
                            className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                            placeholder="0.00"
                        />
                      </div>
                      <div>
                        <label className="block text-gray-400 text-sm font-semibold mb-2">Crafter *</label>
                        <select
                            required
                            value={formData.crafterId}
                            onChange={(e) => setFormData({...formData, crafterId: e.target.value})}
                            className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                        >
                          <option value="">Select a crafter</option>
                          {crafters.map((crafter) => (
                              <option key={crafter.crafterId} value={crafter.crafterId}>
                                {crafter.crafterName}
                              </option>
                          ))}
                        </select>
                      </div>
                      <div>
                        <label className="block text-gray-400 text-sm font-semibold mb-2">Element *</label>
                        <select
                            required
                            value={formData.element}
                            onChange={(e) => setFormData({...formData, element: e.target.value})}
                            className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-700"
                        >
                          <option value="">Select an element</option>
                          {elements.map((element) => (
                              <option key={element.elementId || element.name} value={element.name}>
                                {element.name}
                              </option>
                          ))}
                        </select>
                      </div>
                      <div className="flex gap-3 pt-4">
                        <button
                            type="button"
                            onClick={() => {
                              setShowCreateModal(false);
                              setCreateError(null);
                            }}
                            className="flex-1 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg transition-colors"
                        >
                          Cancel
                        </button>
                        <button
                            type="button"
                            onClick={handleCreateItem}
                            disabled={createLoading}
                            className="flex-1 px-4 py-2 bg-gray-700 hover:bg-gray-600 text-white rounded-lg transition-colors disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center gap-2"
                        >
                          {createLoading ? (
                              <>
                                <RefreshCw className="w-4 h-4 animate-spin" />
                                Creating...
                              </>
                          ) : (
                              <>
                                <Plus className="w-4 h-4" />
                                Create Item
                              </>
                          )}
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
          )}
        </div>
      </div>
  );
}

export default App;