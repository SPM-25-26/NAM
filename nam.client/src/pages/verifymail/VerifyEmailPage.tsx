import React, { useEffect, useState, useRef } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import {
    Box,
    Typography,
    Card,
    Container,
    useTheme,
    CircularProgress
} from "@mui/material";
import { CheckCircleOutline, HighlightOff } from "@mui/icons-material";
import FlightIcon from "@mui/icons-material/Flight";
import MyButton from "../../components/button";
import { buildApiUrl } from "../../config";

const VerifyEmailPage: React.FC = () => {
    const theme = useTheme();
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    const [status, setStatus] = useState<string>("loading");
    const [message, setMessage] = useState<string>("Verifying your email...");

    const hasFetched = useRef(false);

    useEffect(() => {
        const verifyEmail = async () => {
            const email = searchParams.get("email");
            const token = searchParams.get("token");

            if (!email || !token) {
                setStatus("error");
                setMessage("Invalid verification link. Missing parameters.");
                return;
            }

            try {
                const response = await fetch(buildApiUrl("auth/verify-email"), {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ email, token }),
                });

                if (response.ok) {
                    const data = await response.json();
                    setStatus("success");
                    setMessage(data.message || "Your email has been successfully verified.");
                } else {
                    const errorData = await response.json();
                    setStatus("error");
                    setMessage(errorData.detail || "Verification failed. The link may be invalid or expired.");
                }

            } catch (error) {
                console.error("Verification error:", error);
                setStatus("error");
                setMessage("An unexpected error occurred. Please try again later.");
            }
        };

        if (!hasFetched.current) {
            hasFetched.current = true;
            verifyEmail();
        }
    }, [searchParams]);

    const handleGoToLogin = () => {
        navigate("/login");
    };

    return (
        <Box sx={{ backgroundColor: theme.palette.background.default, minHeight: "100vh" }}>
            <Container maxWidth="sm">
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        justifyContent: "center",
                        minHeight: "100vh",
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
                                fontWeight: 600,
                            }}
                        >
                            <FlightIcon sx={{ transform: "rotate(45deg)" }} /> Eppoi
                        </Typography>
                    </Box>

                    <Card
                        sx={{
                            width: "85%",
                            padding: "3rem 2rem",
                            textAlign: "center",
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "center",
                            minHeight: "300px",
                            justifyContent: "center"
                        }}
                    >
                        {status === "loading" && (
                            <>
                                <CircularProgress size={60} sx={{ mb: 3 }} />
                                <Typography variant="h6" color="text.secondary">
                                    Verifying...
                                </Typography>
                            </>
                        )}

                        {status === "success" && (
                            <>
                                <CheckCircleOutline
                                    color="success"
                                    sx={{ fontSize: 80, mb: 2 }}
                                />
                                <Typography
                                    variant="h4"
                                    sx={{
                                        mb: 1,
                                        color: theme.palette.text.primary,
                                        fontWeight: 600
                                    }}
                                >
                                    Verified!
                                </Typography>
                                <Typography
                                    variant="body1"
                                    sx={{
                                        mb: 4,
                                        color: theme.palette.text.secondary
                                    }}
                                >
                                    {message}
                                </Typography>
                                <Box sx={{ width: "100%" }}>
                                    <MyButton label="Sign in" action={handleGoToLogin} />
                                </Box>
                            </>
                        )}

                        {status === "error" && (
                            <>
                                <HighlightOff
                                    color="error"
                                    sx={{ fontSize: 80, mb: 2 }}
                                />
                                <Typography
                                    variant="h4"
                                    sx={{
                                        mb: 1,
                                        color: theme.palette.text.primary,
                                        fontWeight: 600
                                    }}
                                >
                                    Error
                                </Typography>
                                <Typography
                                    variant="body1"
                                    sx={{
                                        mb: 4,
                                        color: theme.palette.text.secondary
                                    }}
                                >
                                    {message}
                                </Typography>
                                <Box sx={{ width: "100%" }}>
                                    <MyButton label="Back to Login" action={handleGoToLogin} />
                                </Box>
                            </>
                        )}
                    </Card>
                </Box>
            </Container>
        </Box>
    );
};

export default VerifyEmailPage;