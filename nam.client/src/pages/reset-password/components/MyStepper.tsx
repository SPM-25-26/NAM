import React from "react";
import { Box, Stepper, Step, StepLabel } from "@mui/material";
import { usePasswordReset } from "../hooks/usePasswordReset";
import StepEmail from "./StepEmail";
import StepVerification from "./StepVerification";
import StepNewPassword from "./StepNewPassword";
import StepComplete from "./StepComplete";

const steps = ["Email", "Verify Code", "New Password", "Completed"];

const getStepContent = (
  step: number,
  state: ReturnType<typeof usePasswordReset>
) => {
  const updateDataField = (
    field: keyof typeof state.data,
    value: string | string[]
  ) => {
    state.updateData(field, value);
  };

  switch (step) {
    case 0:
      return (
        <StepEmail
          email={state.data.email}
          isError={state.isError}
          isLoading={state.isLoading}
          updateEmail={(val) => updateDataField("email", val)}
          handleSendEmail={state.handleSendEmail}
        />
      );
    case 1:
      return (
        <StepVerification
          code={state.data.authCode}
          isError={state.isError}
          isLoading={state.isLoading}
          updateCode={(val) => updateDataField("authCode", val)}
          handleVerifyCode={state.handleVerifyCode}
          handleResendCode={state.handleSendEmail}
          email={state.data.email}
        />
      );
    case 2:
      return (
        <StepNewPassword
          password={state.data.newPassword}
          confirmPassword={state.data.confirmPassword}
          isError={state.isError}
          isLoading={state.isLoading}
          updatePassword={(val) => updateDataField("newPassword", val)}
          updateConfirmPassword={(val) =>
            updateDataField("confirmPassword", val)
          }
          handleResetPassword={state.handleResetPassword}
        />
      );
    case 3:
      return <StepComplete handleGoToLogin={state.handleGoToLogin} />;
    default:
      return null;
  }
};

const MyStepper: React.FC = () => {
  const state = usePasswordReset();
  const { activeStep } = state;

  return (
    <Box sx={{ width: "100%", maxWidth: 600, margin: "0 auto", p: 2 }}>
      <Stepper
        activeStep={activeStep}
        alternativeLabel
        sx={{ mb: 4 }}
        connector={null}
      >
        {steps.map((label, index) => (
          <Step key={index} completed={activeStep > index}>
            <StepLabel>{label}</StepLabel>
          </Step>
        ))}
      </Stepper>
      <Box
        sx={{
          minHeight: "400px",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          p: 4,
        }}
      >
        {getStepContent(activeStep, state)}
      </Box>
    </Box>
  );
};

export default MyStepper;
