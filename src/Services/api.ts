export async function sendMessage(message: string): Promise<string> {

    const response = await fetch('https://localhost:7180/api/chat?userId=user', {
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