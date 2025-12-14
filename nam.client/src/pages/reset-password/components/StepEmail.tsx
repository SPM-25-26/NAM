import React from "react";
import { Typography, Box, Alert } from "@mui/material";
import { EmailOutlined, SendOutlined } from "@mui/icons-material";
import MyButton from "../../../components/button";
import MyInputField from "../../../components/input_field/input_field";

interface StepEmailProps {
  email: string;
  isError: string | null;
  isLoading: boolean;
  updateEmail: (value: string) => void;
  handleSendEmail: () => void;
}

const StepEmail: React.FC<StepEmailProps> = ({
  email,
  isError,
  isLoading,
  updateEmail,
  handleSendEmail,
}) => {
    return (
        <Box sx={{ width: "100%", maxWidth: 400, textAlign: "center" }}>
      <Typography variant="h5" sx={{ mb: 1 }}>
        Enter your Email
      </Typography>
      <Typography variant="body1" sx={{ mb: 3, color: "text.secondary" }}>
        Add your email address and we will send you instructions on how to
        recover your password.
      </Typography>

      {isError && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {isError}
        </Alert>
      )}

      <MyInputField
        label="Email"
        placeholder="nome@esempio.it"
        value={email}
        onChange={(e) => updateEmail(e.target.value)}
        IconComponent={EmailOutlined}

      />

      <MyButton
        label={isLoading ? "send..." : "Send email"}
        icon={<SendOutlined />}
        action={handleSendEmail}
      />
    </Box>
  );
};

export default StepEmail;
