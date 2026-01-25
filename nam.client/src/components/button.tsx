import React from "react";
import { Button, useTheme } from "@mui/material";

export interface MyButtonProps {
  label: string;
  action?: () => void;
  icon?: React.ReactElement;
  disabled?: boolean;
  fullWidth?: boolean;
}

export default function MyButton({
  label,
  action,
  icon,
  disabled,
  fullWidth = false,
}: MyButtonProps) {
  const theme = useTheme();

  return (
    <Button
      variant="contained"
      onClick={action}
      startIcon={icon}
      disabled={disabled}
      fullWidth={fullWidth}
      disableElevation
      sx={{
        background:
          "linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%)",
        color: "white",
        borderRadius: "12px",
        py: 1.2,
        px: 3,
        fontWeight: 600,
        textTransform: "none",
        fontSize: "0.9rem",
        backdropFilter: "blur(4px)",
        border: "1px solid rgba(255, 255, 255, 0.1)",

        transition: "all 0.2s ease-in-out",
        "&:hover": {
          background:
            "linear-gradient(90deg, rgba(21, 93, 252, 0.65) 0%, rgba(152, 16, 250, 0.65) 100%)",
          transform: "translateY(-1px)",
        },
        "&.Mui-disabled": {
          background: theme.palette.action.disabledBackground,
          color: theme.palette.action.disabled,
        },
      }}
    >
      {label}
    </Button>
  );
}
