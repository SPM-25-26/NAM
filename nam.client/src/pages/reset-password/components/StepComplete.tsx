import React from "react";
import { Box, Typography, useTheme} from "@mui/material";
import { CheckCircleOutline } from "@mui/icons-material";
import MyButton from "../../../components/button";

interface StepCompleteProps {
  handleGoToLogin: () => void;
}


const StepComplete: React.FC<StepCompleteProps> = ({ handleGoToLogin }) => {
    const theme = useTheme();
    return (
        <Box sx={{ width: "100%", maxWidth: 400, textAlign: "center", backgroundColor: theme.palette.background.paper }}>
      <CheckCircleOutline color="success" sx={{ fontSize: 80, mb: 2 }} />
      <Typography variant="h4" sx={{ mb: 1 }}>
        Password Updated
      </Typography>
      <Typography variant="body1" sx={{ mb: 3, color: "text.secondary" }}>
        You’ve successfully updated your password.
      </Typography>

      <MyButton label="Go to Login" action={handleGoToLogin} />
    </Box>
  );
};

export default StepComplete;
