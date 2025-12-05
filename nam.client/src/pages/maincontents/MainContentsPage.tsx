
import React, { useEffect, useState } from "react";
import FlightIcon from "@mui/icons-material/Flight";
import LogoutIcon from "@mui/icons-material/Logout";
import {
    Box,
    Card,
    Container,
    IconButton,
    Typography,
    useTheme,
    CircularProgress,
} from "@mui/material";
import MyAppBar from "../../components/appbar";
import { buildApiUrl } from "../../config";
import ElementCard from "../../components/ElementCardComponent";
import CategorySelect from "../../components/SelectComponent";
import type {
    CategoryOption,
} from "../../components/SelectComponent";

/**
 * Shape of the external events API response.
 */
type ApiEventItem = {
    entityId: string;
    entityName: string;
    imagePath: string;
    badgeText: string;
    address: string;
    date: string;
};

/**
 * Internal element shape used by the UI.
 */
type ElementItem = {
    id: string;
    title: string;
    badge: string;
    address: string;
    imageUrl?: string;
    date?: string;
    category?: string;
};

const IMAGE_BASE_URL = "https://eppoi.io";

const MainContentsPage: React.FC = () => {
    const theme = useTheme();

    // Auth state
    const [loadingAuth, setLoadingAuth] = useState(true);
    const [authenticated, setAuthenticated] = useState(false);

    // Data state
    const [elements, setElements] = useState<ElementItem[]>([]);
    const [loadingElements, setLoadingElements] = useState(false);
    const [elementsError, setElementsError] = useState<string | null>(null);

    // Category state
    const [selectedCategory, setSelectedCategory] = useState<string | null>(
        null
    );

    const categoryOptions: CategoryOption[] = [
        { value: null, label: "All" },
        { value: "ArtCulture", label: "Art&Culture" },
        { value: "Article", label: "Article" },
        { value: "EatAndDrink", label: "Eat&Drink" },
        { value: "EntertainmentLeisure", label: "Entertainment&Leisure" },
        { value: "Events", label: "Events" },
        { value: "Nature", label: "Nature" },
        { value: "Routes", label: "Routes" },
        { value: "Services", label: "Services" },
        { value: "Shopping", label: "Shopping" },
        { value: "Sleep", label: "Sleep" },
        { value: "TypicalProducts", label: "TypicalProducts" }
    ];

    // Check authentication on mount
    useEffect(() => {
        const checkAuth = async () => {
            try {
                const response = await fetch(buildApiUrl("poi/poiList"), {
                    method: "GET",
                    credentials: "include",
                });

                if (response.ok) {
                    setAuthenticated(true);
                } else {
                    window.location.href = "/login";
                }
            } catch (err) {
                console.error("Auth check error:", err);
                window.location.href = "/login";
            } finally {
                setLoadingAuth(false);
            }
        };

        checkAuth();
    }, []);

    // Fetch events from external API once authenticated
    useEffect(() => {
        if (!authenticated) return;

        const fetchElements = async () => {
            try {
                setLoadingElements(true);
                setElementsError(null);

                const url =
                    "https://apispm.eppoi.io/api/events/card-list" +
                    "?municipality=Massignano&language=it";

                const response = await fetch(url, {
                    method: "GET",
                    headers: {
                        Accept: "application/json",
                    },
                });

                if (!response.ok) {
                    throw new Error(
                        `Elements API returned status ${response.status}`
                    );
                }

                const json: unknown = await response.json();
                const data = (json ?? []) as ApiEventItem[];

                const mapped: ElementItem[] = data.map(
                    (item: ApiEventItem, index: number) => {
                        // Build full image URL: /Media/... -> https://eppoi.io/Media/...
                        const rawImagePath = item.imagePath;
                        const imageUrl =
                            rawImagePath && rawImagePath.startsWith("http")
                                ? rawImagePath
                                : rawImagePath
                                    ? `${IMAGE_BASE_URL}${rawImagePath.startsWith("/")
                                        ? ""
                                        : "/"
                                    }${rawImagePath}`
                                    : undefined;

                        return {
                            id: item.entityId?.toString() ?? `element-${index}`,
                            title: item.entityName || "Untitled",
                            badge:
                                item.badgeText || "",
                            address:
                                item.address || "",
                            imageUrl,
                            date: item.date || undefined,
                            category: "Events",
                        };
                    }
                );

                setElements(mapped);
            } catch (err) {
                console.error("Error while fetching elements:", err);
                setElementsError(
                    "Unable to load items. Please try again later."
                );
            } finally {
                setLoadingElements(false);
            }
        };

        fetchElements();
    }, [authenticated]);

    const handleLogout = async () => {
        try {
            const response = await fetch(buildApiUrl("auth/logout"), {
                method: "POST",
                credentials: "include",
            });

            if (!response.ok) {
                console.error("Logout failed");
            }
        } catch (err) {
            console.error("Logout error:", err);
        } finally {
            window.location.href = "/login";
        }
    };

    const filteredElements =
        selectedCategory == null
            ? elements
            : elements.filter((e) => e.category === selectedCategory);

    // Loader while checking auth
    if (loadingAuth) {
        return (
            <Box
                sx={{
                    backgroundColor: theme.palette.background.default,
                    minHeight: "100vh",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                }}
            >
                <CircularProgress />
            </Box>
        );
    }

    if (!authenticated) {
        // If redirect fails, avoid rendering protected content
        return null;
    }

    return (
        <Box
            sx={{
                backgroundColor: theme.palette.background.default,
                minHeight: "100vh",
            }}
        >
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
                    {/* Centered logo */}
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
                            <FlightIcon sx={{ transform: "rotate(45deg)" }} />{" "}
                            Eppoi
                        </Typography>
                    </Box>

                    {/* Main container card */}
                    <Card
                        sx={{
                            width: "85%",
                            padding: "1.5rem",
                            borderRadius: "1rem",
                            boxShadow: theme.shadows[3],
                            backgroundColor: theme.palette.background.paper,
                        }}
                    >
                        {/* Category select */}
                        <CategorySelect
                            label="Category"
                            value={selectedCategory}
                            options={categoryOptions}
                            onChange={(val) => setSelectedCategory(val)}
                        />

                        {/* Elements list */}
                        {loadingElements ? (
                            <Box
                                sx={{
                                    display: "flex",
                                    justifyContent: "center",
                                    alignItems: "center",
                                    paddingY: 2,
                                }}
                            >
                                <CircularProgress size={28} />
                            </Box>
                        ) : elementsError ? (
                            <Typography
                                variant="body2"
                                sx={{ color: theme.palette.error.main, mt: 2 }}
                            >
                                {elementsError}
                            </Typography>
                        ) : filteredElements.length === 0 ? (
                            <Typography
                                variant="body2"
                                sx={{
                                    color: theme.palette.text.disabled,
                                    mt: 2,
                                }}
                            >
                                No items available for the selected category.
                            </Typography>
                        ) : (
                            <Box
                                sx={{
                                    display: "flex",
                                    flexDirection: "column",
                                    gap: 2,
                                    marginTop: 2,
                                }}
                            >
                                {filteredElements.map((item) => (
                                    <ElementCard
                                        key={item.id}
                                        title={item.title}
                                        badge={item.badge}
                                        address={item.address}
                                        imageUrl={item.imageUrl}
                                        date={item.date}
                                    />
                                ))}
                            </Box>
                        )}
                    </Card>
                </Box>
            </Container>
        </Box>
    );
};

export default MainContentsPage;