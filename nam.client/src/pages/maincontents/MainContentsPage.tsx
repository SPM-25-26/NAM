import React, { useEffect, useMemo, useState } from "react";
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
import { buildApiUrl } from "../../config";
import ElementCard from "../../components/ElementCardComponent";
import CategorySelect from "../../components/SelectComponent";
import type { CategoryOption } from "../../components/SelectComponent";

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
    const [selectedCategory, setSelectedCategory] = useState<string | null>(null);
    const [selectedBadge, setSelectedBadge] = useState<string | null>(null);

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
        { value: "TypicalProducts", label: "TypicalProducts" },
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
                        const rawImagePath = item.imagePath;

                        // remove "-thumb" everywhere to get higher-res image
                        const cleanedPath =
                            rawImagePath
                                ?.replace(/-thumb-/g, "-")
                                .replace(/-thumb(?=\.[^.]+$)/, "") || rawImagePath;

                        const imageUrl =
                            cleanedPath && cleanedPath.startsWith("http")
                                ? cleanedPath
                                : cleanedPath
                                    ? `${IMAGE_BASE_URL}${cleanedPath.startsWith("/") ? "" : "/"
                                    }${cleanedPath}`
                                    : undefined;

                        return {
                            id: item.entityId?.toString() ?? `element-${index}`,
                            title: item.entityName || "Untitled",
                            badge: item.badgeText || "",
                            address: item.address || "",
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

    // Build dynamic badge options from fetched elements
    const uniqueBadgeOptions: CategoryOption[] = useMemo(() => {
        const badges = new Set<string>();
        elements.forEach((el) => {
            if (el.badge) badges.add(el.badge);
        });
        return [
            { value: null, label: "Tutti i badge" },
            ...Array.from(badges).map((b) => ({ value: b, label: b })),
        ];
    }, [elements]);

    const filteredElements = elements.filter((e) => {
        const categoryOk =
            selectedCategory == null || e.category === selectedCategory;
        const badgeOk = selectedBadge == null || e.badge === selectedBadge;
        return categoryOk && badgeOk;
    });

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

            <Container maxWidth="lg">
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                        paddingTop: 1,
                        paddingBottom: 4,
                    }}
                >
                    {/* Header row: logo centered, logout aligned right */}
                    <Box
                        sx={{
                            display: "flex",
                            alignItems: "center",
                            width: "100%",
                            mb: 2.5,
                        }}
                    >
                        <Box sx={{ flex: 1, minWidth: 48 }} />
                        <Box
                            sx={{
                                flex: 1,
                                display: "flex",
                                justifyContent: "center",
                                alignItems: "center",
                                gap: 1,
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
                        <Box
                            sx={{
                                flex: 1,
                                display: "flex",
                                justifyContent: "flex-end",
                                alignItems: "center",
                            }}
                        >
                            <IconButton
                                onClick={handleLogout}
                                aria-label="Logout"
                                color="primary"
                            >
                                <LogoutIcon />
                            </IconButton>
                        </Box>
                    </Box>

                    {/* Main container card */}
                    <Card
                        sx={{
                            width: "100%",
                            padding: "2.4rem",
                            borderRadius: "1.2rem",
                            boxShadow: theme.shadows[3],
                            backgroundColor: theme.palette.background.paper,
                        }}
                    >
                        {/* Two selects side by side */}
                        <Box
                            sx={{
                                display: "flex",
                                flexDirection: { xs: "column", sm: "row" },
                                gap: 2,
                            }}
                        >
                            <Box sx={{ flex: 1, minWidth: 0 }}>
                                <CategorySelect
                                    label="Category"
                                    value={selectedCategory}
                                    options={categoryOptions}
                                    onChange={(val) => setSelectedCategory(val)}
                                />
                            </Box>
                            <Box sx={{ flex: 1, minWidth: 0 }}>
                                <CategorySelect
                                    label="Badge"
                                    value={selectedBadge}
                                    options={uniqueBadgeOptions}
                                    onChange={(val) => setSelectedBadge(val)}
                                    accentColor={"#9810fa"}
                                />
                            </Box>
                        </Box>

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
                                No items available for the selected filters.
                            </Typography>
                        ) : (
                            <Box
                                sx={{
                                    display: "grid",
                                    gridTemplateColumns: {
                                        xs: "1fr",
                                        sm: "repeat(2, minmax(0, 1fr))",
                                        md: "repeat(3, minmax(0, 1fr))",
                                        lg: "repeat(3, minmax(0, 1fr))",
                                    },
                                    gap: 2.5,
                                    marginTop: 2,
                                }}
                            >
                                {filteredElements.map((item) => (
                                    <Box key={item.id} sx={{ height: "100%" }}>
                                        <ElementCard
                                            title={item.title}
                                            badge={item.badge}
                                            address={item.address}
                                            imageUrl={item.imageUrl}
                                            date={item.date}
                                        />
                                    </Box>
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