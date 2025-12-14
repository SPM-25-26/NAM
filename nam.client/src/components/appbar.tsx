import React from "react";
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  IconButton,
  useTheme,
} from "@mui/material";
import { Logout } from "@mui/icons-material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { useNavigate } from "react-router-dom";

interface MyAppBarProps {
  title: string;
  back?: boolean;
  icon?: React.ReactNode;
}

const MyAppBar: React.FC<MyAppBarProps & { logout?: () => Promise<void> }> = ({
  title,
  back = false,
  logout,
  icon,
}) => {
  const theme = useTheme();
  const navigate = useNavigate();

  return (
    <AppBar
      position="static"
      sx={{ background: "transparent", boxShadow: "none" }}
    >
      <Toolbar sx={{ color: theme.palette.primary.main }}>
        <Box sx={{ display: "flex", alignItems: "center", width: "100%" }}>
          {back && (
            <IconButton
              edge="start"
              color="inherit"
              aria-label="back"
              onClick={() => navigate(-1)}
            >
              <ArrowBackIcon />
            </IconButton>
          )}

          {/* Centered title + icon */}
          <Box
            sx={{
              position: "absolute",
              left: "50%",
              transform: "translateX(-50%)",
              display: "flex",
              alignItems: "center",
              gap: 1,
              pointerEvents: "none",
              maxWidth: "60%",
            }}
          >
            {icon && (
              <Box
                sx={{
                  display: "flex",
                  alignItems: "center",
                }}
              >
                {icon}
              </Box>
            )}

            <Typography
              variant="h6"
              noWrap
              sx={{ color: theme.palette.primary.main }}
            >
              {title}
            </Typography>
          </Box>
        </Box>

        {logout && (
          <IconButton onClick={logout} aria-label="Logout" color="error">
            <Logout />
          </IconButton>
        )}
      </Toolbar>
    </AppBar>
  );
};

export default MyAppBar;
