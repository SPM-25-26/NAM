import React from "react";
import { Box, Typography, Alert, useTheme } from "@mui/material";

import MyButton from "../../../components/button";
import FormBox from "../../../components/FormBox";
import { ModeEditOutline } from "@mui/icons-material";
import LockIcon from '@mui/icons-material/Lock';

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
    const theme = useTheme();
    return (
        <Box sx={{
            width: "100%", maxWidth: 400, textAlign: "center", display: "flex", flexDirection: "column", gap: 2}}>
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

            <FormBox
                name="password"
                label="New Password"
                value={password}
                onChange={(e) => updatePassword(e.target.value)}
                placeholder={"Enter your password"}
                type="password"
                icon={<LockIcon />}
                iconColor={theme.palette.text.disabled}
                showPasswordToggle
            />
            <FormBox
                name="password"
                label="Confirm Password"
                value={confirmPassword}
                type="password"
                onChange={(e) => updateConfirmPassword(e.target.value)}
                placeholder={"Enter password"}
                icon={<LockIcon />}
                iconColor={theme.palette.text.disabled}
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