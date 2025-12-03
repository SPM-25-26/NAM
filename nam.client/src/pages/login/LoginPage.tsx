import { useState } from "react";
import EmailIcon from "@mui/icons-material/Email";
import LockIcon from "@mui/icons-material/Lock";
import FlightIcon from "@mui/icons-material/Flight";
import {
  Box,
  Card,
  Link,
  Typography,
  Container,
  useTheme,
} from "@mui/material";
import MyAppBar from "../../components/appbar";
import MyButton from "../../components/button";
import FormBox from "../../components/FormBox";
import { buildApiUrl } from "../../config";

const LoginPage: React.FC = () => {
  const theme = useTheme();

  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const [isLoading, setIsLoading] = useState(false);
  const [apiError, setApiError] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));

    // Clear API error when user modifies form
    if (apiError) {
      setApiError(null);
    }
  };

  // Calls backend to authenticate user and retrieve JWT token
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
        let errorMessage =
          "Login failed. Please check your credentials and try again.";

        try {
          const errorData = await response.json();
          errorMessage = errorData.title || errorData.message || errorMessage;
        } catch {
          // If response body is not JSON, keep default error message
        }

        setApiError(errorMessage);
        return;
      }

      const data = await response.json();
      console.log(data.message);

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
    <Box
      sx={{
        backgroundColor: theme.palette.background.default,
        minHeight: "100vh",
      }}
    >
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
          {/* Logo and title */}
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

          {/* Main card */}
          <Card
            sx={{
              width: "85%",
              padding: "2rem",
            }}
          >
            {/* Title and subtitle */}
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

            {/* API error */}
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
                disabled={isLoading}
                icon={<EmailIcon />}
              />

              {/* Password */}
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

              {/* Forgot password link */}
              <Box
                sx={{
                  display: "flex",
                  justifyContent: "flex-end",
                  marginTop: -1,
                }}
              >
                <Typography variant="body2">
                  <Link href="/resetPassword">Forgot your password?</Link>
                </Typography>
              </Box>

              {/* Login button */}
              <MyButton
                label={isLoading ? "Logging in..." : "Login"}
                action={handleLoginClick}
                disabled={isLoading}
              />

              {/* Sign up link */}
              <Box
                sx={{
                  textAlign: "center",
                  marginTop: 2,
                }}
              >
                <Typography variant="body2">
                  You are not already registered?{" "}
                  <Link href="/signup">Sign up</Link>
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
