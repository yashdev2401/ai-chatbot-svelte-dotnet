<script lang="ts">

    import MessageList from './MessageList.svelte';
    import ChatInput from './ChatInput.svelte';

    import { sendMessage } from '../Services/api';

    import type { ChatMessage } from '../Types/chat';

    let messages: ChatMessage[] = [];

    let loading = false;

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

            const reply = await sendMessage(text);

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
</script>

<div class="chat-layout">

    <MessageList
        {messages}
        {loading}
    />

   <ChatInput
    on:send={handleSend}
    {loading}
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