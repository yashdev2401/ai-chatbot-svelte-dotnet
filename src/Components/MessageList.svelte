<script lang="ts">
    import { afterUpdate } from 'svelte';
    import MessageItem from './MessageItem.svelte';
    import TypingLoader from './TypingLoader.svelte';
    import type { ChatMessage } from '../Types/chat';

    export let messages: ChatMessage[] = [];
    export let loading = false;

    let container: HTMLDivElement;

    afterUpdate(() => {
        container.scrollTop = container.scrollHeight;
    });
</script>

<div bind:this={container} class="messages">

    {#each messages as message}
        <MessageItem {message} />
    {/each}

    {#if loading}
        <TypingLoader />
    {/if}

</div>

<style>
    .messages {
        flex: 1;
        overflow-y: auto;
        padding: 30px;
        display: flex;
        flex-direction: column;
    }
</style>