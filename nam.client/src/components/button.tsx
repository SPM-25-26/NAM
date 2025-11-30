import React from "react";
import Button from "@mui/material/Button";

export interface MyButtonProps {
    label: string;
    action?: () => void;
    icon?: React.ReactElement;
    disabled?: boolean;
}

export default function MyButton({ label, action, icon, disabled }: MyButtonProps) {
    return (
        <Button variant="contained"
            onClick={action}
            startIcon={icon}
            disabled={disabled}
            sx={{
                background: "linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%)",
                color: "white",
            }}>
            {label}
        </Button>
    );
}
