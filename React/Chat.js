import React, { useEffect, useRef } from 'react';

const Chat = ({ messages, isMinimized, toggleChat }) => {
    const messageEndRef = useRef(null);

    useEffect(() => {
        if (!isMinimized) {
            messageEndRef.current?.scrollIntoView({ behavior: 'smooth' });
        }
    }, [messages, isMinimized]);

    if (!Array.isArray(messages)) {
        return <div>Error: Messages is not an array</div>;
    }

    return (
        <div id="chat-container" className={isMinimized ? 'minimized' : ''}>
            <button className="minimize-button" onClick={toggleChat}>
                {isMinimized ? '' : ''}
            </button>
            {isMinimized ? (
                <div className="minimized-title"></div>
            ) : (
                <>
                    <div className="chat-history">
                        {messages.map((msg, index) => (
                            <div key={index} className="chat-message">
                                <span>{msg.message}</span>
                            </div>
                        ))}
                        <div ref={messageEndRef} />
                    </div>
                </>
            )}
        </div>
    );
};

export default Chat;
