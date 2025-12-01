import { useState } from "react";
import type { IResetData } from "./model/IResetData";
import { useNavigate } from "react-router-dom";
import { buildApiUrl } from "../../../config";

export const usePasswordReset = () => {
  const navigate = useNavigate();
  const [activeStep, setActiveStep] = useState(0);
  const [data, setData] = useState<IResetData>({
    email: "",
    authCode: new Array(6).fill(""),
    newPassword: "",
    confirmPassword: "",
  });
  const [isLoading, setIsLoading] = useState(false);
  const [isError, setIsError] = useState<string | null>(null);

  const updateData = (field: keyof IResetData, value: string | string[]) => {
    setData((prev) => ({ ...prev, [field]: value }));
    setIsError(null);
  };
  const headers = {
    "Content-Type": "application/json",
  };

  const handleSendEmail = async () => {
    if (!data.email.includes("@")) {
      setIsError("Invalid email format");
      return;
    }
    await apiRequest(buildApiUrl("/auth/password-reset/request/"), {
      email: data.email,
    });
    setActiveStep(1);
  };

  const handleVerifyCode = async () => {
    if (data.authCode.length !== 6) return;
    await apiRequest(buildApiUrl("/auth/password-reset/verify/"), {
      authCode: data.authCode.join(""),
    });
    setActiveStep(2);
  };

  const handleResetPassword = async () => {
    if (data.newPassword !== data.confirmPassword) {
      setIsError("Passwords do not match.");
      return;
    }

    if (data.newPassword.length < 8) {
      setIsError("Password must be at least 8 characters long.");
      return;
    }
    await apiRequest(buildApiUrl("/auth/password-reset/"), {
      AuthCode: data.authCode.join(""),
      NewPassword: data.newPassword,
      ConfirmPassword: data.confirmPassword,
    });
    setActiveStep(3);
  };

  const handleGoToLogin = () => {
    navigate("/");
  };

  const apiRequest = async (url: string, body: object) => {
    setIsLoading(true);
    setIsError(null);
    try {
      const response = await fetch(url, {
        method: "POST",
        headers,
        body: JSON.stringify(body),
      });
      if (!response.ok) {
        const errorData = await response.json().catch(() => ({}));
        throw new Error(errorData?.message || `HTTP error: ${response.status}`);
      }
      return await response.json().catch(() => null);
    } catch (err: unknown) {
      if (err instanceof Error) {
        setIsError(err.message);
      } else {
        setIsError("Unknown error occurred");
      }
      throw err;
    } finally {
      setIsLoading(false);
    }
  };
  return {
    activeStep,
    data,
    isLoading,
    isError,
    updateData,
    handleSendEmail,
    handleVerifyCode,
    handleResetPassword,
    handleGoToLogin,
    setActiveStep,
  };
};
