import React, { memo } from "react";
import ReactMarkdown from "react-markdown";
import {
  Box,
  Typography,
  Avatar,
  useTheme,
  alpha,
  type Theme,
} from "@mui/material";
import PersonRoundedIcon from "@mui/icons-material/PersonRounded";
import remarkGfm from "remark-gfm";
import type { Message } from "../hooks/ObjectModel";
import { AssistantAvatar } from "./AssistantAvatar";

export const TypingIndicator: React.FC = () => {
  const theme = useTheme();
  const aiAccent = theme.palette.primary.main;

  return (
    <MessageIA isLoading>
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          gap: 0.8,
          minHeight: "24px",
        }}
      >
        {[0, 1, 2].map((i) => (
          <Box
            key={i}
            sx={{
              width: 5,
              height: 5,
              bgcolor: aiAccent,
              borderRadius: "50%",
              animation: "typingBounce 1.4s infinite ease-in-out both",
              animationDelay: `${i * 0.16}s`,
              "@keyframes typingBounce": {
                "0%, 80%, 100%": { transform: "scale(0.6)", opacity: 0.3 },
                "40%": { transform: "scale(1.2)", opacity: 1 },
              },
            }}
          />
        ))}
      </Box>
    </MessageIA>
  );
};

interface MessageIAProps {
  children: React.ReactNode;
  timestamp?: number;
  isLoading?: boolean;
}

export const MessageIA: React.FC<MessageIAProps> = ({
  children,
  timestamp,
  isLoading,
}) => {
  const theme = useTheme();
  const aiAccent = theme.palette.primary.main;

  return (
    <Box
      sx={{
        display: "flex",
        textAlign: "left",
        flexDirection: "row",
        alignItems: "flex-start",
        gap: { xs: 1.5, md: 2.5 },
        mb: 5,
        width: "100%",
        animation: "smoothAppearance 0.5s cubic-bezier(0.2, 0.8, 0.2, 1)",
      }}
    >
      <AssistantAvatar size={32} />

      <Box sx={{ maxWidth: { xs: "85%", md: "75%" } }}>
        <Box
          sx={{
            p: "14px 20px",
            borderRadius: "4px 20px 20px 20px",
            backgroundImage:
              "linear-gradient(90deg, rgba(21, 93, 252, 0.03) 0%, rgba(152, 16, 250, 0.03) 100%)",
            border: `1px solid ${alpha(aiAccent, 0.1)}`,
          }}
        >
          <Typography
            component="div"
            variant="body1"
            sx={messageTypographyStyle(theme, aiAccent)}
          >
            {children}
          </Typography>
        </Box>
        {!isLoading && timestamp && (
          <MessageFooter timestamp={timestamp} isAssistant />
        )}
      </Box>
    </Box>
  );
};

interface MessageUserProps {
  content: string;
  timestamp: number;
}

export const MessageUser: React.FC<MessageUserProps> = ({
  content,
  timestamp,
}) => {
  const theme = useTheme();
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "row-reverse",
        alignItems: "flex-start",
        gap: { xs: 1.5, md: 2.5 },
        mb: 5,
        width: "100%",
        animation: "smoothAppearance 0.5s cubic-bezier(0.2, 0.8, 0.2, 1)",
      }}
    >
      <Avatar
        sx={{
          width: 32,
          height: 32,
          bgcolor: "transparent",
          color: theme.palette.text.secondary,
          border: `1px solid ${alpha(theme.palette.divider, 0.15)}`,
          mt: 0.5,
        }}
      >
        <PersonRoundedIcon sx={{ fontSize: 18 }} />
      </Avatar>
      <Box
        sx={{
          maxWidth: { xs: "85%", md: "75%" },
          display: "flex",
          flexDirection: "column",
          alignItems: "flex-end",
        }}
      >
        <Box
          sx={{
            p: "14px 20px",
            borderRadius: "20px 4px 20px 20px",
            bgcolor: theme.palette.action.hover,
            border: `1px solid ${alpha(theme.palette.divider, 0.1)}`,
          }}
        >
          <Typography
            variant="body1"
            sx={{
              color: theme.palette.text.primary,
              fontSize: "0.93rem",
              lineHeight: 1.6,
            }}
          >
            {content}
          </Typography>
        </Box>
        <MessageFooter timestamp={timestamp} isAssistant={false} />
      </Box>
    </Box>
  );
};

const MessageFooter = ({
  timestamp,
  isAssistant,
}: {
  timestamp: number;
  isAssistant: boolean;
}) => (
  <Box
    sx={{
      display: "flex",
      gap: 2,
      mt: 0.8,
      px: 1,
      flexDirection: isAssistant ? "row" : "row-reverse",
    }}
  >
    <Typography
      variant="caption"
      sx={{ color: "text.disabled", fontSize: "0.65rem" }}
    >
      {new Date(timestamp).toLocaleTimeString([], {
        hour: "2-digit",
        minute: "2-digit",
      })}
    </Typography>
  </Box>
);

const messageTypographyStyle = (theme: Theme, aiAccent: string) => ({
  color: theme.palette.text.primary,
  fontSize: "0.93rem",
  lineHeight: 1.6,
  letterSpacing: "0.01em",
  "& p": { m: 0, mb: 1.5 },
  "& p:last-child": { mb: 0 },
  "& strong": { fontWeight: 700 },
  "& code": {
    fontFamily: "'Inter', sans-serif",
    color: aiAccent,
    bgcolor: alpha(aiAccent, 0.06),
    p: "2px 4px",
    borderRadius: "4px",
    fontSize: "0.9em",
  },
});

const ChatMessage: React.FC<{ message: Message }> = ({ message }) => {
  return message.role === "assistant" ? (
    <MessageIA timestamp={message.timestamp}>
      <Box
        sx={{
          maxWidth: "100%",
          "& img": {
            maxWidth: "100%",
            height: "auto",
            display: "block",
            borderRadius: 1,
            marginY: 1,
          },
          "& table": {
            width: "100%",
            borderCollapse: "collapse",
            overflowX: "auto",
            display: "block",
          },
          "& th, & td": {
            border: "1px solid",
            padding: "4px 8px",
            textAlign: "left",
          },
        }}
      >
        <ReactMarkdown remarkPlugins={[remarkGfm]}>
          {message.content}
        </ReactMarkdown>
      </Box>
    </MessageIA>
  ) : (
    <MessageUser content={message.content} timestamp={message.timestamp} />
  );
};

export default memo(ChatMessage);
