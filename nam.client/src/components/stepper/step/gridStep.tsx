import { Box, Typography, Grid, Paper, ButtonBase } from "@mui/material";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";

interface Option {
  id: string;
  label: string;
  description: string;
  image?: string;
}

interface StepGridProps {
  options: Option[];
  selectedValue?: string[];
  onSelect: (id: string) => void;
  onBack?: () => void;
}

export function StepGrid({ options, selectedValue, onSelect }: StepGridProps) {
  return (
    <Box sx={{ width: "100%" }}>
      <Grid container spacing={2}>
        {options.map((option) => {
          const isSelected = selectedValue?.includes(option.id);

          return (
            <Grid key={option.id} size={{ xs: 6, sm: 6, md: 4 }} display="flex">
              <Paper
                component={ButtonBase}
                onClick={() => onSelect(option.id)}
                elevation={isSelected ? 4 : 0}
                sx={{
                  p: 3,
                  width: "100%",
                  position: "relative",
                  display: "flex",
                  flexDirection: "column",
                  alignItems: "center",
                  textAlign: "center",
                  borderRadius: 2,
                  border: "2px solid",
                  borderColor: isSelected ? "#AD99FF" : "#F0F0F0",
                  bgcolor: isSelected ? "#F9F8FF" : "white",
                  transition: "all 0.3s ease",
                  "&:hover": {
                    borderColor: "primary.light",
                    transform: "translateY(-4px)",
                  },
                }}
              >
                {/* Section icon */}
                {isSelected && (
                  <CheckCircleIcon
                    color="primary"
                    sx={{
                      position: "absolute",
                      top: 12,
                      right: 12,
                    }}
                  />
                )}

                {/* Image */}
                <Box
                  sx={{
                    width: 80,
                    height: 80,
                    mb: 2,
                    borderRadius: "20%",
                    overflow: "hidden",
                    bgcolor: "grey.100",
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                  }}
                >
                  {option.image ? (
                    <Box
                      component="img"
                      src={option.image}
                      sx={{ width: "100%", height: "100%", objectFit: "cover" }}
                    />
                  ) : (
                    <Typography color="grey.400">?</Typography>
                  )}
                </Box>

                <Typography variant="subtitle1" sx={{ fontWeight: 700 }}>
                  {option.label}
                </Typography>

                <Typography variant="body2" color="text.secondary">
                  {option.description}
                </Typography>
              </Paper>
            </Grid>
          );
        })}
      </Grid>
    </Box>
  );
}
