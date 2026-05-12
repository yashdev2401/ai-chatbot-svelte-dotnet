<script lang="ts">

    import { createEventDispatcher } from 'svelte';
    export let loading = false;

    let text = '';

    const dispatch = createEventDispatcher();


    function send() {

        if (!text.trim()) {
            return;
        }

        dispatch('send', text);

        text = '';
    }
</script>

<div class="input-container">

    <input
        bind:value={text}
        placeholder="Type your message..."
        disabled={loading}
        on:keydown={(e) => e.key === 'Enter' && send()}
    />

    <button
    on:click={send}
    disabled={loading}>
    {loading ? 'Thinking...' : 'Send'}
    </button>

</div>

<style>
    .input-container {
        display: flex;
        gap: 12px;
        padding: 20px;
        border-top: 1px solid #374151;
        background: #111827;
        position: sticky;
        bottom: 0;
    }

    input {
        flex: 1;
        padding: 16px;
        border-radius: 14px;
        border: 1px solid #374151;
        background: #1f2937;
        color: white;
        font-size: 15px;
        outline: none;
    }

    input:focus {
        border-color: #2563eb;
    }

    button {
        padding: 14px 24px;
        border: none;
        border-radius: 14px;
        background: linear-gradient(135deg, #2563eb, #1d4ed8);
        color: white;
        font-weight: 600;
        cursor: pointer;
    }

    button:hover {
        opacity: 0.9;
    }
</style>