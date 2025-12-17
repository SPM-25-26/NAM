import { Box, CircularProgress, Typography } from "@mui/material";

export const loadingView = (
  <Box
    display="flex"
    flexDirection="column"
    alignItems="center"
    justifyContent="center"
    sx={{ height: "80vh", p: 4 }}
  >
    <CircularProgress size={50} sx={{ mb: 2 }} />
    <Typography variant="h6" color="text.secondary">
      Loading
    </Typography>
  </Box>
);
