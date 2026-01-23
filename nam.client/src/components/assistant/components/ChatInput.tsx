import React, {
  useState,
  type FormEvent,
  type ChangeEvent,
  useRef,
  useEffect,
  memo,
} from "react";
import {
  Box,
  TextField,
  IconButton,
  InputAdornment,
  useTheme,
  CircularProgress,
  alpha,
} from "@mui/material";
import ArrowUpwardRoundedIcon from "@mui/icons-material/ArrowUpwardRounded";
import { AssistantAvatar } from "./AssistantAvatar";

interface ChatInputProps {
  onSend: (content: string) => Promise<void> | void;
  disabled: boolean;
}

const ChatInputComponent: React.FC<ChatInputProps> = ({ onSend, disabled }) => {
  const theme = useTheme();
  const [text, setText] = useState("");
  const inputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    if (!disabled) inputRef.current?.focus();
  }, [disabled]);

  const handleSubmit = (e?: FormEvent) => {
    e?.preventDefault();
    if (!disabled && text.trim()) {
      onSend(text.trim());
      setText("");
    }
  };

  return (
    <Box
      component="form"
      onSubmit={handleSubmit}
      sx={{
        maxWidth: 800,
        mx: "auto",
        width: "100%",
      }}
    >
      <TextField
        fullWidth
        multiline
        maxRows={8}
        inputRef={inputRef}
        value={text}
        disabled={disabled}
        placeholder={`Ciao, sono ${AssistantAvatar.displayName} come posso aiutarti?`}
        onChange={(e: ChangeEvent<HTMLInputElement>) => setText(e.target.value)}
        onKeyDown={(e) => {
          if (e.key === "Enter" && !e.shiftKey) {
            e.preventDefault();
            handleSubmit();
          }
        }}
        sx={{
          "& .MuiInputBase-root": {
            px: 1.5,
            py: 1.2,
            borderRadius: "24px",
            backgroundColor:
              theme.palette.mode === "light"
                ? "#f4f4f4"
                : alpha(theme.palette.common.white, 0.06),
            "& fieldset": {
              border: "none",
            },
            "&.Mui-focused": {
              backgroundColor:
                theme.palette.mode === "light"
                  ? "#ededed"
                  : alpha(theme.palette.common.white, 0.1),
            },
          },
        }}
        slotProps={{
          input: {
            endAdornment: (
              <InputAdornment position="end">
                <IconButton
                  type="submit"
                  disabled={disabled || !text.trim()}
                  sx={{
                    background: text.trim()
                      ? "linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%)"
                      : "transparent",
                    color: text.trim()
                      ? theme.palette.common.white
                      : theme.palette.action.disabled,
                    width: 32,
                    height: 32,
                    transition: "all 0.3s ease-in-out",
                    "&:hover": {
                      background:
                        "linear-gradient(90deg, rgba(21, 93, 252, 0.80) 0%, rgba(152, 16, 250, 0.80) 100%)",
                      transform: "scale(1.05)",
                    },
                    "&.Mui-disabled": {
                      background: "transparent",
                      color: theme.palette.action.disabled,
                    },
                  }}
                >
                  {disabled ? (
                    <CircularProgress size={16} color="inherit" />
                  ) : (
                    <ArrowUpwardRoundedIcon
                      sx={{
                        fontSize: 18,
                        filter: text.trim()
                          ? "drop-shadow(0px 2px 4px rgba(0,0,0,0.1))"
                          : "none",
                      }}
                    />
                  )}
                </IconButton>
              </InputAdornment>
            ),
          },
        }}
      />
    </Box>
  );
};

export const ChatInput = memo(ChatInputComponent);

export default ChatInput;
