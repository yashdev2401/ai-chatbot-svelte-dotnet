<script lang="ts">
    import { createEventDispatcher } from 'svelte';
    export let loading = false;
    let message = '';
    const dispatch = createEventDispatcher();

    function sendMessage() {

        if (!message.trim()) {
            return;
        }
        dispatch('send', message);
        message = '';
    }

    function handleKeyDown(event: KeyboardEvent) {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            sendMessage();
        }
    }

    function startVoiceRecognition() {
        const SpeechRecognition =
            window.SpeechRecognition ||
            window.webkitSpeechRecognition;
        if (!SpeechRecognition) {
            alert('Speech recognition not supported');
            return;
        }
        const recognition = new SpeechRecognition();
        recognition.lang = 'en-US';
        recognition.interimResults = false;
        recognition.maxAlternatives = 1;
        recognition.start();

        recognition.onresult = (event: any) => {
            const transcript = event.results[0][0].transcript;
            message = transcript;
            sendMessage();
        };

        recognition.onerror = (event: any) => {
            console.error('Speech error', event);
        };
    }
</script>

<div class="input-container">

    <textarea
        bind:value={message}
        placeholder="Type your message..."
        disabled={loading}
        on:keydown={handleKeyDown}
        rows="1"
    />

    <button
        class="mic-btn"
        on:click={startVoiceRecognition}
        disabled={loading}
    >
        🎤
    </button>

    <button
        class="send-btn"
        on:click={sendMessage}
        disabled={loading}
    >
        {loading ? '...' : 'Send'}
    </button>

</div>

<style>
    .input-container {
        display: flex;
        align-items: flex-end;
        gap: 10px;
        padding: 12px;
        width: 100%;
    }

    textarea {
        flex: 1;
        min-height: 52px;
        max-height: 160px;
        resize: none;
        border-radius: 12px;
        border: 1px solid #243047;
        background: #0b1220;
        color: white;
        padding: 14px;
        font-size: 14px;
        outline: none;
        overflow-y: auto;
    }

    .mic-btn,
    .send-btn {
        height: 52px;
        border: none;
        border-radius: 12px;
        background: #2563eb;
        color: white;
        cursor: pointer;
        flex-shrink: 0;
    }

    .mic-btn {
        width: 52px;
        font-size: 18px;
    }

    .send-btn {
        padding: 0 20px;
        font-weight: 600;
    }
</style>