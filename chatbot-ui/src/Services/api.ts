export async function sendMessage(conversationId: string, message: string): Promise<string> {

    const response = await fetch(`https://localhost:7180/api/chat?userId=user&conversationId=${conversationId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(message)
    });

    if (!response.ok) {
        throw new Error('API Error');
    }

    return await response.text();
}

export async function getMessages(conversationId: string) {

    const response = await fetch(
        `https://localhost:7180/api/chat?conversationId=${conversationId}`
    );

    return await response.json();
}