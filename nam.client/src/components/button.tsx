import React from "react";
import Button from "@mui/material/Button";

export interface MyButtonProps {
    label: string;
    action?: () => void;
    icon?: React.ReactElement;
}

export default function MyButton({ label, action, icon }: MyButtonProps) {
    return (
        <Button variant="contained"
            onClick={action}
            startIcon={icon}
            sx={{
                background: "linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%)",
                color: "white",
            }}>
            {label}
        </Button>
    );
}
