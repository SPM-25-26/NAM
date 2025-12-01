import { useState } from "react";
import EmailIcon from '@mui/icons-material/Email';
import LockIcon from '@mui/icons-material/Lock';
import FlightIcon from '@mui/icons-material/Flight';
import {
    Box,
    Card,
    FormControlLabel,
    Checkbox,
    Link,
    Typography,
    Container,
    useTheme,
} from "@mui/material";
import MyAppBar from "../../components/appbar";
import MyButton from "../../components/button";
import FormBox from "../../components/FormBox";
import { validateRegistration } from "./RegistrationValidation";
import type { ValidationErrors } from "./RegistrationValidation";
import { buildApiUrl } from '../../config';

const RegistrationPage: React.FC = () => {
    const theme = useTheme();
    const [formData, setFormData] = useState({
        email: "",
        password: "",
        confirmPassword: "",
        agreeToTerms: false,
    });

    const [errors, setErrors] = useState<ValidationErrors>({});
    const [isLoading, setIsLoading] = useState(false);
    const [apiError, setApiError] = useState<string | null>(null);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, type, checked } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));
        // Clear error for this field when user starts typing
        if (errors[name as keyof ValidationErrors]) {
            setErrors((prev) => ({
                ...prev,
                [name]: undefined,
            }));
        }
        // Clear API error when user modifies form
        if (apiError) {
            setApiError(null);
        }
    };

    const registerUser = async () => {
        try {
            setIsLoading(true);
            setApiError(null);

            const response = await fetch(buildApiUrl('auth/register'), {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: formData.email,
                    password: formData.password,
                    confirmPassword: formData.confirmPassword,
                }),
            });

            if (!response.ok) {
                const errorData = await response.json();
                const errorMessage = errorData.title || errorData.message || "Registration failed. Please try again.";
                setApiError(errorMessage);
                return;
            }

            const data = await response.text();
            console.log("Registration successful:", data);
            alert("Account created successfully! Please sign in.");
            // Optionally redirect to login page
            // window.location.href = "/login";
        } catch (error) {
            console.error("Registration error:", error);
            setApiError("An error occurred during registration. Please try again.");
        } finally {
            setIsLoading(false);
        }
    };

    const handleCreateAccount = () => {
        // Check if terms are agreed
        if (!formData.agreeToTerms) {
            alert("Please agree to the Privacy Policy and Terms of Service.");
            return;
        }

        // Validate form data
        const { isValid, errors: validationErrors } = validateRegistration({
            email: formData.email,
            password: formData.password,
            confirmPassword: formData.confirmPassword,
        });

        setErrors(validationErrors);

        if (isValid) {
            registerUser();
        }
    };

    return (
        <Box sx={{ backgroundColor: theme.palette.background.default, minHeight: "100vh" }}>
            <MyAppBar title={"Sign up"} backUrl={"/"} />

            <Container maxWidth="sm">
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        paddingY: 4,
                    }}
                >
                    {/* Logo e titolo */}
                    <Box
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            gap: 1,
                            marginBottom: 4,
                        }}
                    >
                        <Typography
                            variant="h5"
                            sx={{
                                color: theme.palette.primary.main,
                                display: "flex",
                                alignItems: "center",
                                gap: 1,
                            }}
                        >
                            <FlightIcon sx={{ transform: "rotate(45deg)" }} /> Eppoi
                        </Typography>
                    </Box>

                    {/* Card principale */}
                    <Card
                        sx={{
                            width: "85%",
                            padding: "2rem",
                        }}
                    >
                        {/* Titolo e sottotitolo */}
                        <Typography
                            variant="h4"
                            sx={{
                                marginBottom: 1,
                                color: theme.palette.text.primary,
                            }}
                        >
                            Create Account
                        </Typography>
                        <Typography
                            variant="body2"
                            sx={{
                                color: theme.palette.text.disabled,
                                marginBottom: 3,
                            }}
                        >
                            Join thousands of travelers
                        </Typography>

                        {/* Errore API */}
                        {apiError && (
                            <Box
                                sx={{
                                    backgroundColor: theme.palette.error.light,
                                    color: theme.palette.error.main,
                                    padding: "12px",
                                    borderRadius: "8px",
                                    marginBottom: 2,
                                    fontSize: "14px",
                                }}
                            >
                                {apiError}
                            </Box>
                        )}

                        {/* Form */}
                        <Box sx={{ display: "flex", flexDirection: "column", gap: 2.5 }}>
                            {/* Email */}
                            <FormBox
                                label="Email Address"
                                name="email"
                                type="email"
                                placeholder="you@example.com"
                                value={formData.email}
                                onChange={handleChange}
                                error={errors.email}
                                disabled={isLoading}
                                icon={<EmailIcon />}
                            />

                            {/* Password */}
                            <FormBox
                                label="Password"
                                name="password"
                                type="password"
                                placeholder="•••••••"
                                value={formData.password}
                                onChange={handleChange}
                                error={errors.password}
                                disabled={isLoading}
                                icon={<LockIcon />}
                                showPasswordToggle
                            />

                            {/* Confirm Password */}
                            <FormBox
                                label="Confirm Password"
                                name="confirmPassword"
                                type="password"
                                placeholder="•••••••"
                                value={formData.confirmPassword}
                                onChange={handleChange}
                                error={errors.confirmPassword}
                                disabled={isLoading}
                                icon={<LockIcon />}
                                iconColor={theme.palette.text.disabled}
                            />

                            {/* Checkbox Termini */}
                            <Box sx={{ display: "flex", alignItems: "flex-start", gap: 1 }}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            name="agreeToTerms"
                                            checked={formData.agreeToTerms}
                                            onChange={handleChange}
                                            disabled={isLoading}
                                            sx={{
                                                padding: 0,
                                            }}
                                        />
                                    }
                                    label={
                                        <Typography variant="body2">
                                            I agree to the{" "}
                                            <Link href="#">
                                                Privacy Policy
                                            </Link>{" "}
                                            and{" "}
                                            <Link href="#">
                                                Terms of Service
                                            </Link>
                                        </Typography>
                                    }
                                    sx={{ margin: 0 }}
                                />
                            </Box>

                            {/* Pulsante Create Account */}
                            <MyButton
                                label={isLoading ? "Creating Account..." : "Create Account"}
                                action={handleCreateAccount}
                                disabled={isLoading}
                            />

                            {/* Link Sign In */}
                            <Box
                                sx={{
                                    textAlign: "center",
                                    marginTop: 2,
                                }}
                            >
                                <Typography variant="body2">
                                    Already have an account?{" "}
                                    <Link href="/login">
                                        Sign in
                                    </Link>
                                </Typography>
                            </Box>
                        </Box>
                    </Card>
                </Box>
            </Container>
        </Box>
    );
};

export default RegistrationPage;
