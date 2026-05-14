<script lang="ts">
    import MessageList from './MessageList.svelte';
    import ChatInput from './ChatInput.svelte';
    import { sendMessage, getMessages } from '../Services/api';
    import type { ChatMessage } from '../Types/chat';
    import { onMount } from 'svelte';

    let messages: ChatMessage[] = [];

    let loading = false;

    let conversationId =
    sessionStorage.getItem("conversationId");

    if (!conversationId) {
        conversationId = crypto.randomUUID().toString();
        sessionStorage.setItem(
            "conversationId",
            conversationId
        );
    }

    onMount(async () => {
        const oldMessages =
            await getMessages(conversationId);
        messages = oldMessages.map((x: any) => ({
            role: x.role,
            text: x.content
        }));
    });

    function startNewChat() {

    conversationId =
        crypto.randomUUID().toString();

    sessionStorage.setItem(
        "conversationId",
        conversationId
    );

    messages = [];
}

    async function handleSend(event: CustomEvent<string>) {

        const text = event.detail;

        if (!text.trim() || loading) {
            return;
        }

        messages = [
            ...messages,
            {
                role: 'user',
                text
            }
        ];

        loading = true;

        try {
            const reply = await sendMessage(conversationId, text);
            speakMessage(reply);

            messages = [
                ...messages,
                {
                    role: 'assistant',
                    text: reply
                }
            ];
        }
        catch {
            messages = [
                ...messages,
                {
                    role: 'assistant',
                    text: 'Something went wrong.'
                }
            ];
        }
        loading = false;
    }

   function speakMessage(text: string) {

    if (window.speechSynthesis.speaking) {
        return;
    }

    const speech = new SpeechSynthesisUtterance(text);

    speech.lang = 'en-US';
    speech.rate = 1;
    speech.pitch = 1;

    speech.onstart = () => {
        console.log('Speech started');
    };

    speech.onend = () => {
        console.log('Speech ended');
    };

    speech.onerror = (e) => {
        console.error('Speech error', e);
    };

    window.speechSynthesis.speak(speech);
}
</script>

    <div class="chat-layout">
        <MessageList
            {messages}
            {loading}
        />
        <ChatInput
            on:send={handleSend}
        />
    </div>


<style>

    .chat-layout {
        flex: 1;
        display: flex;
        flex-direction: column;
        background: #0f172a;
        height: 100vh;
    }

</style>