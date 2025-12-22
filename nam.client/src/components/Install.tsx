import { useEffect, useState } from "react";
import { Button, Snackbar, Alert } from "@mui/material";
import GetAppIcon from "@mui/icons-material/GetApp";

export const InstallPwaButton = () => {
    const [deferredPrompt, setDeferredPrompt] = useState<any>(null);
    const [showBanner, setShowBanner] = useState(false);
    const [isIOS, setIsIOS] = useState(false);

    useEffect(() => {
        // 1. Control if we are on iOS (because the automatic prompt is not there)
        const isIosDevice =
            /iPad|iPhone|iPod/.test(navigator.userAgent) && !(window as any).MSStream;
        setIsIOS(isIosDevice);

        // 2. Listen for the 'beforeinstallprompt' event (Android/Desktop)
        const handler = (e: Event) => {
            // Block the native browser automatic banner
            e.preventDefault();
            // Save the event to trigger it later with our button
            setDeferredPrompt(e);
            // Show our button/banner
            setShowBanner(true);
        };

        window.addEventListener("beforeinstallprompt", handler);

        return () => window.removeEventListener("beforeinstallprompt", handler);
    }, []);

    const handleInstallClick = async () => {
        if (!deferredPrompt) return;

        // Show the native installation prompt
        deferredPrompt.prompt();

        // Wait for the user's response
        const { outcome } = await deferredPrompt.userChoice;
        console.log(`User response to install prompt: ${outcome}`);

        // Reset everything
        setDeferredPrompt(null);
        setShowBanner(false);
    };

    // If the app is already installed (or not supported), don't show anything
    if (!showBanner && !isIOS) return null;

    return (
        <>
            {/* Case Android / Desktop: real button */}
            <Snackbar
                open={showBanner}
                anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
                message="Install the app for a better experience!"
            >
                <Alert
                    severity="info"
                    action={
                        <Button color="inherit" size="small" onClick={handleInstallClick} startIcon={<GetAppIcon />}>
                            INSTALL
                        </Button>
                    }
                >
                    Do you want to install the App?
                </Alert>
            </Snackbar>

            {/* Case iOS: Static instructions */}
            {isIOS && (
                <Snackbar open={true} autoHideDuration={6000} onClose={() => { }}>
                    <Alert severity="info" icon={<GetAppIcon fontSize="inherit" />}>
                        To install on iOS: Press Share and then "Add to Home"
                    </Alert>
                </Snackbar>
            )}
        </>
    );
};