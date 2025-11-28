import React from "react";
import { AppBar, Toolbar, Typography, Box, IconButton } from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { Link } from "react-router-dom";

interface MyAppBarProps {
  title: string;
  backUrl: string;
}

const MyAppBar: React.FC<MyAppBarProps> = ({ title, backUrl }) => {
  const showBackButton = backUrl && backUrl !== "";

  return (
    <AppBar position="static">
      <Toolbar>
        <Box sx={{ display: "flex", alignItems: "center", width: "100%" }}>
          {showBackButton && (
            <IconButton
              edge="start"
              color="inherit"
              aria-label="back"
              component={Link}
              to={backUrl}
            >
              <ArrowBackIcon />
            </IconButton>
          )}

          <Typography
            variant="h6"
            sx={{
              flexGrow: 1,
              textDecoration: "none",
            }}
            color="inherit"
          >
            {title}
          </Typography>
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default MyAppBar;
