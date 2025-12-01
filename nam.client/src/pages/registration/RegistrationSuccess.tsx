import React from "react";
import { Box, Typography, Card, Container, useTheme } from "@mui/material";
import { CheckCircleOutline } from "@mui/icons-material";
import FlightIcon from "@mui/icons-material/Flight";
import MyButton from "../../components/button";

interface RegistrationSuccessProps {
    handleGoToLogin: () => void;
}

const StepComplete: React.FC<RegistrationSuccessProps> = ({ handleGoToLogin }) => {
    const theme = useTheme();

    return (
        <Box sx={{ backgroundColor: theme.palette.background.default, minHeight: "100vh" }}>
            <Container maxWidth="sm">
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        justifyContent: "center",
                        minHeight: "100vh", // Centers content vertically
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

                    {/* Main Card */}
                    <Card
                        sx={{
                            width: "85%",
                            padding: "3rem 2rem",
                            textAlign: "center",
                            display: "flex",
                            flexDirection: "column",
                            alignItems: "center",
                        }}
                    >
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
                            Success!
                        </Typography>

                        <Typography
                            variant="body1"
                            sx={{
                                mb: 4,
                                color: theme.palette.text.secondary
                            }}
                        >
                            You have successfully signed up.
                        </Typography>

                        <Box sx={{ width: "100%" }}>
                            <MyButton label="Sign in" action={handleGoToLogin} />
                        </Box>
                    </Card>
                </Box>
            </Container>
        </Box>
    );
};

export default StepComplete;