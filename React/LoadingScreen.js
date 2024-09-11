import React from 'react';
import './LoadingScreen.css';

const LoadingScreen = () => (
  <div id="unity-loading-bar">
    <div id="unity-logo"></div>
    <div id="unity-progress-bar-empty">
      <div id="unity-progress-bar-full"></div>
    </div>
  </div>
);

export default LoadingScreen;
