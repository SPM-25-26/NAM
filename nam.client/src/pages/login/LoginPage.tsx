import { useState, useEffect } from "react";
import EmailIcon from "@mui/icons-material/Email";
import LockIcon from "@mui/icons-material/Lock";
import FlightIcon from "@mui/icons-material/Flight";
import {
    Box,
    Card,
    Link,
    Typography,
    Container,
    useTheme
} from "@mui/material";
import MyAppBar from "../../components/appbar";
import MyButton from "../../components/button";
import FormBox from "../../components/FormBox";
import { buildApiUrl } from "../../config";

interface ProblemDetails {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
}

const LoginPage: React.FC = () => {
    const theme = useTheme();

    const [formData, setFormData] = useState({
        email: "",
        password: "",
    });

    const [isLoading, setIsLoading] = useState(false);
    const [apiError, setApiError] = useState<string | null>(null);

    useEffect(() => {
        const checkSession = async () => {
            try {
                const response = await fetch(buildApiUrl("auth/validate-token"), {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    credentials: "include",
                });

                if (response.ok) {
                    window.location.href = "/maincontents";
                }
            } catch {
                // 
            }
        };

        checkSession();
    }, []);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));

        if (apiError) {
            setApiError(null);
        }
    };

    const loginUser = async () => {
        try {
            setIsLoading(true);
            setApiError(null);

            const response = await fetch(buildApiUrl("auth/login"), {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                credentials: "include",
                body: JSON.stringify({
                    email: formData.email,
                    password: formData.password,
                }),
            });

            if (!response.ok) {
                let errorMessage = "Login failed. Please check your credentials.";
                try {
                    const errorData = (await response.json()) as ProblemDetails;
                    if (errorData.detail) {
                        errorMessage = errorData.detail;
                    }
                } catch {
                    //
                }
                setApiError(errorMessage);
                return;
            }

            window.location.href = "/maincontents";

        } catch (error) {
            console.error("Login error:", error);
            setApiError("An error occurred during login. Please try again.");
        } finally {
            setIsLoading(false);
        }
    };

    const handleLoginClick = () => {
        if (!formData.email || !formData.password) {
            setApiError("Please enter both email and password.");
            return;
        }

        loginUser();
    };

    return (
        <Box sx={{ backgroundColor: theme.palette.background.default, minHeight: "100vh" }}>
            <MyAppBar title={"Sign in"} backUrl={"/"} />

            <Container maxWidth="sm">
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        paddingY: 4,
                    }}
                >
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

                    <Card
                        sx={{
                            width: "85%",
                            padding: "2rem",
                        }}
                    >
                        <Typography
                            variant="h4"
                            sx={{
                                marginBottom: 1,
                                color: theme.palette.text.primary,
                            }}
                        >
                            Sign in
                        </Typography>
                        <Typography
                            variant="body2"
                            sx={{
                                color: theme.palette.text.disabled,
                                marginBottom: 3,
                            }}
                        >
                            Welcome back, traveler
                        </Typography>

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

                        <Box sx={{ display: "flex", flexDirection: "column", gap: 2.5 }}>
                            <FormBox
                                label="Email Address"
                                name="email"
                                type="email"
                                placeholder="you@example.com"
                                value={formData.email}
                                onChange={handleChange}
                                disabled={isLoading}
                                icon={<EmailIcon />}
                            />

                            <FormBox
                                label="Password"
                                name="password"
                                type="password"
                                placeholder="Password"
                                value={formData.password}
                                onChange={handleChange}
                                disabled={isLoading}
                                icon={<LockIcon />}
                                showPasswordToggle
                            />

                            <Box
                                sx={{
                                    display: "flex",
                                    justifyContent: "flex-end",
                                    marginTop: -1,
                                }}
                            >
                                <Link href="/resetPassword">
                                    Forgot your password?
                                </Link>
                            </Box>

                            <MyButton
                                label={isLoading ? "Logging in..." : "Login"}
                                action={handleLoginClick}
                                disabled={isLoading}
                            />

                            <Box
                                sx={{
                                    textAlign: "center",
                                    marginTop: 2,
                                }}
                            >
                                <Typography variant="body2">
                                    You are not already registered?{" "}
                                    <Link href="/signup">
                                        Sign up
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

export default LoginPage;