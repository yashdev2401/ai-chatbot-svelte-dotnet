<script lang="ts">
    import ChatContainer from './Components/ChatContainer.svelte';

    let refreshKey = 0;
    function startNewChat() {
        sessionStorage.removeItem('conversationId');
        refreshKey += 1;
    }
</script>

<div class="app-layout">

    <div class="sidebar">
        <button
            class="new-chat-btn"
            on:click={startNewChat}
        >
            + New Chat
        </button>
    </div>

   {#key refreshKey}
    <ChatContainer />
    {/key}
</div>

<style>
    .app-layout {
        display: flex;
        height: 100vh;
        overflow: hidden;
        background: #020617;
    }
    .sidebar {
        width: 220px;
        background: #0f172a;
        border-right: 1px solid #1e293b;
        padding: 14px;
        box-sizing: border-box;
    }
    .new-chat-btn {
        width: 100%;
        padding: 12px;
        border: none;
        border-radius: 12px;
        background: #2563eb;
        color: white;
        cursor: pointer;
        font-weight: 600;
    }
</style>