import React, { useState } from 'react';
import PlayItems from './components/PlayItems';
import PlayWorld from './components/PlayWorld';
import PlayUsers from './components/PlayUsers';
import { Package, Map } from 'lucide-react';

function App() {
  const [activeTab, setActiveTab] = useState('items');

  return (
      <div className="min-h-screen bg-black text-white">
        <div className="container mx-auto px-4 py-8">
          {/* Header */}
          <div className="mb-8">
            <h1 className="text-4xl font-bold mb-2">Play Microservices Admin</h1>
            <p className="text-gray-400">Manage items and explore the game world</p>
          </div>

          {/* Tabs */}
          <div className="mb-6 border-b border-gray-800">
            <div className="flex gap-4">
              <button
                  onClick={() => setActiveTab('items')}
                  className={`px-4 py-2 flex items-center gap-2 border-b-2 transition-colors ${
                      activeTab === 'items'
                          ? 'border-gray-400 text-white'
                          : 'border-transparent text-gray-500 hover:text-gray-300'
                  }`}
              >
                <Package className="w-4 h-4" />
                Play.Items
              </button>
              <button
                  onClick={() => setActiveTab('world')}
                  className={`px-4 py-2 flex items-center gap-2 border-b-2 transition-colors ${
                      activeTab === 'world'
                          ? 'border-gray-400 text-white'
                          : 'border-transparent text-gray-500 hover:text-gray-300'
                  }`}
              >
                <Map className="w-4 h-4" />
                Play.World
              </button>
              <button
                  onClick={() => setActiveTab('users')}
                  className={`px-4 py-2 flex items-center gap-2 border-b-2 transition-colors ${
                      activeTab === 'users'
                          ? 'border-gray-400 text-white'
                          : 'border-transparent text-gray-500 hover:text-gray-300'
                  }`}
              >
                <Map className="w-4 h-4" />
                Play.Users
              </button>
            </div>
          </div>

          {/* Content */}
          {activeTab === 'items' && <PlayItems />}
          {activeTab === 'world' && <PlayWorld />}
          {activeTab === 'users' && <PlayUsers />}
        </div>
      </div>
  );
}

export default App;