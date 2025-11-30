// src/pages/main/MainContentsPage.tsx (adatta il path se serve)
import React from "react";
import FlightIcon from "@mui/icons-material/Flight";
import LogoutIcon from "@mui/icons-material/Logout";
import {
    Box,
    Card,
    Container,
    IconButton,
    Typography,
    useTheme,
} from "@mui/material";
import MyAppBar from "../../components/appbar";
import { buildApiUrl } from "../../config";

const MainContentsPage: React.FC = () => {
    const theme = useTheme();

    const handleLogout = async () => {
        try {
            // call backend for logout
            const response = await fetch(buildApiUrl("auth/logout"), {
                method: "POST",
                credentials: "include", // Send the cookie
            });

            if (!response.ok) {
                console.error("Logout failed");
            } else {
                window.location.href = "/login";
            }
        } catch (err) {
            console.error("Logout error:", err);
        } finally {
            //window.location.href = "/signup";
        }
        console.log("Logout clicked");
    };

    return (
        <Box sx={{ backgroundColor: theme.palette.background.default, minHeight: "100vh" }}>
            <MyAppBar title="" backUrl="" />

            {/* Logout icon */}
            <Box
                sx={{
                    position: "fixed",
                    top: 8,
                    right: 16,
                    zIndex: (theme) => theme.zIndex.appBar + 1,
                }}
            >
                <IconButton
                    onClick={handleLogout}
                    aria-label="Logout"
                    sx={{
                        color: theme.palette.text.primary,
                    }}
                >
                    <LogoutIcon />
                </IconButton>
            </Box>

            <Container maxWidth="sm">
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        paddingY: 4,
                    }}
                >
                    {/* Centred Logo */}
                    <Box
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
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

                    {/* Main card, for now empty*/}
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
                            Main content
                        </Typography>
                        <Typography
                            variant="body2"
                            sx={{
                                color: theme.palette.text.disabled,
                            }}
                        >
                            This area will contain the main content of the application.
                        </Typography>
                    </Card>
                </Box>
            </Container>
        </Box>
    );
};

export default MainContentsPage;