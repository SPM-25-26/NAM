import { useState, useCallback, useEffect } from "react";
import type { Message } from "./ObjectModel";
import { buildApiUrl } from "../../../config";

/**
 * Custom hook to manage chat state, including message history,
 * loading states, and error handling with session persistence.
 */
export const useChat = () => {
  // Initialize messages from sessionStorage to maintain state across internal navigation
  const [messages, setMessages] = useState<Message[]>(() => {
    try {
      const saved = sessionStorage.getItem("chat_session_history");
      return saved ? JSON.parse(saved) : [];
    } catch (e) {
      console.error("Failed to restore chat session:", e);
      return [];
    }
  });

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<Error | null>(null);

  // Sync state to sessionStorage whenever the message array updates
  useEffect(() => {
    sessionStorage.setItem("chat_session_history", JSON.stringify(messages));
  }, [messages]);

  /**
   * Resets the chat state and clears the browser session storage.
   */
  const clearChat = useCallback(() => {
    setMessages([]);
    setIsLoading(false);
    setError(null);
    sessionStorage.removeItem("chat_session_history");
  }, []);

  /**
   * Processes the user input, updates the UI, and simulates AI response logic.
   * @param content The text message sent by the user.
   */
  const sendMessage = useCallback(
    async (content: string) => {
      if (!content.trim()) return;

      const userMessage: Message = {
        id: crypto.randomUUID(),
        role: "user",
        content,
        timestamp: Date.now(),
        status: "sent",
      };

      setMessages((prev) => [...prev, userMessage]);
      setIsLoading(true);
      setError(null);

      try {
        const payload = JSON.stringify({
          history: [...messages, userMessage].map((message) => ({
            role: message.role,
            content: message.content,
          })),
        });
        const response = await fetch(buildApiUrl("/api/assistant/chat"), {
          method: "POST",
          headers: { Accept: "application/json" },
          credentials: "include",
          body: payload,
        });

        let assistantMessage: Message;

        if (response.ok) {
          assistantMessage = {
            id: crypto.randomUUID(),
            role: "assistant",
            content: await response.json(),
            timestamp: Date.now(),
            status: "sent",
          };
        } else {
          throw new Error("Qualcosa è andato storto...");
        }

        setMessages((prev) => [...prev, assistantMessage]);
      } catch (err) {
        setError(
          err instanceof Error ? err : new Error("An unknown error occurred"),
        );
      } finally {
        setIsLoading(false);
      }
    },
    [messages],
  );

  return { messages, sendMessage, isLoading, error, clearChat };
};
