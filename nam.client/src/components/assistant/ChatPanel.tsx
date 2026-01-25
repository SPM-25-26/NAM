import React, { useEffect, useRef } from "react";
import {
  Box,
  Typography,
  Stack,
  useTheme,
  alpha,
  IconButton,
  Tooltip,
} from "@mui/material";
import ChatInput from "./components/ChatInput";
import type { Message } from "./hooks/ObjectModel";
import ChatMessage, { MessageIA } from "./components/ChatMessage";
import { RefreshRounded } from "@mui/icons-material";
import ErrorOutlineRounded from "@mui/icons-material/ErrorOutlineRounded";
import { AssistantAvatar } from "./components/AssistantAvatar";

const EMPTY_STATE_CONFIG = {
  title: `Esplora Matelica con ${AssistantAvatar.displayName}`,
  description:
    "Sono la tua guida interattiva per Matelica. Posso aiutarti a scoprire sentieri, borghi medievali e le leggende locali.",
};

interface ChatInterfaceProps {
  messages: Message[];
  sendMessage: (content: string) => Promise<void>;
  isLoading: boolean;
  error: Error | null;
}

const ChatPanel: React.FC<ChatInterfaceProps> = ({
  messages,
  sendMessage,
  isLoading,
  error,
}) => {
  const theme = useTheme();
  const scrollRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (scrollRef.current) {
      scrollRef.current.scrollTo({
        top: scrollRef.current.scrollHeight,
        behavior: "smooth",
      });
    }
  }, [messages, isLoading]);

  const renderEmptyState = () => (
    <Box
      sx={{
        m: "auto",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        maxWidth: "420px",
        textAlign: "center",
        px: 3,
        animation: "fadeInUp 0.6s ease-out",
        "@keyframes fadeInUp": {
          from: { opacity: 0, transform: "translateY(20px)" },
          to: { opacity: 1, transform: "translateY(0)" },
        },
      }}
    >
      <AssistantAvatar size={90} />
      <Typography
        variant="h5"
        sx={{ fontWeight: 800, mb: 1.5, letterSpacing: "-0.02em" }}
      >
        {EMPTY_STATE_CONFIG.title}
      </Typography>

      <Typography
        variant="body2"
        sx={{ color: "text.secondary", mb: 4, lineHeight: 1.6 }}
      >
        {EMPTY_STATE_CONFIG.description}
      </Typography>
      <Stack
        direction="row"
        spacing={1}
        flexWrap="wrap"
        justifyContent="center"
        gap={1.5}
      >
        {[
          {
            label: "Sentieri del San Vicino",
            icon: "⛰️",
          },
          {
            label: "Il centro Storico",
            icon: "🏰",
          },
          {
            label: "Vino Verdicchio di Matelica",
            icon: "🍷",
          },
        ].map((item) => (
          <Box
            key={item.label}
            onClick={() => sendMessage(item.label)}
            sx={{
              px: 2.5,
              py: 1.2,
              borderRadius: "100px",
              border: `1px solid ${alpha(theme.palette.divider, 0.15)}`,
              bgcolor: theme.palette.background.paper,
              fontSize: "0.85rem",
              fontWeight: 600,
              cursor: "pointer",
              display: "flex",
              alignItems: "center",
              gap: 1,
              transition: "all 0.2s cubic-bezier(0.4, 0, 0.2, 1)",
              "&:hover": {
                borderColor: theme.palette.primary.main,
                bgcolor: alpha(theme.palette.primary.main, 0.04),
                transform: "translateY(-3px)",
                boxShadow: `0 4px 12px ${alpha(theme.palette.primary.main, 0.08)}`,
              },
            }}
          >
            <span>{item.icon}</span> {item.label}
          </Box>
        ))}
      </Stack>
    </Box>
  );

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        height: { xs: "100dvh", md: "100%" },
        width: "100%",
        overflow: "hidden",
        bgcolor: theme.palette.background.paper,
        position: "relative",
      }}
    >
      <Box
        component="main"
        ref={scrollRef}
        sx={{
          flex: "1 1 0%",
          minHeight: 0,
          overflowY: "auto",
          p: { xs: 2, md: 4 },
          display: "flex",
          flexDirection: "column",
          "&::-webkit-scrollbar": { width: "5px" },
          "&::-webkit-scrollbar-thumb": {
            backgroundColor: alpha(theme.palette.divider, 0.15),
            borderRadius: "10px",
          },
        }}
      >
        {messages.length === 0 && !isLoading ? (
          renderEmptyState()
        ) : (
          <Stack spacing={2} sx={{ width: "100%", pb: 2 }}>
            {messages.map((msg) => (
              <ChatMessage key={msg.id} message={msg} />
            ))}

            {isLoading && (
              <MessageIA isLoading>
                <Stack
                  direction="row"
                  spacing={1}
                  alignItems="center"
                  sx={{ minHeight: "24px", py: 0.5 }}
                >
                  {[0, 1, 2].map((i) => (
                    <Box
                      key={i}
                      sx={{
                        width: 6,
                        height: 6,
                        bgcolor: theme.palette.primary.main,
                        borderRadius: "50%",
                        animation:
                          "typingBounce 1.4s infinite ease-in-out both",
                        animationDelay: `${i * 0.16}s`,
                        "@keyframes typingBounce": {
                          "0%, 80%, 100%": {
                            transform: "scale(0.6)",
                            opacity: 0.3,
                          },
                          "40%": { transform: "scale(1.2)", opacity: 1 },
                        },
                      }}
                    />
                  ))}
                </Stack>
              </MessageIA>
            )}
          </Stack>
        )}
        {error && (
          <Box
            sx={{
              mt: "auto",
              mb: 3,
              mx: "auto",
              maxWidth: "600px",
              width: "95%",
              display: "flex",
              alignItems: "center",
              gap: 2,
              p: "10px 16px",
              borderRadius: "14px",
              bgcolor: (theme) => alpha(theme.palette.error.main, 0.04),
              border: (theme) =>
                `1px solid ${alpha(theme.palette.error.main, 0.1)}`,
              animation: "errorAppear 0.3s ease-out",
              "@keyframes errorAppear": {
                from: { opacity: 0, transform: "scale(0.98)" },
                to: { opacity: 1, transform: "scale(1)" },
              },
            }}
          >
            {/* Contenuto Errore */}
            <Box
              sx={{ display: "flex", alignItems: "center", gap: 1.5, flex: 1 }}
            >
              <ErrorOutlineRounded
                sx={{
                  color: "error.main",
                  fontSize: 20,
                  opacity: 0.9,
                }}
              />
              <Typography
                variant="body2"
                sx={{
                  color: "error.main",
                  fontWeight: 600,
                  fontSize: "0.82rem",
                  lineHeight: 1.3,
                }}
              >
                {error.message}
              </Typography>
            </Box>

            {/* Azione di Resend */}
            <Tooltip title="Riprova l'invio" arrow>
              <IconButton
                size="small"
                onClick={() => {
                  const lastUserMsg = messages
                    .filter((m) => m.role === "user")
                    .pop();
                  if (lastUserMsg) sendMessage(lastUserMsg.content);
                }}
                sx={{
                  transition: "all 0.2s cubic-bezier(0.4, 0, 0.2, 1)",
                  "&:hover": {
                    bgcolor: (theme) => alpha(theme.palette.error.main, 0.15),
                    transform: "rotate(30deg)",
                  },
                  "&:active": {
                    transform: "scale(0.9)",
                  },
                }}
              >
                <RefreshRounded sx={{ fontSize: 18 }} />
              </IconButton>
            </Tooltip>
          </Box>
        )}{" "}
      </Box>

      <Box
        component="footer"
        sx={{
          flexShrink: 0,
          p: 2,
          pb: { xs: 3, md: 2 },
          borderTop: `1px solid ${alpha(theme.palette.divider, 0.08)}`,
          bgcolor: theme.palette.background.paper,
        }}
      >
        <ChatInput onSend={sendMessage} disabled={isLoading} />

        <Box
          sx={{
            mt: 1.5,
            px: 2,
            textAlign: "center",
          }}
        >
          <Typography
            variant="caption"
            sx={{
              color: theme.palette.text.disabled,
              fontSize: "0.6rem",
              lineHeight: 1.4,
              display: "block",
              "& span": {
                color: theme.palette.text.secondary,
                fontWeight: 500,
              },
            }}
          >
            <span>{AssistantAvatar.displayName}</span> può commettere errori.
            Verifica le informazioni importanti riguardanti orari e prenotazioni
            ufficiali.
          </Typography>
        </Box>
      </Box>
    </Box>
  );
};

export default ChatPanel;
