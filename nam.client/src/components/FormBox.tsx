import React from "react";
import { Box, TextField, Typography, useTheme } from "@mui/material";

interface FormBoxProps {
    label: string;
    name: string;
    type?: string;
    placeholder?: string;
    value: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    error?: string;
    disabled?: boolean;
    icon?: React.ReactNode;
    iconColor?: string;
}

const FormBox: React.FC<FormBoxProps> = ({
    label,
    name,
    type = "text",
    placeholder,
    value,
    onChange,
    error,
    disabled = false,
    icon,
    iconColor,
}) => {
    const theme = useTheme();

    return (
        <Box>
            <Typography
                variant="subtitle2"
                sx={{
                    fontWeight: 600,
                    marginBottom: 1,
                    color: theme.palette.text.primary,
                }}
            >
                {label}
            </Typography>
            <TextField
                fullWidth
                name={name}
                type={type}
                placeholder={placeholder}
                value={value}
                onChange={onChange}
                variant="outlined"
                error={!!error}
                helperText={error}
                disabled={disabled}
                sx={{
                    backgroundColor: theme.palette.background.default,
                    borderRadius: 1,
                    "& .MuiOutlinedInput-root": {
                        "& fieldset": {
                            borderColor: error ? theme.palette.error.main : "#e0e0e0",
                        },
                    },
                    "& .MuiInputBase-input:-webkit-autofill": {
                        WebkitBoxShadow: `0 0 0 100px ${theme.palette.background.default} inset`,
                        WebkitTextFillColor: theme.palette.text.primary,
                    },
                }}
                slotProps={{
                    input: {
                        startAdornment: icon ? (
                            <Typography sx={{ marginRight: 1, color: iconColor || theme.palette.text.primary }}>
                                {icon}
                            </Typography>
                        ) : undefined,
                    },
                }}
            />
        </Box>
    );
};

export default FormBox;
