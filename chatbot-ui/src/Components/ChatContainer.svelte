<script lang="ts">
    import { onMount, tick } from 'svelte';
    import MessageList from './MessageList.svelte';
    import ChatInput from './ChatInput.svelte';

    import {
        sendMessage,
        getMessages
    } from '../Services/api';

    let messages: any[] = [];

    let loading = false;

    let messagesContainer: HTMLDivElement;

    let conversationId = sessionStorage.getItem('conversationId');

    if (!conversationId) {
        conversationId =crypto.randomUUID();

        sessionStorage.setItem(
            'conversationId',
            conversationId
        );
    }

    onMount(async () => {
        const oldMessages =
            await getMessages(conversationId!);

        messages = oldMessages.map((x: any) => ({
            role: x.role,
            text: x.content
        }));
        await tick();
        scrollToBottom();
    });

    function scrollToBottom() {
        if (messagesContainer) {
            messagesContainer.scrollTop =
                messagesContainer.scrollHeight;
        }
    }

    function speakMessage(text: string) {
        window.speechSynthesis.cancel();

        const speech =
            new SpeechSynthesisUtterance(text);

        speech.lang = 'en-US';
        window.speechSynthesis.speak(speech);
    }

    async function handleSend(
        event: CustomEvent<string>
    ) {
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

        await tick();
        scrollToBottom();
        loading = true;

        try {
            const reply =
                await sendMessage(
                    conversationId!,
                    text
                );

            messages = [
                ...messages,
                {
                    role: 'assistant',
                    text: reply
                }
            ];
            await tick();
            scrollToBottom();
            speakMessage(reply);
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

<div class="chat-container">

    <div
        bind:this={messagesContainer}
        class="messages-wrapper"
    >
        <MessageList
            {messages}
            {loading}
        />
    </div>
    <div class="input-wrapper">
        <ChatInput
            on:send={handleSend}
        />
    </div>
</div>

<style>
    .chat-container {
        flex: 1;
        display: flex;
        flex-direction: column;
        height: 100vh;
        overflow: hidden;
        background: #020617;
    }
    .messages-wrapper {
        flex: 1;
        overflow-y: auto;
        padding: 24px;
        min-height: 0;
    }

    .input-wrapper {
        padding: 16px 24px;
        border-top: 1px solid #1e293b;
        background: #020617;
    }
</style>