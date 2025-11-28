import { useState } from "react";
import EmailIcon from '@mui/icons-material/Email';
import LockIcon from '@mui/icons-material/Lock';
import FlightIcon from '@mui/icons-material/Flight';
import {
    Box,
    Card,
    TextField,
    FormControlLabel,
    Checkbox,
    Link,
    Typography,
    Container,
} from "@mui/material";
import MyAppBar from "../../components/appbar";
import MyButton from "../../components/button";

const RegistrationPage: React.FC = () => {
    const [formData, setFormData] = useState({
        email: "",
        password: "",
        confirmPassword: "",
        agreeToTerms: false,
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value, type, checked } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const handleCreateAccount = () => {
        console.log("Account creation:", formData);
    };

    return (
        <Box sx={{ backgroundColor: "#f5f5f5", minHeight: "100vh" }}>
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
                                color: "#1976d2",
                                fontWeight: 600,
                                display: "flex",
                                alignItems: "center",
                                gap: 1,
                            }}
                        >
                            <FlightIcon /> Eppoi
                        </Typography>
                    </Box>

                    {/* Card principale */}
                    <Card
                        sx={{
                            width: "100%",
                            padding: 4,
                            boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
                        }}
                    >
                        {/* Titolo e sottotitolo */}
                        <Typography
                            variant="h4"
                            sx={{
                                fontWeight: 600,
                                marginBottom: 1,
                                color: "#000",
                            }}
                        >
                            Create Account
                        </Typography>
                        <Typography
                            variant="body2"
                            sx={{
                                color: "#999",
                                marginBottom: 3,
                            }}
                        >
                            Join thousands of travelers
                        </Typography>

                        {/* Form */}
                        <Box sx={{ display: "flex", flexDirection: "column", gap: 2.5 }}>
                            {/* Email */}
                            <Box>
                                <Typography
                                    variant="subtitle2"
                                    sx={{
                                        fontWeight: 600,
                                        marginBottom: 1,
                                        color: "#000",
                                    }}
                                >
                                    Email Address
                                </Typography>
                                <TextField
                                    fullWidth
                                    name="email"
                                    type="email"
                                    placeholder="you@example.com"
                                    value={formData.email}
                                    onChange={handleChange}
                                    variant="outlined"
                                    sx={{
                                        backgroundColor: "#f5f5f5",
                                        borderRadius: 1,
                                        "& .MuiOutlinedInput-root": {
                                            "& fieldset": {
                                                borderColor: "#e0e0e0",
                                            },
                                        },
                                    }}
                                    InputProps={{
                                        startAdornment: (
                                            <Typography sx={{ marginRight: 1, color: "#999" }}>
                                                <EmailIcon />
                                            </Typography>
                                        ),
                                    }}
                                />
                            </Box>

                            {/* Password */}
                            <Box>
                                <Typography
                                    variant="subtitle2"
                                    sx={{
                                        fontWeight: 600,
                                        marginBottom: 1,
                                        color: "#000",
                                    }}
                                >
                                    Password
                                </Typography>
                                <TextField
                                    fullWidth
                                    name="password"
                                    type="password"
                                    placeholder="•••••••"
                                    value={formData.password}
                                    onChange={handleChange}
                                    variant="outlined"
                                    sx={{
                                        backgroundColor: "#f5f5f5",
                                        borderRadius: 1,
                                        "& .MuiOutlinedInput-root": {
                                            "& fieldset": {
                                                borderColor: "#e0e0e0",
                                            },
                                        },
                                    }}
                                    InputProps={{
                                        startAdornment: (
                                            <LockIcon />
                                        ),
                                    }}
                                />
                            </Box>

                            {/* Confirm Password */}
                            <Box>
                                <Typography
                                    variant="subtitle2"
                                    sx={{
                                        fontWeight: 600,
                                        marginBottom: 1,
                                        color: "#000",
                                    }}
                                >
                                    Confirm Password
                                </Typography>
                                <TextField
                                    fullWidth
                                    name="confirmPassword"
                                    type="password"
                                    placeholder="•••••••"
                                    value={formData.confirmPassword}
                                    onChange={handleChange}
                                    variant="outlined"
                                    sx={{
                                        backgroundColor: "#f5f5f5",
                                        borderRadius: 1,
                                        "& .MuiOutlinedInput-root": {
                                            "& fieldset": {
                                                borderColor: "#e0e0e0",
                                            },
                                        },
                                    }}
                                    InputProps={{
                                        startAdornment: (
                                            <Typography sx={{ marginRight: 1, color: "#999" }}>
                                                <LockIcon />
                                            </Typography>
                                        ),
                                    }}
                                />
                            </Box>

                            {/* Checkbox Termini */}
                            <Box sx={{ display: "flex", alignItems: "flex-start", gap: 1 }}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            name="agreeToTerms"
                                            checked={formData.agreeToTerms}
                                            onChange={handleChange}
                                            sx={{
                                                padding: 0,
                                                "&.Mui-checked": {
                                                    color: "#1976d2",
                                                },
                                            }}
                                        />
                                    }
                                    label={
                                        <Typography variant="body2" sx={{ color: "#666" }}>
                                            I agree to the{" "}
                                            <Link
                                                href="#"
                                                sx={{
                                                    color: "#1976d2",
                                                    textDecoration: "none",
                                                    fontWeight: 600,
                                                }}
                                            >
                                                Privacy Policy
                                            </Link>{" "}
                                            and{" "}
                                            <Link
                                                href="#"
                                                sx={{
                                                    color: "#1976d2",
                                                    textDecoration: "none",
                                                    fontWeight: 600,
                                                }}
                                            >
                                                Terms of Service
                                            </Link>
                                        </Typography>
                                    }
                                    sx={{ margin: 0 }}
                                />
                            </Box>

                            {/* Pulsante Create Account */}
                            <MyButton
                                label="Create Account"
                                action={handleCreateAccount}
                            />

                            {/* Link Sign In */}
                            <Box
                                sx={{
                                    textAlign: "center",
                                    marginTop: 2,
                                }}
                            >
                                <Typography variant="body2" sx={{ color: "#666" }}>
                                    Already have an account?{" "}
                                    <Link
                                        href="/login"
                                        sx={{
                                            color: "#1976d2",
                                            textDecoration: "none",
                                            fontWeight: 600,
                                            cursor: "pointer",
                                        }}
                                    >
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