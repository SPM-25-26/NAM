import React, { useState, useRef, useEffect } from "react";
import { Box, Typography, TextField, Button, Alert, useTheme} from "@mui/material";
import MyButton from "../../../components/button";

const RESEND_TIME_SECONDS = 60 * 15;

interface IStepVerificationProps {
  code: string[];
  isError: string | null;
  isLoading: boolean;
  email: string;
  updateCode: (codeArray: string[]) => void;
  handleVerifyCode: () => void;
  handleResendCode: () => void;
}

const StepVerification: React.FC<IStepVerificationProps> = ({
  code,
  isError,
  isLoading,
  updateCode,
  handleVerifyCode,
  handleResendCode,
  email,
}) => {
  const [timer, setTimer] = useState(RESEND_TIME_SECONDS);
  const inputRefs = useRef<Array<HTMLInputElement | null>>([]);

  useEffect(() => {
    if (timer > 0 && !isLoading) {
      const interval = setInterval(() => {
        setTimer((prevTime) => prevTime - 1);
      }, 1000);
      return () => clearInterval(interval);
    }
  }, [timer, isLoading]);

  const handleResend = () => {
    handleResendCode();
    setTimer(RESEND_TIME_SECONDS);
    updateCode(new Array(6).fill(""));
    inputRefs.current[0]?.focus();
  };

  const handleChange = (element: HTMLInputElement, index: number) => {
    if (isNaN(Number(element.value))) return;

    const newCode = [...code];
    newCode[index] = element.value.slice(-1);
    updateCode(newCode);

    if (element.value !== "" && index < 5) {
      inputRefs.current[index + 1]?.focus();
    }
  };

  const handleKeyDown = (
    e: React.KeyboardEvent<HTMLInputElement>,
    index: number
  ) => {
    if (e.key === "Backspace" && code[index] === "" && index > 0) {
      e.preventDefault();
      inputRefs.current[index - 1]?.focus();
    }
  };

  const formatTimer = (seconds: number) => {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, "0")}:${secs
      .toString()
      .padStart(2, "0")}`;
  };

    const theme = useTheme();
  return (
    <Box sx={{ width: "100%", maxWidth: 400, textAlign: "center" }}>
      <Typography variant="h5" sx={{ mb: 1 }}>
        Enter confirmation code
      </Typography>
      <Typography variant="body1" sx={{ mb: 3, color: "text.secondary" }}>
        A 6-digit code was sent to {email}
      </Typography>

      {isError && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {isError}
        </Alert>
      )}

      <Box sx={{ display: "flex", justifyContent: "center", gap: 1, mb: 2 }}>
        {code.map((digit, index) => (
          <TextField
            key={index}
            inputRef={(el) => (inputRefs.current[index] = el)}
            value={digit}
            onChange={(e) => handleChange(e.target as HTMLInputElement, index)}
            variant="outlined"
            size="small"
            type="tel"
            error={isError !== null}
            InputProps={{
              onKeyDown: (e) =>
                handleKeyDown(
                  e as React.KeyboardEvent<HTMLInputElement>,
                  index
                ),
            }}
            inputProps={{
              maxLength: 1,
              style: { textAlign: "center", padding: "10px 0" },
                }}
                sx={{
                    width: "40px", "& .MuiOutlinedInput-root": {
                        backgroundColor: theme.palette.background.default// Colora solo l'interno arrotondato
                } }}
          />
        ))}
      </Box>

      <Box display="flex" alignItems="center" justifyContent="center" gap={2}>
        <Typography variant="body2" sx={{ color: "text.secondary" }}>
          {timer > 0 ? `Expires in: ${formatTimer(timer)}` : "The code expired"}
        </Typography>

        <Button
          variant="text"
          disabled={timer > 0 || isLoading}
          onClick={handleResend}
        >
          Invia Nuovo Codice
        </Button>
      </Box>

      <MyButton
        label={isLoading ? "Verification..." : "Verification Code"}
        action={handleVerifyCode}
      />
    </Box>
  );
};

export default StepVerification;
