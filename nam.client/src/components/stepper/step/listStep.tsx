import { Box, Paper } from "@mui/material";
import { CustomSelectionCard } from "../components/customButtonRadio";

interface Option {
  id: string;
  label: string;
  description: string;
}

interface StepListProps {
  options: Option[];
  selectedValue?: string[];
  onSelect: (id: string) => void;
}

export function StepList({
  options,
  selectedValue = [],
  onSelect,
}: StepListProps) {
  return (
    <Box sx={{ flexDirection: "column" }}>
      <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
        {options.map((option) => {
          return (
            <Paper
              key={option.id}
              onClick={() => onSelect(option.id)}
              elevation={0}
            >
              <CustomSelectionCard
                label={option.label}
                description={option.description}
                value={option.id}
                selectedValue={
                  selectedValue.includes(option.id) ? option.id : ""
                }
                onChange={() => onSelect(option.id)}
              />
            </Paper>
          );
        })}
      </Box>
    </Box>
  );
}
