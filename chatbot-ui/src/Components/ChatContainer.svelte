<script lang="ts">
  import { onMount, tick } from "svelte";
  import MessageList from "./MessageList.svelte";
  import ChatInput from "./ChatInput.svelte";

  import { sendMessage, getMessages } from "../Services/api";
  export let loading = false;

  let messages: any[] = [];

  let messagesContainer: HTMLDivElement;

  let showScrollButton = false;
  let conversationId: string;
  const storedConversationId = sessionStorage.getItem("conversationId");

  if (storedConversationId) {
    conversationId = storedConversationId;
  } else {
    conversationId = crypto.randomUUID();

    sessionStorage.setItem("conversationId", conversationId);
  }

  async function loadMessages() {
    const oldMessages = await getMessages(conversationId);

    messages = oldMessages.map((x: any) => ({
      role: x.role,
      text: x.content,
    }));
    await tick();
    scrollToBottom();
  }

  onMount(async () => {
    await loadMessages();
  });

  function scrollToBottom() {
    if (!messagesContainer) {
      return;
    }
    messagesContainer.scrollTo({
      top: messagesContainer.scrollHeight,
      behavior: "smooth",
    });
  }

  function handleScroll() {
    if (!messagesContainer) {
      return;
    }
    const isNearBottom =
      messagesContainer.scrollHeight -
        messagesContainer.scrollTop -
        messagesContainer.clientHeight <
      200;
    showScrollButton = !isNearBottom;
  }

  function speakMessage(text: string) {
    window.speechSynthesis.cancel();

    const speech = new SpeechSynthesisUtterance(text);

    speech.lang = "en-US";
    speech.rate = 1;
    speech.pitch = 1;

    window.speechSynthesis.speak(speech);
  }

  async function handleSend(event: CustomEvent<string>) {
    const text = event.detail;

    if (!text.trim() || loading) {
      return;
    }

    messages = [
      ...messages,
      {
        role: "user",text,
      },
    ];

    await tick();
    scrollToBottom();
    loading = true;

    try {
      const reply = await sendMessage(conversationId, text);

      messages = [
        ...messages,
        {
          role: "assistant",
          text: reply,
        },
      ];
      await tick();
      scrollToBottom();
      speakMessage(reply);
    } catch {
      messages = [
        ...messages,
        {
          role: "assistant",
          text: "Something went wrong.",
        },
      ];
    }

    loading = false;
  }
</script>

<div class="chat-container">
  <div
    bind:this={messagesContainer}
    class="messages-wrapper"
    on:scroll={handleScroll}
  >
    <MessageList {messages} {loading} />
  </div>
  {#if showScrollButton}
    <button class="scroll-bottom-btn" on:click={scrollToBottom}> ↓ </button>
  {/if}
  <div class="input-wrapper">
    <ChatInput {loading} on:send={handleSend} />
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
    position: relative;
  }
  .messages-wrapper {
    flex: 1;
    overflow-y: auto;
    padding: 24px;
    min-height: 0;
    padding-bottom: 120px;
  }

  .input-wrapper {
    padding: 16px 24px;
    border-top: 1px solid #1e293b;
    background: #020617;
  }

  .scroll-bottom-btn {
    position: absolute;
    right: 30px;
    bottom: 100px;
    width: 48px;
    height: 48px;
    border-radius: 50%;
    border: none;
    background: #2563eb;
    color: white;
    cursor: pointer;
    font-size: 22px;
    z-index: 100;
  }
</style>