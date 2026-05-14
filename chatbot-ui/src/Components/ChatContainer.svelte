<script lang="ts">
    import MessageList from './MessageList.svelte';
    import ChatInput from './ChatInput.svelte';
    import { sendMessage, getMessages } from '../Services/api';
    import type { ChatMessage } from '../Types/chat';
    import { onMount } from 'svelte';

    let messages: ChatMessage[] = [];

    let loading = false;

    let conversationId =
    localStorage.getItem("conversationId");

    if (!conversationId) {
        conversationId = crypto.randomUUID().toString();
        localStorage.setItem(
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