import { useState } from "react";
import type { IResetData } from "./model/IResetData";
import { useNavigate } from "react-router-dom";
import { buildApiUrl } from "../../../config";
import { validatePassword, validateConfirmPassword } from "../../registration/RegistrationValidation";

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
        if (isError) setIsError(null);
    };

    const headers = {
        "Content-Type": "application/json",
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
                // Try to extract the error message from the ProblemDetails format
                let errorMessage = `Error: ${response.status}`;
                try {
                    const errorJson = await response.json();
                    if (errorJson && typeof errorJson.detail === "string") {
                        errorMessage = errorJson.detail;
                    }
                } catch {
                    // If the body is not valid JSON, keep the default message
                }
                throw new Error(errorMessage);
            }

            // Return the success JSON if needed
            return await response.json();
        } catch (err: unknown) {
            if (err instanceof Error) {
                setIsError(err.message);
            } else {
                setIsError("An unknown error occurred");
            }
            throw err;
        } finally {
            setIsLoading(false);
        }
    };

    const handleSendEmail = async () => {
        if (!data.email.includes("@")) {
            setIsError("Invalid email format");
            return;
        }
        try {
            await apiRequest(buildApiUrl("auth/password-reset/request"), {
                email: data.email,
            });
            setActiveStep(1);
        } catch (error) {
            console.error(error);
        }
    };

    const handleVerifyCode = async () => {
        const codeString = data.authCode.join("");
        if (data.authCode.some(d => d === "")) {
            setIsError("Please enter the full 6-digit code.");
            return;
        }

        try {
            await apiRequest(buildApiUrl("auth/password-reset/verify"), {
                email: data.email,
                authCode: codeString,
            });
            setActiveStep(2);
        } catch (error) {
            console.error(error);
        }
    };

    const handleResetPassword = async () => {
        const passwordError = validatePassword(data.newPassword);
        if (passwordError) {
            setIsError(passwordError);
            return;
        }

        const confirmError = validateConfirmPassword(data.newPassword, data.confirmPassword);
        if (confirmError) {
            setIsError(confirmError);
            return;
        }

        try {
            await apiRequest(buildApiUrl("auth/password-reset"), {
                email: data.email,
                authCode: data.authCode.join(""),
                newPassword: data.newPassword,
                confirmPassword: data.confirmPassword,
            });
            setActiveStep(3);
        } catch (error) {
            console.error(error);
        }
    };

    const handleGoToLogin = () => {
        navigate("/login");
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