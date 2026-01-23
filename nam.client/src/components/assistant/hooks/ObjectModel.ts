export type MessageRole = "user" | "assistant";

export interface Message {
  id: string;
  role: MessageRole;
  content: string;
  timestamp: number;
  links?: string[];
  status?: "sending" | "sent" | "error";
}

export interface ChatState {
  messages: Message[];
  isLoading: boolean;
  error: Error | null;
}
