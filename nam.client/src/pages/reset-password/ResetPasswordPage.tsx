import React from "react";
import MyStepper from "./components/MyStepper";
import MyAppBar from "../../components/appbar";
import { useTheme, Box, Card, Container, Typography } from "@mui/material";
import FlightIcon from '@mui/icons-material/Flight';

const ResetPasswordPage: React.FC = () => {
    const theme = useTheme();
    return (
        <Box sx={{
            backgroundColor: theme.palette.background.default,
            minHeight: "100vh",
        }}>
            <MyAppBar title={"Reset Password"} backUrl={"/"} />
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

                    <Card sx={{
                        width: "100%",
                        padding: "2rem",
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center"
                    }}>
                            <MyStepper />
                    </Card>
                </Box>
            </Container>
        </Box>
    );
};


export default ResetPasswordPage;
