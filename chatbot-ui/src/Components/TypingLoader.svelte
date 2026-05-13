<div class="typing">
    <span></span>
    <span></span>
    <span></span>
</div>

<style>
    .typing {
        display: flex;
        gap: 6px;
        padding: 10px 14px;
    }

    .typing span {
        width: 10px;
        height: 10px;
        background: #9ca3af;
        border-radius: 50%;
        animation: bounce 1.4s infinite ease-in-out;
    }

    .typing span:nth-child(2) {
        animation-delay: 0.2s;
    }

    .typing span:nth-child(3) {
        animation-delay: 0.4s;
    }

    @keyframes bounce {
        0%, 80%, 100% {
            transform: scale(0);
        }

        40% {
            transform: scale(1);
        }
    }
</style>