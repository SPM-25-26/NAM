import { Box, Stack, Button, Typography } from "@mui/material";
import type { StepConfig } from "./stepConfig";
import MyButton from "../button";

export type MyStepperProps<TState extends { activeStep: number }> = {
  state: TState;
  steps: StepConfig<TState>[];
  variant?: "stepper" | "progress";
  showLabel?: boolean;
  onNext: () => void;
  onBack: () => void;
  onsubmit: () => Promise<void>;
};

export function MyStepper<TState extends { activeStep: number }>({
  state,
  steps,
  variant = "progress",
  onNext,
  onsubmit,
  onBack,
}: MyStepperProps<TState>) {
    const { activeStep } = state;
    const currentStepConfig = steps[activeStep];

  return (
    <Box
      sx={{
        height: "80vh",
        display: "flex",
        flexDirection: "column",
        overflow: "hidden",
        boxSizing: "border-box",
      }}
    >
      <Box sx={{ pt: 2, flexShrink: 0 }}>
        {variant === "progress" && (
          <Stack direction="row" spacing={1.5}>
            {steps.map((_, index) => (
              <Box
                key={index}
                sx={{
                  height: 8,
                  flex: 1,
                  borderRadius: 4,
                  bgcolor: index <= activeStep ? "#AD99FF" : "#E0E0E0",
                }}
              />
            ))}
          </Stack>
        )}
      </Box>
      {/* 2. Question Header */}
      <Box sx={{ textAlign: "center", mt: 1, mb: 1 }}>
        <Typography variant="h4">{steps[activeStep]?.label}</Typography>
        <Typography variant="body1" color="text.secondary">
          {steps[activeStep]?.description}
        </Typography>
      </Box>
      {/* Section scrollable */}
      <Box
        sx={{
          flexGrow: 1,
          overflowY: "auto",
          overflowX: "hidden",
          display: "flex",
          flexDirection: "column",
          width: "100%",
          padding: 1,
          boxSizing: "border-box",
        }}
      >
        {steps[activeStep]?.render(state)}
      </Box>

      {/* 3. FOOTER (navigation) */}
      <Box
        sx={{
          width: "100%",
          padding: 2,
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          flexShrink: 0,
          boxSizing: "border-box",
        }}
      >
        <Button
          disabled={activeStep === 0}
          onClick={onBack}
          sx={{ color: "text.primary" }}
        >
          Undo
        </Button>

        <MyButton
          label={activeStep === steps.length - 1 ? "Complete" : "Continue"}
          disabled={currentStepConfig?.disableNext}
          action={async () => {
            if (activeStep === steps.length - 1) {
              await onsubmit();
              onNext();
            } else {
              onNext();
            }
          }}
        />
      </Box>
    </Box>
  );
}
