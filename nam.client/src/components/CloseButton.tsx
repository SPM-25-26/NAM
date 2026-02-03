import { IconButton, Tooltip } from "@mui/material";
import CloseRoundedIcon from "@mui/icons-material/CloseRounded";
import { alpha, useTheme } from "@mui/material/styles";

type CloseButtonProps = {
  action: () => void;
  size?: "small" | "medium" | "large";
};

export function CloseButton({ action, size = "small" }: CloseButtonProps) {
  const theme = useTheme();

  return (
    <Tooltip title="Chiudi">
      <IconButton
        onClick={action}
        size={size}
        sx={{
          color: theme.palette.error.main,
          bgcolor: alpha(theme.palette.error.main, 0.08),
          transition: "all 0.2s cubic-bezier(0.4, 0, 0.2, 1)",
          "&:hover": {
            color: "#fff",
            bgcolor: theme.palette.error.main,
            transform: "rotate(90deg) scale(1.1)",
          },
        }}
      >
        <CloseRoundedIcon fontSize="small" />
      </IconButton>
    </Tooltip>
  );
}
