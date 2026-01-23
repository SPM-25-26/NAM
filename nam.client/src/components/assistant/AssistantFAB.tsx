import React, { useState } from "react";
import {
  Fab,
  Dialog,
  Slide,
  IconButton,
  Box,
  useTheme,
  useMediaQuery,
  alpha,
  Stack,
  Typography,
  Tooltip,
  Button,
} from "@mui/material";
import type { TransitionProps } from "@mui/material/transitions";
import OpenInFullRoundedIcon from "@mui/icons-material/OpenInFullRounded";
import { useChat } from "./hooks/UseChatMock";
import ChatPanel from "./ChatPanel";
import InfoModal from "../InfoModal";
import { CloseButton } from "../CloseButton";
import { useNavigate } from "react-router-dom";
import { AssistantAvatar } from "./components/AssistantAvatar";

const Transition = React.forwardRef(function Transition(
  props: TransitionProps & { children: React.ReactElement },
  ref: React.Ref<unknown>,
) {
  const { children, ...other } = props;
  return (
    <Slide direction="up" ref={ref} {...other}>
      {children}
    </Slide>
  );
});

export const AssistantFAB: React.FC = () => {
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const [openConfirm, setOpenConfirm] = useState(false);
  const { messages, sendMessage, isLoading, error, clearChat } = useChat();

  return (
    <>
      {!open && (
        <Tooltip title={`Apri ${AssistantAvatar.displayName} IA`}>
          <Fab
            onClick={() => setOpen(true)}
            sx={{
              position: "fixed",
              bottom: { xs: 20, sm: 24 },
              right: { xs: 20, sm: 24 },
              width: { xs: 56, sm: 64 },
              height: { xs: 56, sm: 64 },
              bgcolor: theme.palette.background.paper,
              boxShadow: `0 12px 32px ${alpha(theme.palette.common.black, 0.25)}`,
              border: `1px solid ${alpha(theme.palette.primary.main, 0.15)}`,
              zIndex: 9000,
              transition: "all 0.3s cubic-bezier(0.4, 0, 0.2, 1)",
              "&:hover": { transform: "scale(1.1) translateY(-4px)" },
              "&:active": { transform: "scale(0.95)" },
            }}
          >
            <AssistantAvatar full />
          </Fab>
        </Tooltip>
      )}

      <Dialog
        fullScreen={isMobile}
        open={open}
        onClose={() => setOpen(false)}
        disableScrollLock
        slots={{
          transition: Transition,
        }}
        slotProps={{
          paper: {
            sx: {
              position: { sm: "fixed" },
              bottom: { sm: 30 },
              right: { sm: 30 },
              width: { sm: "420px", xs: "100%" },
              height: { sm: "min(700px, 80vh)", xs: "100dvh" },
              maxHeight: { xs: "100dvh", sm: "80vh" },
              borderRadius: { sm: "24px", xs: 0 },
              m: 0,
              overflow: "hidden",
              display: "flex",
              flexDirection: "column",
              backgroundImage: "none",
            },
          },
          transition: {
            timeout: 250,
          },
        }}
        sx={{
          "& .MuiDialog-container": {
            alignItems: { xs: "center", sm: "flex-end" },
            justifyContent: { xs: "center", sm: "flex-end" },
          },
        }}
      >
        {/* HEADER */}
        <Box
          sx={{
            p: { xs: 1.5, sm: 2 },
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            borderBottom: `1px solid ${alpha(theme.palette.divider, 0.08)}`,
            bgcolor: theme.palette.background.paper,
            flexShrink: 0,
          }}
        >
          <Stack direction="row" spacing={1.5} alignItems="center">
            <AssistantAvatar size={32} />
            <Box>
              <Typography variant="subtitle2">
                {AssistantAvatar.displayName}
              </Typography>
              <Stack direction="row" spacing={0.5} alignItems="center">
                <Box
                  sx={{
                    width: 6,
                    height: 6,
                    bgcolor: "success.main",
                    borderRadius: "50%",
                  }}
                />
                <Typography variant="caption">Online</Typography>
              </Stack>
            </Box>
          </Stack>

          <Stack direction="row" spacing={0.5}>
            {!isMobile && (
              <Tooltip title="Espandi" arrow>
                <IconButton size="small" onClick={() => navigate("/assistant")}>
                  <OpenInFullRoundedIcon sx={{ fontSize: 18 }} />
                </IconButton>
              </Tooltip>
            )}
            {messages.length > 0 && (
              <>
                <Tooltip title="Pulisci la conversazione">
                  <Button
                    onClick={() => setOpenConfirm(true)}
                    size="small"
                    variant="outlined"
                    sx={{
                      borderColor: "error.main",
                      textTransform: "none",
                      color: "error.main",
                      fontWeight: 400,
                      "&:hover": {
                        backgroundColor: "rgba(211, 47, 47, 0.04)",
                        borderColor: "error.main",
                      },
                    }}
                  >
                    Pulisci chat
                  </Button>
                </Tooltip>
                <InfoModal
                  isDelete
                  open={openConfirm}
                  onClose={() => setOpenConfirm(false)}
                  onAction={clearChat}
                  title="Svuota conversazione"
                  description="Sei sicuro di voler eliminare tutti i messaggi? Questa azione è irreversibile e non potrai recuperare la cronologia di questa sessione."
                  buttonText="Elimina tutto"
                />
              </>
            )}
            <CloseButton action={() => setOpen(false)} />
          </Stack>
        </Box>

        <Box
          sx={{
            flex: 1,
            minHeight: 0,
            bgcolor: theme.palette.background.paper,
            display: "flex",
            flexDirection: "column",
          }}
        >
          <ChatPanel
            messages={messages}
            sendMessage={sendMessage}
            isLoading={isLoading}
            error={error}
          />
        </Box>
      </Dialog>
    </>
  );
};
