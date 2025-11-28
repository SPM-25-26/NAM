import React from "react";
import Button from "@mui/material/Button";

export interface MyButtonProps {
  label: string;
  action?: () => void;
  icon?: React.ReactElement;
}

export default function MyButton({ label, action, icon }: MyButtonProps) {
  return (
    <Button variant="contained" onClick={action} startIcon={icon}>
      {label}
    </Button>
  );
}
