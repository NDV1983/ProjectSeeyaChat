import React, { useEffect, useRef, useState } from 'react';
import './App.css';
import LoadingScreen from './LoadingScreen';
import Chat from './Chat';
import Emoji from './Emoji';
import chatHistoryIcon from './chathistory.png'; // Import the chat history icon

function UnityApp() {
    const unityInstanceRef = useRef(null);
    const [loading, setLoading] = useState(true);
    const [chatHistory, setChatHistory] = useState([]);
    const chatInputRef = useRef(null);
    const playerNameInputRef = useRef(null);
    const [isMobile, setIsMobile] = useState(false);
    const [isChatMinimized, setIsChatMinimized] = useState(true); // State for chat minimization

    useEffect(() => {
        const isMobileDevice = /iPhone|iPad|iPod|Android/i.test(navigator.userAgent);
        setIsMobile(isMobileDevice); // Set the state based on the device type

        window.ReceiveChatHistory = (jsonChatHistory) => {
            try {
                const parsedData = JSON.parse(jsonChatHistory);
                if (parsedData.success && parsedData.chatHistory) {
                    const chatHistoryData = JSON.parse(parsedData.chatHistory);
                    if (Array.isArray(chatHistoryData)) {
                        setChatHistory(chatHistoryData);
                    } else {
                        console.error('Chat history is not an array:', chatHistoryData);
                    }
                } else {
                    console.error('Received chat history is not in expected format:', parsedData);
                }
            } catch (error) {
                console.error('Error parsing chat history JSON:', error);
            }
        };

        const createUnityCanvas = () => {
            const unityContainer = document.getElementById('unityContainer');
            if (unityContainer) {
                unityContainer.innerHTML = '<canvas id="unity-canvas" width="960" height="540" tabIndex="-1"></canvas>';
                const canvas = document.getElementById('unity-canvas');
                return canvas;
            }
            return null;
        };

        const resizeCanvas = () => {
            const canvas = document.getElementById('unity-canvas');
            if (canvas) {
                if (isMobileDevice) {
                    // Set lower resolution for mobile
                    const scaleFactor = 0.5; // Adjust this factor as needed to balance performance and quality
                    canvas.style.width = '100vw';
                    canvas.style.height = '100vh';
                    canvas.width = window.innerWidth * scaleFactor;
                    canvas.height = window.innerHeight * scaleFactor;
                } else {
                    canvas.style.width = '100%';
                    canvas.style.height = '100%';
                    canvas.width = window.innerWidth;  // Full resolution
                    canvas.height = window.innerHeight;  // Full resolution
                }
            }
        };

        window.addEventListener('resize', resizeCanvas);
        const canvas = createUnityCanvas();
        resizeCanvas();

        if (canvas && !unityInstanceRef.current) {
            const buildUrl = "Build";
            const config = {
                dataUrl: buildUrl + "/SeeyaChat.data",
                frameworkUrl: buildUrl + "/SeeyaChat.framework.js",
                codeUrl: buildUrl + "/SeeyaChat.wasm",
                streamingAssetsUrl: "StreamingAssets",
                companyName: "DefaultCompany",
                productName: "SeeyaChat",
                productVersion: "0.1.0",
                showBanner: unityShowBanner,
            };

            function unityShowBanner(msg, type) {
                const warningBanner = document.getElementById('unity-warning');
                if (!warningBanner) return;

                function updateBannerVisibility() {
                    warningBanner.style.display = warningBanner.children.length ? 'block' : 'none';
                }
                const div = document.createElement('div');
                div.innerHTML = msg;
                warningBanner.appendChild(div);
                if (type === 'error') div.style = 'background: red; padding: 10px;';
                else if (type === 'warning') div.style = 'background: yellow; padding: 10px;';
                setTimeout(() => {
                    warningBanner.removeChild(div);
                    updateBannerVisibility();
                }, 5000);
                updateBannerVisibility();
            }

            const initializeUnity = () => {
                if (typeof window.createUnityInstance !== 'undefined') {
                    window.createUnityInstance(canvas, config, (progress) => {
                        const progressBarFull = document.getElementById('unity-progress-bar-full');
                        if (progressBarFull) {
                            progressBarFull.style.width = 100 * progress + '%';
                        }
                    }).then((instance) => {
                        unityInstanceRef.current = instance;
                        setLoading(false);
                        const fullscreenButton = document.getElementById('unity-fullscreen-button');
                        if (fullscreenButton) {
                            fullscreenButton.onclick = () => {
                                instance.SetFullscreen(1);
                            };
                        }
                        window.SendMessage = (gameObject, method, message) => {
                            if (unityInstanceRef.current) {
                                unityInstanceRef.current.SendMessage(gameObject, method, message);
                            } else {
                                console.error('Unity instance is not fully initialized yet.');
                            }
                        };
                    }).catch((message) => {
                        alert(message);
                    });
                } else {
                    console.error("createUnityInstance function is not available. Ensure UnityLoader script is loaded.");
                }
            };

            if (typeof window.createUnityInstance === 'undefined') {
                const script = document.createElement('script');
                script.src = buildUrl + '/SeeyaChat.loader.js';
                script.onload = initializeUnity;
                document.body.appendChild(script);
            } else {
                initializeUnity();
            }
        }

        const handleFocus = () => {
            document.body.style.height = '100vh'; // Prevents body height adjustment
            document.body.style.overflow = 'hidden'; // Prevents body overflow
        };

        const handleBlur = () => {
            document.body.style.height = ''; // Reset body height
            document.body.style.overflow = ''; // Reset body overflow
        };

        const inputField = document.getElementById('chatMessageInput');
        if (inputField) {
            inputField.addEventListener('focus', handleFocus);
            inputField.addEventListener('blur', handleBlur);
        }

        return () => {
            window.removeEventListener('resize', resizeCanvas);
            if (unityInstanceRef.current) {
                unityInstanceRef.current.Quit().then(() => {
                    unityInstanceRef.current = null;
                    createUnityCanvas();
                });
            }
            if (inputField) {
                inputField.removeEventListener('focus', handleFocus);
                inputField.removeEventListener('blur', handleBlur);
            }
        };
    }, []);

    const handleSendMessage = (method, inputSelector) => {
        const inputField = document.querySelector(inputSelector);
        if (inputField) {
            const message = inputField.value;
            if (unityInstanceRef.current) {
                try {
                    window.SendMessage('Network', method, message);
                    console.log(`Message sent to Unity (${method}):`, message);
                    inputField.blur(); // Hide the keyboard after sending the message
                    if (inputSelector === '#chatMessageInput') {
                        inputField.value = ''; // Clear the chat input field after sending
                    }
                } catch (error) {
                    console.error(`Error calling SendMessage (${method}):`, error);
                }
            } else {
                console.error('Unity instance is not initialized.');
            }
        } else {
            console.error('Input field not found.');
        }
    };

    const handleKeyPress = (event, method, inputSelector) => {
        if (event.key === 'Enter' || event.key === 'Send') { // Detect Enter or Send key
            handleSendMessage(method, inputSelector);
            event.preventDefault(); // Prevent the default behavior of the Enter key
        }
    };

    const handleEmojiSelect = (emoji) => {
        if (chatInputRef.current) {
            chatInputRef.current.value += emoji;
            chatInputRef.current.focus();
        }
    };

    const toggleChat = () => {
        setIsChatMinimized(!isChatMinimized);
    };

    return (
        <div className="App">
            {loading && <LoadingScreen />}
            <div id="unityContainer"></div>
            <header className="App-header">
                <div id="setting-section" className={isMobile ? 'mobile-only' : ''}>
                    <input
                        type="text"
                        placeholder="Enter player name..."
                        className="player-name-input"
                        onKeyPress={(event) => handleKeyPress(event, 'SetPlayerNameFromJS', '#playerNameInput')}
                        id="playerNameInput"
                        ref={playerNameInputRef}
                        enterkeyhint="send" // Hint to the keyboard to show a "Send" button
                    />
                </div>
                <div id="input-container">
                    <input
                        type="text"
                        placeholder="Enter chat message..."
                        className="input-field"
                        onKeyPress={(event) => handleKeyPress(event, 'SetChatFromJS', '#chatMessageInput')}
                        id="chatMessageInput"
                        ref={chatInputRef}
                        enterkeyhint="send"  // Hint to the keyboard to show a "Send" button
                    />
                    <button className="button" onClick={() => handleSendMessage('SetChatFromJS', '#chatMessageInput')}>Send</button>
                    <Emoji onSelectEmoji={handleEmojiSelect} /> {/* Add Emoji component */}
                </div>
            </header>
            <Chat messages={chatHistory} isMinimized={isChatMinimized} toggleChat={toggleChat} />
            <button className="history-button" onClick={toggleChat}>
                <img src={chatHistoryIcon} alt="Chat History" />
            </button>
        </div>
    );
}

export default UnityApp;
