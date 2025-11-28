import React from "react";
import { Box, Typography, Alert } from "@mui/material";

import MyButton from "../../../components/button";
import MyInputField from "../../../components/input_field/input_field";
import { ModeEditOutline, PasswordOutlined } from "@mui/icons-material";

interface StepNewPasswordProps {
  password: string;
  confirmPassword: string;
  isError: string | null;
  isLoading: boolean;
  updatePassword: (value: string) => void;
  updateConfirmPassword: (value: string) => void;
  handleResetPassword: () => void;
}

const StepNewPassword: React.FC<StepNewPasswordProps> = ({
  confirmPassword,
  password,
  isError,
  isLoading,
  updateConfirmPassword,
  updatePassword,
  handleResetPassword,
}) => {
  return (
    <Box sx={{ width: "100%", maxWidth: 400, textAlign: "center" }}>
      <Typography variant="h5" sx={{ mb: 1 }}>
        Update password
      </Typography>
      <Typography variant="body1" sx={{ mb: 3, color: "text.secondary" }}>
        Select a new password and confirm it. Once saved, you will be able to
        log back into your account without any problems.
      </Typography>

      {isError && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {isError}
        </Alert>
      )}

      <MyInputField
        label="New Password"
        value={password}
        onChange={(e) => updatePassword(e.target.value)}
        placeholder={"Enter your password"}
        type="password"
        IconComponent={PasswordOutlined}
      />
      <MyInputField
        label="Confirm Password"
        value={confirmPassword}
        type="password"
        onChange={(e) => updateConfirmPassword(e.target.value)}
        placeholder={"Enter password"}
        IconComponent={PasswordOutlined}
      />

      <MyButton
        label={isLoading ? "Resetting..." : "Update Password"}
        action={handleResetPassword}
        icon={<ModeEditOutline />}
      />
    </Box>
  );
};

export default StepNewPassword;
