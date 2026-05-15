<script lang="ts">
    import { createEventDispatcher } from 'svelte';
    let message = '';
    const dispatch = createEventDispatcher();

    function sendMessage() {
        if (!message.trim()) {
            return;
        }
        dispatch('send', message);
        message = '';
    }

    function handleKeyDown(
        event: KeyboardEvent
    ) {
        if ( event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            sendMessage();
        }
    }

    function startVoiceRecognition() {
        const SpeechRecognition =
            (window as any)
                .SpeechRecognition ||
            (window as any)
                .webkitSpeechRecognition;

        if (!SpeechRecognition) {
            alert(
                'Speech recognition not supported'
            );
            return;
        }

        const recognition =
            new SpeechRecognition();
        recognition.lang = 'en-US';
        recognition.start();

        recognition.onresult =
            (event: any) => {
                const transcript =
                    event.results[0][0]
                        .transcript;

                message = transcript;
                sendMessage();
            };
    }
</script>

<div class="input-container">

    <textarea
        bind:value={message}
        placeholder="Type your message..."
        on:keydown={handleKeyDown}
    />

    <button
        class="mic-btn"
        on:click={startVoiceRecognition}
    >
        🎤
    </button>

    <button
        class="send-btn"
        on:click={sendMessage}
    >
        Send
    </button>

</div>

<style>
    .input-container {
        display: flex;
        align-items: center;
        gap: 12px;
        width: 100%;
    }

    textarea {
        flex: 1;
        height: 52px;
        padding: 14px 16px;
        border-radius: 14px;
        border: 1px solid #334155;
        background: #0f172a;
        color: white;
        resize: none;
        outline: none;
        font-size: 14px;
        box-sizing: border-box;
    }

    .mic-btn,
    .send-btn {
        height: 52px;
        padding: 0 18px;

        border: none;
        border-radius: 12px;
        background: #2563eb;
        color: white;
        cursor: pointer;
        flex-shrink: 0;
    }
</style>