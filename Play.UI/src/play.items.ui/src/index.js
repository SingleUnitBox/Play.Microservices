import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';  // ← This imports the CSS file
import App from './App';
import "leaflet/dist/leaflet.css";


const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);