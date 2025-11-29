import React from "react";
import { Box, TextField, Typography } from "@mui/material";

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
    iconColor = "#000",
}) => {
    return (
        <Box>
            <Typography
                variant="subtitle2"
                sx={{
                    fontWeight: 600,
                    marginBottom: 1,
                    color: "#000",
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
                    backgroundColor: "#f5f5f5",
                    borderRadius: 1,
                    "& .MuiOutlinedInput-root": {
                        "& fieldset": {
                            borderColor: error ? "#d32f2f" : "#e0e0e0",
                        },
                    },
                    "& .MuiInputBase-input:-webkit-autofill": {
                        WebkitBoxShadow: "0 0 0 100px #f5f5f5 inset",
                        WebkitTextFillColor: "#000000",
                    },
                }}
                slotProps={{
                    input: {
                        startAdornment: icon ? (
                            <Typography sx={{ marginRight: 1, color: iconColor }}>
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