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
}

const MyAppBar: React.FC<MyAppBarProps & { logout?: () => Promise<void> }> = ({
  title,
  back = false,
  logout,
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

          <Typography
            variant="h6"
            color="inherit"
            sx={{
              position: "absolute",
              left: "50%",
              transform: "translateX(-50%)",
              textAlign: "center",
              pointerEvents: "none",
              width: "60%",
              whiteSpace: "nowrap",
              overflow: "hidden",
              textOverflow: "ellipsis",
            }}
          >
            {title}
          </Typography>
        </Box>

        {logout && (
          <IconButton onClick={logout} aria-label="Logout" color="primary">
            <Logout />
          </IconButton>
        )}
      </Toolbar>
    </AppBar>
  );
};

export default MyAppBar;
