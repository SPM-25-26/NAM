import React from "react";
import FlightIcon from "@mui/icons-material/Flight";
import {
    Box,
    Card,
    Container,
    Typography,
    useTheme,
} from "@mui/material";
import MyAppBar from "../../components/appbar";
import MyButton from "../../components/button";

const HomePage: React.FC = () => {
    const theme = useTheme();

    const handleSignInClick = () => {
        window.location.href = "/login";
    };

    const handleSignUpClick = () => {
        window.location.href = "/signup";
    };

    return (
        <Box sx={{ backgroundColor: theme.palette.background.default, minHeight: "100vh" }}>
            <MyAppBar title={"Homepage"} backUrl="" />

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
                            Welcome
                        </Typography>
                        <Typography
                            variant="body2"
                            sx={{
                                color: theme.palette.text.disabled,
                                marginBottom: 3,
                            }}
                        >
                            Welcome to Eppoi, your travel companion. Sign in if you already
                            have an account, or sign up to start your journey.
                        </Typography>

                        <Box
                            sx={{
                                display: "flex",
                                flexDirection: "column",
                                gap: 2,
                                marginTop: 2,
                            }}
                        >
                            <MyButton
                                label="Sign in"
                                action={handleSignInClick}
                            />

                            <MyButton
                                label="Sign up"
                                action={handleSignUpClick}
                            />

                            <Box
                                sx={{
                                    textAlign: "center",
                                    marginTop: 2,
                                }}
                            >
                            </Box>
                        </Box>
                    </Card>
                </Box>
            </Container>
        </Box>
    );
};

export default HomePage;
