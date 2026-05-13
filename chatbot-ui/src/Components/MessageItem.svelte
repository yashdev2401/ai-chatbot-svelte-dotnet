<script lang="ts">
    import { marked } from 'marked';
    import type { ChatMessage } from '../Types/chat';

    export let message: ChatMessage;

    const html = marked(message.text);
</script>

<div class:user={message.role === 'user'}
     class:assistant={message.role === 'assistant'}
     class="message">

    {@html html}

</div>

<style>
    .message {
        max-width: 75%;
        padding: 14px 18px;
        border-radius: 18px;
        margin-bottom: 18px;
        line-height: 1.6;
        font-size: 15px;
        word-break: break-word;
    }

    .user {
        margin-left: auto;
        background: linear-gradient(135deg, #2563eb, #1d4ed8);
        color: white;
        border-bottom-right-radius: 6px;
    }

    .assistant {
        margin-right: auto;
        background: #1f2937;
        color: #f3f4f6;
        border-bottom-left-radius: 6px;
        border: 1px solid #374151;
    }

    :global(pre) {
        background: #111827;
        padding: 14px;
        border-radius: 10px;
        overflow-x: auto;
    }

    :global(code) {
        color: #93c5fd;
    }
</style>