import React, { useState, useEffect } from 'react';
import { RefreshCw, Search, Eye, Calendar, DollarSign, AlertCircle, Plus, X, Edit } from 'lucide-react';

const API_BASE_URL = 'http://localhost:5008/play-items';

function App() {
  const [items, setItems] = useState([]);
  const [crafters, setCrafters] = useState([]);
  const [elements, setElements] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedItem, setSelectedItem] = useState(null);
  const [autoRefresh, setAutoRefresh] = useState(true);
  const [showCreateModal, setShowCreateModal] = useState(false);
  const [showSocketModal, setShowSocketModal] = useState(false);
  const [socketItemId, setSocketItemId] = useState(null);
  const [createLoading, setCreateLoading] = useState(false);
  const [socketLoading, setSocketLoading] = useState(false);
  const [createError, setCreateError] = useState(null);
  const [socketError, setSocketError] = useState(null);
  const [hollowType, setHollowType] = useState('Stone');
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    price: '',
    crafterId: '',
    element: ''
  });

  const fetchItems = async () => {
    try {
      setLoading(true);
      setError(null);

      const timestamp = new Date().getTime();
      const url = `${API_BASE_URL}/items?_t=${timestamp}`;
      console.log('Fetching from:', url);

      const response = await fetch(url, {
        method: 'GET',
        mode: 'cors',
        cache: 'no-cache',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      console.log('Response status:', response.status);

      if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      }

      const data = await response.json();
      console.log('Received data:', data);
      setItems(Array.isArray(data) ? data : []);
    } catch (err) {
      console.error('Fetch error:', err);
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const fetchCrafters = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/crafters`, {
        method: 'GET',
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        const data = await response.json();
        console.log('Crafters:', data);
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
        headers: {
          'Content-Type': 'application/json'
        }
      });

      if (response.ok) {
        const data = await response.json();
        console.log('Elements:', data);
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

  const formatDate = (date) => {
    return new Date(date).toLocaleString();
  };

  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(price);
  };

  const handleCreateItem = async () => {
    setCreateLoading(true);
    setCreateError(null);

    try {
      const response = await fetch(`${API_BASE_URL}/items`, {
        method: 'POST',
        mode: 'cors',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: formData.name,
          description: formData.description,
          price: parseFloat(formData.price),
          crafterId: formData.crafterId,
          element: formData.element
        })
      });

      if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      }

      // Reset form
      setFormData({
        name: '',
        description: '',
        price: '',
        crafterId: '',
        element: ''
      });

      // Close modal
      setShowCreateModal(false);

      // Refresh items list
      await fetchItems();
    } catch (err) {
      console.error('Create error:', err);
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
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          hollowType: hollowType
        })
      });

      if (!response.ok) {
        throw new Error(`HTTP ${response.status}: ${response.statusText}`);
      }

      setShowSocketModal(false);
      setSocketItemId(null);
      setHollowType('Stone');

      await fetchItems();
    } catch (err) {
      console.error('Socket error:', err);
      setSocketError(err.message);
    } finally {
      setSocketLoading(false);
    }
  };

  return (
      <div className="min-h-screen bg-black">
        <div className="container mx-auto px-4 py-8">
          {/* Header */}
          <div className="mb-8">
            <h1 className="text-4xl font-bold text-white mb-2">
              Play.Items Admin Dashboard
            </h1>
            <p className="text-gray-400">Manage and monitor game items in real-time</p>
            <div className="mt-2 text-sm text-gray-500">
              API: <code className="bg-gray-900 px-2 py-1 rounded border border-gray-800">{API_BASE_URL}/items</code>
            </div>
          </div>

          {/* Controls */}
          <div className="bg-gray-900 rounded-lg p-6 mb-6 border border-gray-800">
            <div className="flex flex-col md:flex-row gap-4 items-center justify-between">
              {/* Search */}
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

              {/* Controls */}
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

            {/* Stats */}
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

          {/* Error Message */}
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

          {/* Add Socket Modal */}
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
                        <p className="text-gray-500 text-xs mt-2">Select the type of hollow for this socket</p>
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

          {/* Loading State */}
          {loading && items.length === 0 && !error && (
              <div className="flex flex-col items-center justify-center py-12 text-gray-400">
                <RefreshCw className="w-8 h-8 mb-3 animate-spin" />
                <p>Loading items...</p>
              </div>
          )}

          {/* Empty State */}
          {!loading && !error && filteredItems.length === 0 && (
              <div className="text-center py-12">
                <div className="text-gray-400 mb-4">
                  {searchTerm
                      ? '🔍 No items match your search.'
                      : '📦 No items found in database.'}
                </div>
                {!searchTerm && (
                    <p className="text-gray-500 text-sm">
                      Create an item by clicking the "Create Item" button above!
                    </p>
                )}
              </div>
          )}

          {/* Items Grid */}
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {filteredItems.map((item) => (
                <div
                    key={item.id}
                    className="bg-gray-900 rounded-lg p-6 border border-gray-800 hover:border-gray-700 transition-all"
                >
                  <div className="flex justify-between items-start mb-4">
                    <h3 className="text-xl font-bold text-white">{item.name || 'Unnamed Item'}</h3>
                    <div className="flex gap-2">
                      <button
                          onClick={() => {
                            setSocketItemId(item.id);
                            setShowSocketModal(true);
                          }}
                          className="p-2 hover:bg-gray-800 rounded-lg transition-colors"
                          title="Add/Edit Socket"
                      >
                        <Edit className="w-5 h-5 text-gray-400" />
                      </button>
                      <button
                          onClick={() => setSelectedItem(item)}
                          className="p-2 hover:bg-gray-800 rounded-lg transition-colors"
                          title="View Details"
                      >
                        <Eye className="w-5 h-5 text-gray-400" />
                      </button>
                    </div>
                  </div>

                  <p className="text-gray-400 text-sm mb-4 line-clamp-2">
                    {item.description || 'No description'}
                  </p>

                  <div className="space-y-2">
                    <div className="flex items-center gap-2 text-sm">
                      <DollarSign className="w-4 h-4 text-green-400" />
                      <span className="text-green-400 font-semibold">
                    {formatPrice(item.price || 0)}
                  </span>
                    </div>

                    <div className="flex items-center gap-2 text-sm text-gray-400">
                      <Calendar className="w-4 h-4" />
                      <span>{formatDate(item.createdDate)}</span>
                    </div>
                  </div>

                  <div className="mt-4 pt-4 border-t border-gray-800">
                    <code className="text-xs text-gray-500 break-all">
                      ID: {item.id}
                    </code>
                  </div>
                </div>
            ))}
          </div>

          {/* Item Details Modal */}
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
                          <p className="text-green-400 text-xl font-bold mt-1">
                            {formatPrice(selectedItem.price || 0)}
                          </p>
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
                              {selectedItem.socket.artifact && (
                                  <div className="flex justify-between">
                                    <span className="text-gray-400 text-sm">Artifact:</span>
                                    <span className="text-purple-400 font-semibold">{selectedItem.socket.artifact.name || 'N/A'}</span>
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

          {/* Create Item Modal */}
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