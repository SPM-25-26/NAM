import { useState } from "react";
import type { IResetData } from "./model/IResetData";
import { useNavigate } from "react-router-dom";
import { buildApiUrl } from "../../../config";
// Importiamo le utility di validazione
import { validatePassword, validateConfirmPassword } from "../../registration/RegistrationValidation";

interface ApiResponse<T = unknown> {
    success: boolean;
    message: string;
    errorCode?: string;
    data?: T;
}

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

    const apiRequest = async <T = unknown>(url: string, body: object): Promise<ApiResponse<T>> => {
        setIsLoading(true);
        setIsError(null);
        try {
            const response = await fetch(url, {
                method: "POST",
                headers,
                body: JSON.stringify(body),
            });

            let responseData: ApiResponse<T>;
            try {
                responseData = (await response.json()) as ApiResponse<T>;
            } catch {
                throw new Error("Invalid server response");
            }

            if (!response.ok || !responseData.success) {
                throw new Error(responseData.message || `Error: ${response.status}`);
            }

            return responseData;
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
        } catch {
            // Errore già gestito dalla apiRequest (che setta isError)
        }
    };

    const handleVerifyCode = async () => {
        const codeString = data.authCode.join("");
        if (data.authCode.some(d => d === "")) {
            setIsError("Please enter the full 6-digit code.");
            return;
        }

        try {
            // CORREZIONE: Il backend vuole "authCode", non "token"
            await apiRequest(buildApiUrl("auth/password-reset/verify"), {
                email: data.email,
                authCode: codeString,
            });
            setActiveStep(2);
        } catch {
            // Stop
        }
    };

    const handleResetPassword = async () => {
        // Validazione lato client prima della chiamata
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
            // CORREZIONE: Usiamo "authCode" anche qui
            await apiRequest(buildApiUrl("auth/password-reset"), {
                email: data.email,
                authCode: data.authCode.join(""),
                newPassword: data.newPassword,
                confirmPassword: data.confirmPassword,
            });
            setActiveStep(3);
        } catch {
            // Stop
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