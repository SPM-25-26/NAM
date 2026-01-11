import React from "react";
import { Box, Typography, Paper, Radio } from "@mui/material";
import { alpha, styled, useTheme } from "@mui/material/styles";

export interface CustomSelectionCardProps {
  label: string;
  description: string;
  value: string | number;
  selectedValue: string | number;
  onChange: (value: string | number) => void;
}

const StyledOption = styled(Paper, {
  shouldForwardProp: (prop) => prop !== "selected",
})<{ selected?: boolean }>(({ selected, theme }) => ({
  padding: theme.spacing(2),
  borderRadius: 20,
  display: "flex",
  alignItems: "center",
  justifyContent: "flex-start",
  cursor: "pointer",
  transition: theme.transitions.create([
    "border-color",
    "background-color",
    "transform",
  ]),
  border: "2px solid",
  flex: 1,
  width: "90%",

  borderColor: selected ? theme.palette.primary.main : theme.palette.divider,
  background: selected
    ? `linear-gradient(90deg, ${alpha(
        theme.palette.primary.main,
        0.08
      )} 0%, ${alpha(theme.palette.secondary.main, 0.08)} 100%)`
    : theme.palette.background.paper,

  "&:hover": {
    transform: "translateY(-2px)",
    borderColor: theme.palette.primary.light,
    backgroundColor: alpha(theme.palette.primary.main, 0.02),
  },
}));

export const CustomSelectionCard: React.FC<CustomSelectionCardProps> = ({
  label,
  description,
  value,
  selectedValue,
  onChange,
}) => {
  const theme = useTheme();
  const isSelected = selectedValue === value;

  const radioGradient =
    "linear-gradient(90deg, rgba(21, 93, 252, 1) 0%, rgba(152, 16, 250, 1) 100%)";

  return (
    <StyledOption
      selected={isSelected}
      onClick={() => onChange(value)}
      elevation={isSelected ? 2 : 0}
    >
      <Radio
        checked={isSelected}
        sx={{
          padding: 0,
          marginRight: 2,
          color: theme.palette.divider,
          "&.Mui-checked": {
            color: theme.palette.primary.main,
          },

          "&.Mui-checked .MuiSvgIcon-root": {
            background: radioGradient,
            borderRadius: "50%",
            color: "white",
            padding: "2px",
          },
        }}
      />
      <Box sx={{ textAlign: "left" }}>
        <Typography
          variant="body1"
          sx={{
            fontWeight: 800,
            color: "text.primary",
            lineHeight: 1.2,
          }}
        >
          {label}
        </Typography>
        <Typography
          variant="caption"
          sx={{
            color: "text.secondary",
            display: "block",
            mt: 0.5,
          }}
        >
          {description}
        </Typography>
      </Box>
    </StyledOption>
  );
};
