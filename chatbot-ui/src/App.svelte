<script lang="ts">
    import ChatContainer from './Components/ChatContainer.svelte';

    let refreshKey = 0;
    let loading = false;
    function startNewChat() {
        if (loading) {
            return;
        }
        sessionStorage.removeItem('conversationId');
        refreshKey += 1;
    }
</script>

<div class="app">

    <div class="sidebar">
        <button
            class="new-chat-btn"
            disabled={loading}
            on:click={startNewChat}
        >
            + New Chat
        </button>
    </div>
    <div class="chat-layout">

        {#key refreshKey}
            <ChatContainer bind:loading
            />
        {/key}
    </div>

</div>