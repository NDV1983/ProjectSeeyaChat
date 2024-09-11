import React, { useState } from 'react';
import './App.css';

const emojis = ['😀', '😂', '😅', '😍', '😎'];

const Emoji = ({ onSelectEmoji }) => {
    const [showEmojis, setShowEmojis] = useState(false);

    const toggleEmojiMenu = () => {
        setShowEmojis(!showEmojis);
    };

    return (
        <div className="emoji-container">
            <button className="emoji-toggle-button" onClick={toggleEmojiMenu}>
                😊
            </button>
            {showEmojis && (
                <div className="emoji-menu">
                    {emojis.map((emoji, index) => (
                        <button
                            key={index}
                            className="emoji-button"
                            onClick={() => {
                                onSelectEmoji(emoji);
                                setShowEmojis(false);
                            }}
                        >
                            {emoji}
                        </button>
                    ))}
                </div>
            )}
        </div>
    );
};

export default Emoji;
