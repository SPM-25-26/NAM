export const selectionGradient =
  "linear-gradient(90deg, rgba(21, 93, 252, 0.50) 0%, rgba(152, 16, 250, 0.50) 100%)";

import { Box, Chip, useTheme, alpha } from "@mui/material";
import CheckIcon from "@mui/icons-material/Check";

interface Option {
  id: string;
  label: string;
}

interface StepChipSelectionProps {
  options: Option[];
  selectedValue?: string[];
  onSelect: (id: string) => void;
}

export function StepChipSelection({
  options,
  selectedValue = [],
  onSelect,
}: StepChipSelectionProps) {
  const theme = useTheme();

  return (
    <Box
      sx={{
        width: "100%",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        py: { xs: 2, sm: 4 },
      }}
    >
      <Box
        sx={{
          display: "flex",
          flexWrap: "wrap",
          justifyContent: "center",
          gap: { xs: 1, sm: 1.5 },
          width: "100%",
          maxWidth: "md",
        }}
      >
        {options.map((option) => {
          const isSelected = selectedValue.includes(option.id);

          return (
            <Chip
              key={option.id}
              label={option.label}
              onClick={() => onSelect(option.id)}
              icon={
                isSelected ? (
                  <CheckIcon
                    sx={{
                      color: "white !important",
                      fontSize: "1.1rem",
                    }}
                  />
                ) : undefined
              }
              sx={{
                px: { xs: 1, sm: 1.5 },
                py: { xs: 2.5, sm: 3 },
                fontSize: { xs: "0.9rem", sm: "1rem" },
                fontWeight: 600,
                borderRadius: 1.5,
                cursor: "pointer",

                ...(isSelected
                  ? {
                      // --- Style Section ---
                      background: selectionGradient,
                      color: "white",
                      borderColor: "transparent",
                      boxShadow: `0 4px 15px ${alpha(
                        theme.palette.primary.main,
                        0.25
                      )}`,
                    }
                  : {
                      // --- Style No selection ---
                      bgcolor: alpha(theme.palette.text.primary, 0.04),
                      color: "text.secondary",
                      borderColor: alpha(theme.palette.divider, 0.1),
                    }),
                border: "1px solid",
              }}
            />
          );
        })}
      </Box>
    </Box>
  );
}
