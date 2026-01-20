import React, { useState, useEffect } from 'react';
import { UserPlus, LogIn, Users, RefreshCw, Eye, EyeOff, X, CheckCircle } from 'lucide-react';

const API_BASE_URL = 'http://localhost:5008/play-user';

export default function PlayUsers() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [accessToken, setAccessToken] = useState('');
  const [showSignUpModal, setShowSignUpModal] = useState(false);
  const [showSignInModal, setShowSignInModal] = useState(false);
  const [showPasswordSignUp, setShowPasswordSignUp] = useState(false);
  const [showPasswordSignIn, setShowPasswordSignIn] = useState(false);

  const [signUpForm, setSignUpForm] = useState({
    username: '',
    email: '',
    password: '',
    role: 'user',
    policies: ['items', 'inventory']
  });

  const [signInForm, setSignInForm] = useState({
    email: '',
    password: ''
  });

  const [signUpLoading, setSignUpLoading] = useState(false);
  const [signInLoading, setSignInLoading] = useState(false);
  const [signUpError, setSignUpError] = useState(null);
  const [signInError, setSignInError] = useState(null);
  const [signUpSuccess, setSignUpSuccess] = useState(false);

  const fetchUsers = async () => {
    if (!accessToken) {
      console.log('No access token available');
      return;
    }

    setLoading(true);
    try {
      const response = await fetch(API_BASE_URL, {
        headers: {
          'Authorization': `Bearer ${accessToken}`
        }
      });

      if (response.ok) {
        const data = await response.json();
        setUsers(data.users || data || []);
      } else if (response.status === 401) {
        setAccessToken('');
        alert('Session expired. Please sign in again.');
      }
    } catch (err) {
      console.error('Error fetching users:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSignUp = async () => {
    setSignUpLoading(true);
    setSignUpError(null);
    setSignUpSuccess(false);

    try {
      const response = await fetch(`${API_BASE_URL}/signUp`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          username: signUpForm.username,
          email: signUpForm.email,
          password: signUpForm.password,
          role: signUpForm.role,
          claims: {
            policies: signUpForm.policies
          }
        })
      });

      if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || `HTTP ${response.status}`);
      }

      setSignUpSuccess(true);
      setSignUpForm({
        username: '',
        email: '',
        password: '',
        role: 'user',
        policies: ['items', 'inventory']
      });

      setTimeout(() => {
        setShowSignUpModal(false);
        setSignUpSuccess(false);
      }, 2000);
    } catch (err) {
      setSignUpError(err.message);
    } finally {
      setSignUpLoading(false);
    }
  };

  const handleSignIn = async () => {
    setSignInLoading(true);
    setSignInError(null);

    try {
      const response = await fetch(`${API_BASE_URL}/signIn`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email: signInForm.email,
          password: signInForm.password
        })
      });

      if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || `HTTP ${response.status}`);
      }

      const data = await response.json();
      const token = data.accessToken || data.token;

      if (token) {
        setAccessToken(token);
        setSignInForm({ email: '', password: '' });
        setShowSignInModal(false);
        setTimeout(() => fetchUsers(), 100);
      } else {
        throw new Error('No access token received');
      }
    } catch (err) {
      setSignInError(err.message);
    } finally {
      setSignInLoading(false);
    }
  };

  const handleSignOut = () => {
    setAccessToken('');
    setUsers([]);
  };

  const togglePolicy = (policy) => {
    setSignUpForm(prev => ({
      ...prev,
      policies: prev.policies.includes(policy)
          ? prev.policies.filter(p => p !== policy)
          : [...prev.policies, policy]
    }));
  };

  return (
      <div className="min-h-screen bg-gray-950 text-white p-6">
        {/* Header */}
        <div className="bg-gray-900 rounded-lg p-6 mb-6 border border-gray-800">
          <div className="flex justify-between items-center">
            <div>
              <h2 className="text-2xl font-bold mb-2">User Management</h2>
              <p className="text-sm text-gray-400">
                {accessToken ? `Signed in • ${users.length} users` : 'Not signed in'}
              </p>
            </div>

            <div className="flex gap-3">
              {!accessToken ? (
                  <>
                    <button
                        onClick={() => setShowSignUpModal(true)}
                        className="flex items-center gap-2 px-4 py-2 bg-green-600 hover:bg-green-700 text-white rounded-lg transition-colors"
                    >
                      <UserPlus className="w-4 h-4" />
                      Sign Up
                    </button>
                    <button
                        onClick={() => setShowSignInModal(true)}
                        className="flex items-center gap-2 px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
                    >
                      <LogIn className="w-4 h-4" />
                      Sign In
                    </button>
                  </>
              ) : (
                  <>
                    <button
                        onClick={fetchUsers}
                        disabled={loading}
                        className="flex items-center gap-2 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg transition-colors"
                    >
                      <RefreshCw className={`w-4 h-4 ${loading ? 'animate-spin' : ''}`} />
                      Refresh
                    </button>
                    <button
                        onClick={handleSignOut}
                        className="flex items-center gap-2 px-4 py-2 bg-red-600 hover:bg-red-700 text-white rounded-lg transition-colors"
                    >
                      Sign Out
                    </button>
                  </>
              )}
            </div>
          </div>
        </div>

        {/* Users List */}
        {accessToken ? (
            <div className="bg-gray-900 rounded-lg border border-gray-800 p-6">
              <h3 className="font-semibold mb-4 flex items-center gap-2">
                <Users className="w-5 h-5" />
                Registered Users
              </h3>

              {loading ? (
                  <div className="text-center py-8 text-gray-400">
                    <RefreshCw className="w-6 h-6 animate-spin mx-auto mb-2" />
                    Loading users...
                  </div>
              ) : users.length === 0 ? (
                  <div className="text-center py-8 text-gray-400">
                    No users found
                  </div>
              ) : (
                  <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                    {users.map((user, idx) => (
                        <div
                            key={user.id || idx}
                            className="p-4 bg-gray-800 rounded hover:bg-gray-750"
                        >
                          <div className="flex items-start justify-between mb-2">
                            <div>
                              <h4 className="font-medium text-lg">{user.username || 'Unknown'}</h4>
                              <p className="text-sm text-gray-400">{user.email}</p>
                            </div>
                            <span className="text-xs px-2 py-1 bg-blue-900 text-blue-200 rounded">
                      {user.role || 'user'}
                    </span>
                          </div>

                          {user.claims && user.claims.policies && (
                              <div className="mt-3">
                                <p className="text-xs text-gray-500 mb-1">Policies:</p>
                                <div className="flex flex-wrap gap-1">
                                  {user.claims.policies.map((policy, pIdx) => (
                                      <span
                                          key={pIdx}
                                          className="text-xs px-2 py-0.5 bg-gray-700 text-gray-300 rounded"
                                      >
                            {policy}
                          </span>
                                  ))}
                                </div>
                              </div>
                          )}

                          {user.id && (
                              <p className="text-xs text-gray-600 mt-2">ID: {user.id}</p>
                          )}
                        </div>
                    ))}
                  </div>
              )}
            </div>
        ) : (
            <div className="bg-gray-900 rounded-lg border border-gray-800 p-12 text-center">
              <Users className="w-16 h-16 mx-auto mb-4 text-gray-600" />
              <h3 className="text-xl font-semibold mb-2">Sign In Required</h3>
              <p className="text-gray-400 mb-6">
                Please sign in to view and manage users
              </p>
              <div className="flex gap-3 justify-center">
                <button
                    onClick={() => setShowSignUpModal(true)}
                    className="flex items-center gap-2 px-6 py-3 bg-green-600 hover:bg-green-700 text-white rounded-lg transition-colors"
                >
                  <UserPlus className="w-5 h-5" />
                  Create Account
                </button>
                <button
                    onClick={() => setShowSignInModal(true)}
                    className="flex items-center gap-2 px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors"
                >
                  <LogIn className="w-5 h-5" />
                  Sign In
                </button>
              </div>
            </div>
        )}

        {/* Sign Up Modal */}
        {showSignUpModal && (
            <div className="fixed inset-0 bg-black/90 flex items-center justify-center p-4 z-50">
              <div className="bg-gray-900 rounded-lg max-w-md w-full border border-gray-800">
                <div className="p-6">
                  <div className="flex justify-between items-start mb-6">
                    <div>
                      <h2 className="text-2xl font-bold text-white">Sign Up</h2>
                      <p className="text-gray-400 text-sm mt-1">Create a new account</p>
                    </div>
                    <button
                        onClick={() => {
                          setShowSignUpModal(false);
                          setSignUpError(null);
                          setSignUpSuccess(false);
                        }}
                        className="text-gray-400 hover:text-gray-300"
                    >
                      <X className="w-6 h-6" />
                    </button>
                  </div>

                  {signUpSuccess && (
                      <div className="mb-4 bg-green-900/50 border border-green-500 rounded-lg p-3 flex items-center gap-2">
                        <CheckCircle className="w-5 h-5 text-green-400" />
                        <p className="text-green-200 text-sm">Account created successfully!</p>
                      </div>
                  )}

                  {signUpError && (
                      <div className="mb-4 bg-red-900/50 border border-red-500 rounded-lg p-3">
                        <p className="text-red-200 text-sm">{signUpError}</p>
                      </div>
                  )}

                  <div className="space-y-4">
                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Username *</label>
                      <input
                          type="text"
                          required
                          value={signUpForm.username}
                          onChange={(e) => setSignUpForm({...signUpForm, username: e.target.value})}
                          className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-blue-500 outline-none"
                          placeholder="dragonslayer42"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Email *</label>
                      <input
                          type="email"
                          required
                          value={signUpForm.email}
                          onChange={(e) => setSignUpForm({...signUpForm, email: e.target.value})}
                          className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-blue-500 outline-none"
                          placeholder="user@play.com"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Password *</label>
                      <div className="relative">
                        <input
                            type={showPasswordSignUp ? "text" : "password"}
                            required
                            value={signUpForm.password}
                            onChange={(e) => setSignUpForm({...signUpForm, password: e.target.value})}
                            className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-blue-500 outline-none pr-10"
                            placeholder="••••••••"
                        />
                        <button
                            type="button"
                            onClick={() => setShowPasswordSignUp(!showPasswordSignUp)}
                            className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-300"
                        >
                          {showPasswordSignUp ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                        </button>
                      </div>
                    </div>

                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Role</label>
                      <select
                          value={signUpForm.role}
                          onChange={(e) => setSignUpForm({...signUpForm, role: e.target.value})}
                          className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-blue-500 outline-none"
                      >
                        <option value="user">User</option>
                        <option value="admin">Admin</option>
                        <option value="moderator">Moderator</option>
                      </select>
                    </div>

                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Policies</label>
                      <div className="space-y-2">
                        {['items', 'inventory', 'zones', 'trade'].map(policy => (
                            <label key={policy} className="flex items-center gap-2 cursor-pointer">
                              <input
                                  type="checkbox"
                                  checked={signUpForm.policies.includes(policy)}
                                  onChange={() => togglePolicy(policy)}
                                  className="w-4 h-4 rounded border-gray-700 bg-gray-800 text-blue-600 focus:ring-blue-500"
                              />
                              <span className="text-sm text-gray-300">{policy}</span>
                            </label>
                        ))}
                      </div>
                    </div>

                    <div className="flex gap-3 pt-4">
                      <button
                          onClick={() => {
                            setShowSignUpModal(false);
                            setSignUpError(null);
                            setSignUpSuccess(false);
                          }}
                          className="flex-1 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg"
                      >
                        Cancel
                      </button>
                      <button
                          onClick={handleSignUp}
                          disabled={signUpLoading}
                          className="flex-1 px-4 py-2 bg-green-600 hover:bg-green-700 text-white rounded-lg disabled:opacity-50"
                      >
                        {signUpLoading ? 'Creating...' : 'Sign Up'}
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
        )}

        {/* Sign In Modal */}
        {showSignInModal && (
            <div className="fixed inset-0 bg-black/90 flex items-center justify-center p-4 z-50">
              <div className="bg-gray-900 rounded-lg max-w-md w-full border border-gray-800">
                <div className="p-6">
                  <div className="flex justify-between items-start mb-6">
                    <div>
                      <h2 className="text-2xl font-bold text-white">Sign In</h2>
                      <p className="text-gray-400 text-sm mt-1">Access your account</p>
                    </div>
                    <button
                        onClick={() => {
                          setShowSignInModal(false);
                          setSignInError(null);
                        }}
                        className="text-gray-400 hover:text-gray-300"
                    >
                      <X className="w-6 h-6" />
                    </button>
                  </div>

                  {signInError && (
                      <div className="mb-4 bg-red-900/50 border border-red-500 rounded-lg p-3">
                        <p className="text-red-200 text-sm">{signInError}</p>
                      </div>
                  )}

                  <div className="space-y-4">
                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Email *</label>
                      <input
                          type="email"
                          required
                          value={signInForm.email}
                          onChange={(e) => setSignInForm({...signInForm, email: e.target.value})}
                          className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-blue-500 outline-none"
                          placeholder="user@play.com"
                      />
                    </div>

                    <div>
                      <label className="block text-gray-400 text-sm font-semibold mb-2">Password *</label>
                      <div className="relative">
                        <input
                            type={showPasswordSignIn ? "text" : "password"}
                            required
                            value={signInForm.password}
                            onChange={(e) => setSignInForm({...signInForm, password: e.target.value})}
                            className="w-full px-4 py-2 bg-black text-white rounded-lg border border-gray-800 focus:border-blue-500 outline-none pr-10"
                            placeholder="••••••••"
                        />
                        <button
                            type="button"
                            onClick={() => setShowPasswordSignIn(!showPasswordSignIn)}
                            className="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-300"
                        >
                          {showPasswordSignIn ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                        </button>
                      </div>
                    </div>

                    <div className="flex gap-3 pt-4">
                      <button
                          onClick={() => {
                            setShowSignInModal(false);
                            setSignInError(null);
                          }}
                          className="flex-1 px-4 py-2 bg-gray-800 hover:bg-gray-700 text-white rounded-lg"
                      >
                        Cancel
                      </button>
                      <button
                          onClick={handleSignIn}
                          disabled={signInLoading}
                          className="flex-1 px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg disabled:opacity-50"
                      >
                        {signInLoading ? 'Signing In...' : 'Sign In'}
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
        )}
      </div>
  );
}