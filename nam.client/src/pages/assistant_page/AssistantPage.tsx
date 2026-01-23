import React, { useState } from "react";
import {
  Box,
  Typography,
  Container,
  useTheme,
  alpha,
  IconButton,
  Tooltip,
  Button,
} from "@mui/material";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import ChatPanel from "../../components/assistant/ChatPanel";
import InfoModal from "../../components/InfoModal";
import {
  HelpOutlineRounded,
  NearMeRounded,
  TipsAndUpdatesRounded,
} from "@mui/icons-material";

import { AssistantAvatar } from "../../components/assistant/components/AssistantAvatar";
import { useChat } from "../../components/assistant/hooks/UseChat";

const infoSections = [
  {
    title: "La tua guida digitale",
    content: `${AssistantAvatar.displayName} è un'intelligenza artificiale evoluta, concepita per svelarti la storia, l'arte e i tesori nascosti di Matelica e del suo territorio.`,
    icon: <NearMeRounded />,
  },
  {
    title: "Esperienze su misura",
    content:
      "Chiedi consigli su monumenti, itinerari nel centro storico, curiosità medievali o suggerimenti enogastronomici per vivere la città.",
    icon: <HelpOutlineRounded />,
  },
  {
    title: "Chiedi i dettagli",
    content: `Più sarai specifico, più ${AssistantAvatar.displayName} saprà stupirti. Prova a chiedere: 'Qual è la leggenda di Palazzo Ottoni?' o 'Cosa vedere in due ore?'`,
    icon: <TipsAndUpdatesRounded />,
  },
];

const AssistantPage: React.FC = () => {
  const theme = useTheme();
  const { messages, sendMessage, isLoading, error, clearChat } = useChat();
  const [infoOpen, setInfoOpen] = useState(false);
  const [openConfirm, setOpenConfirm] = useState(false);
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        height: "100dvh",
        width: "100vw",
        bgcolor: theme.palette.background.default,
        overflow: "hidden",
      }}
    >
      <Box
        component="header"
        sx={{
          flexShrink: 0,
          zIndex: 10,
          pt: { xs: 1, sm: 2 },
        }}
      >
        <Container maxWidth="md" sx={{ px: { xs: 2, sm: 3 } }}>
          <Box
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              p: { xs: 1.5, sm: "12px 24px" },
            }}
          >
            <Box sx={{ display: "flex", alignItems: "center", gap: 1.5 }}>
              <AssistantAvatar size={32} />

              <Box sx={{ textAlign: "left" }}>
                <Typography
                  variant="subtitle1"
                  sx={{
                    fontWeight: 800,
                    fontSize: { xs: "0.95rem", sm: "1.05rem" },
                    lineHeight: 1.2,
                    color: "text.primary",
                  }}
                >
                  {AssistantAvatar.displayName}
                </Typography>
                <Typography
                  variant="caption"
                  sx={{
                    color: "success.main",
                    fontWeight: 700,
                    fontSize: "0.65rem",
                    display: "flex",
                    alignItems: "center",
                    gap: 0.5,
                    textTransform: "uppercase",
                  }}
                >
                  <Box
                    sx={{
                      width: 6,
                      height: 6,
                      bgcolor: "success.main",
                      borderRadius: "50%",
                    }}
                  />
                  Online
                </Typography>
              </Box>
            </Box>
            <Box sx={{ display: "flex", alignItems: "center", gap: 1.5 }}>
              {messages.length > 0 && (
                <>
                  <Tooltip title="Pulisci la conversazione">
                    <Button
                      onClick={() => setOpenConfirm(true)}
                      size="small"
                      variant="text"
                      sx={{
                        textTransform: "none",
                        color: "error.main",
                        fontSize: "0.8rem",
                        fontWeight: 400,
                        "&:hover": {
                          backgroundColor: "rgba(211, 47, 47, 0.04)",
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

              <Tooltip title={`Scopri ${AssistantAvatar.displayName}`}>
                <IconButton
                  onClick={() => setInfoOpen(true)}
                  size="small"
                  sx={{
                    color: "#fff",
                    position: "relative",
                    boxShadow: `0 4px 12px ${alpha(theme.palette.primary.main, 0.2)}`,
                    background:
                      "linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%)",
                  }}
                >
                  <InfoOutlinedIcon sx={{ fontSize: 19 }} />
                </IconButton>
              </Tooltip>
            </Box>
          </Box>
        </Container>
      </Box>

      {/* 2. MAIN SPACE */}
      <Box
        component="main"
        sx={{
          flex: 1,
          minHeight: 0,
          display: "flex",
          flexDirection: "column",
        }}
      >
        <Container
          maxWidth="md"
          sx={{
            flex: 1,
            display: "flex",
            flexDirection: "column",
            minHeight: 0,
            px: { xs: 0, sm: 3 },
            pb: { xs: 0, sm: 2 },
          }}
        >
          <Box
            sx={{
              flex: 1,
              display: "flex",
              flexDirection: "column",
              minHeight: 0,
              bgcolor: theme.palette.background.paper,
              borderRadius: { xs: 0, sm: "28px" },
              border: {
                xs: "none",
                sm: `1px solid ${alpha(theme.palette.divider, 0.06)}`,
              },
              boxShadow: {
                xs: "none",
                sm: `0 40px 100px ${alpha(theme.palette.common.black, 0.07)}`,
              },
              overflow: "hidden",
            }}
          >
            <ChatPanel
              messages={messages}
              sendMessage={sendMessage}
              isLoading={isLoading}
              error={error}
            />
          </Box>
        </Container>
      </Box>

      <InfoModal
        open={infoOpen}
        onClose={() => setInfoOpen(false)}
        title={`Esplora ${AssistantAvatar.displayName}`}
        description={`${AssistantAvatar.displayName} unisce innovazione e cultura per accompagnarti alla scoperta di Matelica. Un progetto sperimentale al servizio del territorio.`}
        buttonText="Inizia a esplorare"
        items={infoSections.map((section) => ({
          t: section.title,
          d: section.content,
          icon: section.icon,
        }))}
      />
    </Box>
  );
};

export default AssistantPage;
