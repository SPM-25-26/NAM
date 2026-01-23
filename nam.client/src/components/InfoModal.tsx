import React from "react";
import {
  Dialog,
  DialogContent,
  DialogTitle,
  Typography,
  Stack,
  Box,
  alpha,
  useTheme,
  Fade,
  Button,
} from "@mui/material";
import MyButton from "./button";
import { CloseButton } from "./CloseButton";

interface InfoItem {
  t: string;
  d: string;
  icon: React.ReactNode;
}

interface InfoModalProps {
  open: boolean;
  onClose: () => void;
  title?: string;
  description?: string;
  buttonText?: string;
  buttonTextClose?: string;
  items?: InfoItem[];
  isDelete?: boolean;
  onAction?: () => void;
}

const InfoModal: React.FC<InfoModalProps> = ({
  open,
  onClose,
  title = "Messaggio",
  description = "",
  buttonText = "Procedi",
  buttonTextClose = "Chiudi",
  items,
  isDelete = false,
  onAction,
}) => {
  const theme = useTheme();
  const currentItems = items || [];

  const handleMainAction = () => {
    if (onAction) onAction();
    onClose();
  };

  return (
    <Dialog
      open={open}
      onClose={onClose}
      slots={{
        transition: Fade,
      }}
      slotProps={{
        transition: {
          timeout: 200,
        },
        paper: {
          sx: {
            borderRadius: "24px",
            maxWidth: "380px",
            width: "100%",
            mx: 2,
            border: `1px solid ${theme.palette.divider}`,
            boxShadow: `0 24px 60px ${alpha(theme.palette.common.black, 0.12)}`,
            backgroundImage: "none",
          },
        },
      }}
    >
      <DialogTitle
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          pt: 4,
          px: 4,
          pb: 1,
          typography: "h5",
        }}
      >
        {title}
        <CloseButton action={() => onClose()} />
      </DialogTitle>

      <DialogContent sx={{ px: 4, pb: 4 }}>
        <Typography
          variant="body2"
          sx={{
            mb: 2,
          }}
        >
          {description}
        </Typography>

        {currentItems.map((item, i) => (
          <Box
            key={i}
            sx={{
              display: "flex",
              gap: 2,
              p: 1.2,
              borderRadius: "16px",
            }}
          >
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                width: 36,
                height: 36,
                borderRadius: 0.5,
                background:
                  "linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%)",
                flexShrink: 0,
                boxShadow: (theme) =>
                  `0 4px 12px ${alpha(theme.palette.primary.main, 0.2)}`,
              }}
            >
              {React.isValidElement(item.icon)
                ? React.cloneElement(
                    item.icon as React.ReactElement<{
                      sx?: object;
                      fontSize?: string | number;
                    }>,
                    {
                      sx: { fontSize: 18, color: "#fff" },
                    },
                  )
                : item.icon}
            </Box>

            <Box
              sx={{
                flex: 1,
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
              }}
            >
              <Typography variant="h6">{item.t}</Typography>
              <Typography variant="body2">{item.d}</Typography>
            </Box>
          </Box>
        ))}

        {/* Operation button*/}
        <Stack direction="row" spacing={2} sx={{ mt: 1, alignItems: "center" }}>
          <Box sx={{ flex: 1 }}>
            <Typography
              onClick={onClose}
              sx={{
                textAlign: "center",
                fontSize: "0.85rem",
                fontWeight: 600,
                color: "text.disabled",
                cursor: "pointer",
                py: 1,
                "&:hover": { color: "text.secondary" },
              }}
            >
              {buttonTextClose}
            </Typography>
          </Box>

          <Box sx={{ flex: 2 }}>
            {isDelete ? (
              <Button
                fullWidth
                variant="contained"
                color="error"
                onClick={handleMainAction}
                sx={{
                  borderRadius: 1,
                  textTransform: "none",
                  fontWeight: 700,
                  py: 1.2,
                  boxShadow: "none",
                  "&:hover": {
                    boxShadow: `0 8px 20px ${alpha(theme.palette.error.main, 0.3)}`,
                  },
                }}
              >
                {buttonText}
              </Button>
            ) : (
              <MyButton
                label={buttonText}
                fullWidth
                action={handleMainAction}
              />
            )}
          </Box>
        </Stack>
      </DialogContent>
    </Dialog>
  );
};

export default InfoModal;
