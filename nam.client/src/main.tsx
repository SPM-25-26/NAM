import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createTheme, ThemeProvider} from "@mui/material";
import "./index.css";
import App from "./App.tsx";


export const theme = createTheme({
    palette: {
        primary: {
            main: "#155DFC",
        },
        secondary: {
            main: "#1976d2",
        },
        background: {
            default: "#f5f5f5",
            paper: "#ffffff",
        },
        text: {
            primary: "#000000",
            secondary: "#666666",
            disabled: "#999999",
        },
        error: {
            main: "#d32f2f",
            light: "#ffebee",
        },
    },
    typography: {
        h1: {
            fontSize: "3rem",
            fontWeight: 600,
        },
        h2: {
            fontSize: "1.75rem",
            fontWeight: 600,
        },
        h3: {
            fontSize: "1.5rem",
            fontWeight: 600,
        },
        h4: {
            fontWeight: 600,
        },
        h5: {
            fontWeight: 600,
        },
        body2: {
            color: "#666666",
        },
    },
    shape: {
        borderRadius: 16,
    },
    components: {
        MuiCard: {
            styleOverrides: {
                root: {
                    boxShadow: "0 2px 8px rgba(0,0,0,0.1)",
                },
            },
        },
        MuiCheckbox: {
            styleOverrides: {
                root: {
                    "&.Mui-checked": {
                        color: "#1976d2",
                    },
                },
            },
        },
        MuiLink: {
            styleOverrides: {
                root: {
                    color: "#1976d2",
                    textDecoration: "none",
                    fontWeight: 600,
                    cursor: "pointer",
                },
            },
        },
    },
})


createRoot(document.getElementById("root")!).render(
    <StrictMode>
        <ThemeProvider theme = {theme}>
            <App />
        </ThemeProvider>
    </StrictMode>
);
