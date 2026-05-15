<script lang="ts">
  import { marked } from "marked";
  import { onMount } from "svelte";
  import type { ChatMessage } from "../Types/chat";

  export let message: ChatMessage;

  let html = "";
  async function copyCode(code: string) {
    await navigator.clipboard.writeText(code);
  }
  onMount(async () => {
    html = await marked(message.text);

    setTimeout(() => {
      const blocks = document.querySelectorAll("pre");
      blocks.forEach((block) => {
        if (block.querySelector(".copy-btn")) {
          return;
        }
        const button = document.createElement("button");
        button.innerText = "Copy";
        button.className = "copy-btn";
        button.onclick = async () => {
          const code = block.querySelector("code")?.innerText || "";
          await copyCode(code);
          button.innerText = "Copied";
          setTimeout(() => {
            button.innerText = "Copy";
          }, 1500);
        };
        block.appendChild(button);
      });
    }, 50);
  });
</script>

<div
  class:user={message.role === "user"}
  class:assistant={message.role === "assistant"}
  class="message"
>
  {@html html}
</div>

<style>
  .message {
    max-width: 75%;
    padding: 16px 20px;
    border-radius: 18px;
    margin-bottom: 20px;
    line-height: 1.7;
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
    background: #111827;
    color: #f3f4f6;
    border-bottom-left-radius: 6px;
    border: 1px solid #374151;
  }

  :global(pre) {
    position: relative;
    background: #0b1220;
    border: 1px solid #374151;
    border-radius: 14px;
    padding: 18px;
    overflow-x: auto;
    margin-top: 14px;
  }

  :global(pre code) {
    background: transparent;
    color: #e5e7eb;
    font-size: 14px;
    line-height: 1.6;
  }

  :global(code) {
    background: #1f2937;
    padding: 2px 6px;
    border-radius: 6px;
    color: #93c5fd;
  }

  :global(.copy-btn) {
    position: absolute;
    top: 10px;
    right: 10px;
    border: none;
    background: #1f2937;
    color: white;
    padding: 6px 10px;
    border-radius: 8px;
    cursor: pointer;
    font-size: 12px;
  }

  :global(.copy-btn:hover) {
    background: #374151;
  }
</style>