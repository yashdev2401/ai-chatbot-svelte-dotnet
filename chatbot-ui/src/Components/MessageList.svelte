<script lang="ts">
    import { afterUpdate } from 'svelte';
    import MessageItem from './MessageItem.svelte';
    import TypingLoader from './TypingLoader.svelte';
    import type { ChatMessage } from '../Types/chat';

    export let messages: ChatMessage[] = [];
    export let loading = false;

    let container: HTMLDivElement;

    let showScrollButton = false;

    let shouldAutoScroll = true;

    function handleScroll() {

        if (!container) {
            return;
        }

        const threshold = 120;

        const isNearBottom =
            container.scrollHeight -
            container.scrollTop -
            container.clientHeight <
            threshold;

        shouldAutoScroll = isNearBottom;

        showScrollButton = !isNearBottom;
    }
    function scrollToBottom() {

        container?.scrollTo({
            top: container.scrollHeight,
            behavior: 'smooth'
        });

        showScrollButton = false;
    }

    afterUpdate(() => {

        if (shouldAutoScroll) {

            scrollToBottom();
        }
    });
</script>

<div class="messages-wrapper">

    <div
        bind:this={container}
        class="messages"
        on:scroll={handleScroll}
    >

        {#each messages as message}

            <MessageItem {message} />

        {/each}

        {#if loading}

            <TypingLoader />

        {/if}

    </div>

    {#if showScrollButton}

        <button
            class="scroll-bottom"
            on:click={scrollToBottom}
        >
            ↓
        </button>

    {/if}

</div>

<style>

    .messages-wrapper {

        position: relative;

        flex: 1;

        overflow: hidden;
    }

    .messages {

        height: 100%;

        overflow-y: auto;

        padding: 30px;

        display: flex;

        flex-direction: column;

        scroll-behavior: smooth;
    }

    .scroll-bottom {

        position: absolute;

        right: 24px;

        bottom: 24px;

        width: 46px;

        height: 46px;

        border-radius: 50%;

        border: none;

        background: #2563eb;

        color: white;

        font-size: 22px;

        cursor: pointer;

        box-shadow: 0 4px 14px rgba(0,0,0,0.4);

        z-index: 100;
    }

    .scroll-bottom:hover {

        opacity: 0.9;
    }

</style>